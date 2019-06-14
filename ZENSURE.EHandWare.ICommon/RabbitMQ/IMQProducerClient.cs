using System;

namespace ZENSURE.EHandWare.ICommon.RabbitMQ
{
    public interface IMQProducerClient
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="qName"></param>
        /// <param name="msg"></param>
        /// <param name="isTask"></param>
        /// <param name="durable"></param>
        void SendMessage<T>(string qName, T msg, bool isTask = true, bool? durable = null) where T : class;

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
        void SendMessageByExchange<T>(string exchangeName, T msg, string routingKey = "", string exchangeType = "direct", bool isTask = true, bool? durable = null) where T : class;
    }
}
