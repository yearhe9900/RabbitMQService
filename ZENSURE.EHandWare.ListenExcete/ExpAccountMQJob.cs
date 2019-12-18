using System;
using ZENSURE.EHandWare.Models.Input;

namespace ZENSURE.EHandWare.ListenExcete
{
    public class ExpAccountMQJob
    {

        public static bool Excete(MessageData msg)
        {
            Console.WriteLine($"获取到消息{msg.Id}");
            return true;
        }
    }
}
