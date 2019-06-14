using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZENSURE.EHandWare.ICommon.RabbitMQ;
using ZENSURE.EHandWare.Models.Options;
using ZENSURE.EHandWare.Utils;

namespace ZENSURE.EHandWare.Common.RabbitMQ
{
    public class MQProducerClient : IMQProducerClient
    {
        private readonly IRabbitMQClient _client;
        private readonly IConnection _connection;
        private readonly bool _durable = false;

        public MQProducerClient(IRabbitMQClient client, IOptions<RabbitMQOption> options)
        {
            var rabbitMQ = options.Value;
            _durable = rabbitMQ.Durable;

            _client = client;
            _connection = _client.GetMQConnection();
        }

        private static readonly ConcurrentQueue<Task> TasksCache = new ConcurrentQueue<Task>(); //缓存队列

        #region 公有函数

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="qName"></param>
        /// <param name="msg"></param>
        /// <param name="isTask"></param>
        /// <param name="durable"></param>
        public void SendMessage<T>(string qName, T msg, bool isTask = true, bool? durable = null) where T : class
        {
            var message = SerializeObject(msg);
            PollyHelper.GetRetryTimesPolicy(2, ex =>
            {
                if (isTask)
                {
                    var task = new Task(() => { SendMsg(qName, message, durable); });
                    TasksCache.Enqueue(task);
                }
                else
                {
                    throw ex;
                }
            }).Execute(() => { SendMsg(qName, message, durable); });
        }

        /// <summary>
        /// 交换机发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchangeName"></param>
        /// <param name="msg"></param>
        /// <param name="routingKey"></param>
        /// <param name="exchangeType"></param>
        /// <param name="isTask"></param>
        /// <param name="durable"></param>
        public void SendMessageByExchange<T>(string exchangeName, T msg, string routingKey = "", string exchangeType = "direct", bool isTask = true, bool? durable = null) where T : class
        {
            var message = SerializeObject(msg);
            PollyHelper.GetRetryTimesPolicy(2, ex =>
            {
                if (isTask)
                {
                    var task = new Task(() => { SendMsgByExchange(exchangeName, message, exchangeType, routingKey, durable); });
                    TasksCache.Enqueue(task);
                }
                else
                {
                    throw ex;
                }
            }).Execute(() => { SendMsgByExchange(exchangeName, message, exchangeType, routingKey, durable); });
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 将msg序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        private byte[] SerializeObject<T>(T msg) where T : class
        {
            if (msg == null) return new byte[0];
            byte[] msgbyte = null;
            var msgstr = JsonConvert.SerializeObject(msg);
            msgbyte = Encoding.UTF8.GetBytes(msgstr);
            return msgbyte;
        }

        /// <summary>
        /// 管道发送消息
        /// </summary>
        /// <param name="qName"></param>
        /// <param name="msg"></param>
        /// <param name="durable"></param>
        private void SendMsg(string qName, byte[] msg, bool? durable = null)
        {
            if (string.IsNullOrEmpty(qName) || msg == null) return;

            if (_connection != null)
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(qName, durable == null ? _durable : durable.Value, false, false);
                    var basicProperties = new BasicProperties { Persistent = true };
                    channel.BasicPublish("", qName, basicProperties, msg);
                }
            }
        }

        /// <summary>
        /// 使用消息交换机绑定管道发送消息
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="msg"></param>
        /// <param name="exchangeType"></param>
        /// <param name="routingKey"></param>
        /// <param name="durable"></param>
        private void SendMsgByExchange(string exchangeName, byte[] msg, string exchangeType, string routingKey = "", bool? durable = null)
        {
            if (string.IsNullOrEmpty(exchangeName) || string.IsNullOrEmpty(exchangeType) || routingKey == null) return;

            if (_connection != null)
            {
                using (var channel = _connection.CreateModel())
                {
                    var thisDurable = durable == null ? _durable : durable.Value;
                    //声明交换机
                    channel.ExchangeDeclare(exchangeName, exchangeType, thisDurable);
                    channel.QueueDeclare(routingKey, thisDurable);
                    channel.QueueBind(routingKey, exchangeName, routingKey);
                    var basicProperties = new BasicProperties { Persistent = true };
                    channel.BasicPublish(exchangeName, routingKey, basicProperties, msg);
                }
            }
        }

        #endregion
    }
}
