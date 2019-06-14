using System;
using System.Collections.Generic;
using System.Text;

namespace ZENSURE.EHandWare.Models.Options
{
    /// <summary>
    /// RabbitMQ配置项
    /// </summary>
    public class RabbitMQOption
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否为AliwareMQ
        /// </summary>
        public bool IsAliMQ { get; set; }

        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool Durable { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 虚拟路由
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 最大任务数
        /// </summary>
        public int MaxTaskCount { get; set; }
    }
}
