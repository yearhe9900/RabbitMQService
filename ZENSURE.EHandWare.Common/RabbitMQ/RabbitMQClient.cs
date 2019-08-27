using RabbitMQ.Client;
using System.Collections.Generic;
using ZENSURE.EHandWare.ICommon.RabbitMQ;
using ZENSURE.EHandWare.Models.Options;
using Microsoft.Extensions.Options;

namespace ZENSURE.EHandWare.Common.RabbitMQ
{
    public class RabbitMQClient : IRabbitMQClient
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;

        public RabbitMQClient(IOptions<RabbitMQOption> options)
        {
            var rabbitMQ = options.Value;

            //创建连接工厂
            _connectionFactory = new ConnectionFactory()
            {
                HostName = rabbitMQ.HostName,
                Port = rabbitMQ.Port,
                UserName = rabbitMQ.UserName,
                Password = rabbitMQ.Password,
                TopologyRecoveryEnabled = true,
                AutomaticRecoveryEnabled = true //自动重连
            };

            _connectionFactory.VirtualHost = rabbitMQ.VirtualHost;

            //如果为AliwareMQ，启动ali配置
            //if (rabbitMQ.IsAliMQ)
            //{
            //    _connectionFactory.AuthMechanisms = new List<AuthMechanismFactory>() { new AliyunMechanismFactory() };
            //}

            //创建链接
            _connection = _connectionFactory.CreateConnection();
        }

        /// <summary>
        /// 获取链接
        /// </summary>
        /// <returns></returns>
        public IConnection GetMQConnection()
        {
            return _connection;
        }
    }
}