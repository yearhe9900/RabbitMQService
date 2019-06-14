using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using ZENSURE.EHandWare.Common.Dapper;
using ZENSURE.EHandWare.Common.RabbitMQ;
using ZENSURE.EHandWare.ICommon.Dapper;
using ZENSURE.EHandWare.ICommon.RabbitMQ;
using ZENSURE.EHandWare.IRepository;
using ZENSURE.EHandWare.Models.Options;
using ZENSURE.EHandWare.Repository;

namespace ZENSURE.LogSystem.Test
{
    public class UnitTestyApplication
    {
        public UnitTestyApplication()
        {
            Init();
        }

        public IServiceProvider ServiceProvider { get; private set; }

        public void Init()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            services.Configure<RabbitMQOption>(c => config.Bind("RabbitMQOption", c));
            services.Configure<DBContextOption>(c => config.Bind("DBContextOption", c));

            services.AddSingleton<IDapperClient, DapperClient>();
            services.AddSingleton<IMQConsumerClient, MQConsumerClient>();
            services.AddSingleton<IMQProducerClient, MQProducerClient>();
            services.AddSingleton<IRabbitMQClient, RabbitMQClient>();
            services.AddSingleton<IDapperClient, DapperClient>();

            services.AddSingleton<IBaseRepository, BaseRepository>();

            ServiceProvider = services.BuildServiceProvider();
        }
        public T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }
    }

    // Use shared context to maintain database fixture
    // see https://xunit.github.io/docs/shared-context.html#collection-fixture
    [CollectionDefinition("ServiceTest")]
    public class ApplicationCollection : ICollectionFixture<UnitTestyApplication>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
