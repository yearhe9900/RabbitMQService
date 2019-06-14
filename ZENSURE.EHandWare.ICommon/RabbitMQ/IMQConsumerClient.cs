using System;
using System.Collections.Generic;
using System.Text;

namespace ZENSURE.EHandWare.ICommon.RabbitMQ
{
    public interface IMQConsumerClient
    {
        /// <summary>
        /// 接受通道队列消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="qName"></param>
        /// <param name="allwaysRunAction">执行动作</param>
        /// <param name="isTask"></param>
        void Listen<T>(string qName, Func<T, bool> allwaysRunAction, bool isTask = false) where T : class;

        /// <summary>
        /// 接受交换机绑定通道消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchangeName"></param>
        /// <param name="allwaysRunAction"></param>
        /// <param name="routingKey"></param>
        /// <param name="exchangeType"></param>
        /// <param name="isTask"></param>
        void ListenByExchange<T>(string exchangeName, Func<T, bool> allwaysRunAction, string routingKey = "", string exchangeType = "direct", bool isTask = false) where T : class;
    }
}
