using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZENSURE.EHandWare.ICommon.RabbitMQ
{
    public interface IRabbitMQClient
    {
        /// <summary>
        /// 获取链接
        /// </summary>
        /// <returns></returns>
        IConnection GetMQConnection();
    }
}
