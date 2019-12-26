using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using HZP.MQ.AdminManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQ.JOB.GRPC.GRPCManager;
using ZENSURE.EHandWare.Common.RabbitMQ;
using ZENSURE.EHandWare.ICommon.RabbitMQ;
using ZENSURE.EHandWare.ListenExcete;
using ZENSURE.EHandWare.Models.Input;
using ZENSURE.EHandWare.Models.Options;

namespace ZENSURE.EHandWare.TopshelfServices
{
    class Program
    {
        private static Server _server;

        static void Main(string[] args)
        {

            var bb = GRPCChannelHelper.SayHello(new HZP.MQ.AdminManager.TestRequest() { Name = "haha" });
            Console.WriteLine($"grpc:{bb.Message}");

            _server = new Server
            {
                Services = { HZP.MQ.ClientManager.gRPC.BindService(new GRPCImpl()) },
                Ports = { new ServerPort("localhost", 40002, ServerCredentials.Insecure) }
            };
            _server.Start();

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

            consumeClient.Listen<MessageData>("crm_hzp_test", ExpAccountMQJob.Excete);
        }
    }
    public class GRPCImpl : HZP.MQ.ClientManager.gRPC.gRPCBase
    {
        public override Task<HZP.MQ.ClientManager.TestReply> SayHello(HZP.MQ.ClientManager.TestRequest request, ServerCallContext callContext)
        {
            Console.WriteLine(request.Name);
            return Task.FromResult(new HZP.MQ.ClientManager.TestReply { Message = "Hello" + request.Name });
        }
    }
}
