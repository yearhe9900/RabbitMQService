using System;
using Xunit;
using Xunit.Abstractions;
using ZENSURE.EHandWare.ICommon.RabbitMQ;
using ZENSURE.EHandWare.TestModel;
using ZENSURE.LogSystem.Test;

namespace ZENSURE.EHandWare.Test.Common
{
    [Collection("ServiceTest")]
    public class RabbitMQTest
    {
        readonly IMQProducerClient _producerClient;
        readonly IMQConsumerClient _consumerClient;
        private static ITestOutputHelper _output;

        public RabbitMQTest(UnitTestyApplication application, ITestOutputHelper tempOutput)
        {
            _producerClient = application.GetService<IMQProducerClient>();
            _consumerClient = application.GetService<IMQConsumerClient>();
            _output = tempOutput;
        }

        /// <summary>
        /// 发送和接受消息测试
        /// </summary>
        [Fact]
        public void SEND_TEST()
        {
            for (int i = 0; i < 10; i++)
            {
                var data = new MessageData2
                {
                    ID = i,
                    MemberID = "xxxxxxxxx1",
                    CardNo = "xxxxxxx",
                    Act = "T",
                    LogCreateTime = DateTime.Now
                };
                _producerClient.SendMessage("MessageMQ", data);
            }
        }

        /// <summary>
        /// 发送和接受消息测试
        /// </summary>
        [Fact]
        public void SEND_AND_LISTEN_TEST()
        {
            for (int i = 0; i < 10; i++)
            {
                var data = new MessageData
                {
                    Id = i,
                    Title = $"测试标题{i.ToString()}",
                    Context = $"测试内容{i.ToString()}",
                    IsHtml = false,
                    Status = 1,
                    CreateMan = "admin",
                    CreateDateTime = DateTime.Now
                };
                _producerClient.SendMessage("MessageMQ", data);
            }

            _consumerClient.Listen("MessageMQ", ((MessageData msg) =>
            {
                _output.WriteLine($"获取到消息{msg.Id}");
                return true;
            }), isTask: false);
        }

        /// <summary>
        /// 接受消息测试
        /// </summary>
        [Fact]
        public void LISTEN_TEST()
        {
            _consumerClient.Listen("MessageMQ", ((MessageData msg) =>
            {
                _output.WriteLine($"获取到消息{msg.Id}");
                return true;
            }), isTask: false);
        }

        /// <summary>
        /// 发送和接受消息测试
        /// </summary>
        [Fact]
        public void SEND_AND_LISTEN_EXCHANGE_TEST()
        {
            for (int i = 0; i < 10; i++)
            {
                var data = new MessageData
                {
                    Id = i,
                    Title = $"测试标题{i.ToString()}",
                    Context = $"测试内容{i.ToString()}",
                    IsHtml = false,
                    Status = 1,
                    CreateMan = "admin",
                    CreateDateTime = DateTime.Now
                };
                _producerClient.SendMessageByExchange("Order", data, $"12345MQ{i}", isTask: true);
            }

            for (int i = 0; i < 10; i++)
            {
                _consumerClient.ListenByExchange<MessageData>("Order", ((MessageData msg) =>
                {
                    _output.WriteLine($"获取到消息{msg.Id}");
                    return true;
                }), $"12345MQ{i}", isTask: false);
            }
        }
    }
}
