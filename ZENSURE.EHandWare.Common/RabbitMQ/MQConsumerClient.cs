using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENSURE.EHandWare.ICommon.RabbitMQ;
using ZENSURE.EHandWare.Models.Options;

namespace ZENSURE.EHandWare.Common.RabbitMQ
{
    public class MQConsumerClient : IMQConsumerClient
    {
        #region  构造函数以及字段

        private readonly IRabbitMQClient _client;
        private readonly IConnection _connection;

        private static readonly ConcurrentQueue<Task> Tasks = new ConcurrentQueue<Task>(); //线程池
        private static readonly ConcurrentQueue<Task> TasksCache = new ConcurrentQueue<Task>(); //缓存队列
        private readonly int _maxTaskCount;
        private long _maxRecord = 1;

        public MQConsumerClient(IRabbitMQClient client, IOptions<RabbitMQOption> options)
        {
            var rabbitMQ = options.Value;
            _client = client;
            _maxTaskCount = rabbitMQ.MaxTaskCount;
            _connection = _client.GetMQConnection();
            TaskDo();
        }

        #endregion

        #region 公有函数

        /// <summary>
        /// 接受通道队列消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="qName"></param>
        /// <param name="allwaysRunAction">执行动作</param>
        /// <param name="isTask"></param>
        public void Listen<T>(string qName, Func<T, bool> allwaysRunAction, bool isTask = false) where T : class
        {
            if (allwaysRunAction == null || string.IsNullOrEmpty(qName)) return;

            IModel listenChannel = _connection.CreateModel();
            listenChannel.QueueDeclare(qName, true, false, false, null);
            var consumer = new EventingBasicConsumer(listenChannel);
            //公平分发,不要同一时间给一个工作者发送多于一个消息
            listenChannel.BasicQos(0, 50, false);
            // 消费消息；false 为手动应答 
            listenChannel.BasicConsume(qName, false, consumer);
            consumer.Received += (model, ea) =>
            {
                byte[] bytes = ea.Body;
                var resp = SerializeObject<T>(ea.Body);
                if (!isTask)
                {
                    Execute(listenChannel, allwaysRunAction, resp, ea.DeliveryTag);
                }
                else
                {
                    var task = new Task(
                        () => { Execute(listenChannel, allwaysRunAction, resp, ea.DeliveryTag); });
                    if (Tasks.Count > _maxTaskCount)
                        TasksCache.Enqueue(task);
                    else
                        Tasks.Enqueue(task);
                }
            };
        }

        /// <summary>
        /// 接受交换机绑定通道消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchangeName"></param>
        /// <param name="allwaysRunAction"></param>
        /// <param name="routingKey"></param>
        /// <param name="exchangeType"></param>
        /// <param name="isTask"></param>
        public void ListenByExchange<T>(string exchangeName, Func<T, bool> allwaysRunAction, string routingKey = "", string exchangeType = "direct", bool isTask = false) where T : class
        {
            if (allwaysRunAction == null || string.IsNullOrEmpty(exchangeName) || string.IsNullOrEmpty(exchangeType) || routingKey == null) return;

            IModel listenChannel = _connection.CreateModel();
            listenChannel.ExchangeDeclare(exchangeName, exchangeType, durable: true);
            var queueName = listenChannel.QueueDeclare().QueueName;
            listenChannel.QueueBind(queueName, exchangeName, routingKey);
            //定义这个队列的消费者
            var consumer = new EventingBasicConsumer(listenChannel);
            //公平分发,不要同一时间给一个工作者发送多于一个消息
            listenChannel.BasicQos(0, 50, false);
            // 消费消息；false 为手动应答 
            listenChannel.BasicConsume(routingKey, false, consumer);
            consumer.Received += (model, ea) =>
            {
                byte[] bytes = ea.Body;
                var resp = SerializeObject<T>(ea.Body);
                if (!isTask)
                {
                    Execute(listenChannel, allwaysRunAction, resp, ea.DeliveryTag);
                }
                else
                {
                    var task = new Task(
                        () => { Execute(listenChannel, allwaysRunAction, resp, ea.DeliveryTag); });
                    if (Tasks.Count > _maxTaskCount)
                        TasksCache.Enqueue(task);
                    else
                        Tasks.Enqueue(task);
                }
            };
        }

        #endregion

        #region 私有函数

        private T SerializeObject<T>(byte[] msg) where T : class
        {
            if (msg == null) return default(T);
            T msgbyte = null;
            var msgstr = Encoding.UTF8.GetString(msg);
            msgbyte = JsonConvert.DeserializeObject<T>(msgstr);
            return msgbyte;
        }

        private void Execute<T>(IModel listenChannel, Func<T, bool> allwaysRunAction, T resp, ulong deliveryTag)
        {
            if (listenChannel == null) return;
            if (listenChannel.IsClosed)
            {
                listenChannel = _connection.CreateModel();
            }
            var isSuccess = allwaysRunAction(resp);
            //手动应答的时候  需要加上如下代码
            if (isSuccess)
                listenChannel.BasicAck(deliveryTag, false);
            else
                listenChannel.BasicNack(deliveryTag, false, true);
        }

        private void TaskDo()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _maxRecord++;
                    if (_maxRecord % 10000 == 0)
                    {
                        Thread.Sleep(2000);
                        _maxRecord = 1;
                    }

                    if (Tasks.Count > _maxTaskCount)
                    {
                        Thread.Sleep(2000);
                    }
                    AddTaskChae();
                    var result = Tasks.TryDequeue(out Task item);
                    if (result)
                    {
                        try
                        {
                            item.Start();
                        }
                        catch
                        {
                            TasksCache.Enqueue(item);
                        }
                    }
                }
            });
        }

        private static void AddTaskChae()
        {
            var resutl = TasksCache.TryDequeue(out Task item);
            if (resutl)
            {
                Tasks.Enqueue(item);
            }
        }

        #endregion
    }
}
