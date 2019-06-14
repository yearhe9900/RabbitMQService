using Polly;
using System;

namespace ZENSURE.EHandWare.Utils
{
    public class PollyHelper
    {
        /// <summary>
        /// 重试次数策略
        /// </summary>
        /// <param name="time"></param>
        /// <param name="exAction">错误处理</param>
        /// <returns></returns>
        public static Policy GetRetryTimesPolicy(int time, Action<Exception> exAction = null)
        {
            if (time <= 0) return default(Policy);
            Policy policy = null;
            if (exAction == null)
            {
                policy = Policy
                    .Handle<Exception>()
                    .Retry(time);
            }
            else
            {
                policy = Policy
                    .Handle<Exception>()
                    .Retry(time, (ex, count) =>
                    {
                        exAction(ex);
                    });
            }
            return policy;
        }

        /// <summary>
        /// 超时策略
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static Policy GetTimeOutPolicy(int milliseconds)
        {
            if (milliseconds <= 0) return default(Policy);
            var policy = Policy
                .Timeout(TimeSpan.FromMilliseconds(milliseconds));
            return policy;
        }

        /// <summary>
        /// 回退策略
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Policy GetFallBackPolicy(Action method)
        {
            if (method == null) return default(Policy);
            var policy = Policy
                .Handle<Exception>()
                .Fallback(method);
            return policy;
        }
    }
}
