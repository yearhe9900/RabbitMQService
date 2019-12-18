using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZENSURE.EHandWare.Common.RabbitMQ;
using ZENSURE.EHandWare.ICommon.RabbitMQ;
using ZENSURE.EHandWare.ListenExcete;
using ZENSURE.EHandWare.Models.Input;
using ZENSURE.EHandWare.Models.Options;

namespace ZENSURE.EHandWare.TopshelfServices
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection() //将配置文件的数据加载到内存中
                .SetBasePath(Directory.GetCurrentDirectory()) //指定配置文件所在的目录
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) //指定加载的配置文件
                .Build(); //编译成对象 

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .Configure<RabbitMQOption>(c => config.Bind("RabbitMQOption", c)) //注入配置数据
                .AddSingleton<IRabbitMQClient, RabbitMQClient>()
                .AddSingleton<IMQProducerClient, MQProducerClient>()
                .AddSingleton<IMQConsumerClient, MQConsumerClient>()
                .BuildServiceProvider();

            var consumeClient = serviceProvider.GetService<IMQConsumerClient>();

            consumeClient.Listen<MessageData>("CrmTestQueue", ExpAccountMQJob.Excete);
        }
    }
}
