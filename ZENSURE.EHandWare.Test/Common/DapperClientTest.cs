using System;
using System.Threading.Tasks;
using Xunit;
using ZENSURE.EHandWare.Doomain;
using ZENSURE.EHandWare.ICommon.Dapper;
using ZENSURE.EHandWare.Models.Enums;
using ZENSURE.LogSystem.Test;

namespace ZENSURE.EHandWare.Test.Common
{
    [Collection("ServiceTest")]
    public class DapperClientTest
    {
        private IDapperClient _dapperClient;

        public DapperClientTest(UnitTestyApplication application)
        {
            _dapperClient = application.GetService<IDapperClient>();
        }

        /// <summary>
        /// 获取列表测试_不带参数
        /// </summary>
        [Fact]
        public void GET_LIST_WITHOUT_PARMS_TEST()
        {
            string selectSql = @"SELECT * FROM dbo.T_DemoUsers";
            var list = _dapperClient.GetList<DemoUsersModel>(selectSql);
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 获取列表测试_不带参数
        /// </summary>
        [Fact]
        public void GET_LIST_WITHOUT_PARMS_MANY_TIMES_TEST()
        {
            for (int i = 0; i < 10; i++)
            {
                string selectSql = @"SELECT * FROM dbo.T_DemoUsers";
                var list = _dapperClient.GetList<DemoUsersModel>(selectSql);
                Assert.True(list.Count > 0);
            }
        }

        /// <summary>
        /// 获取列表测试_带参数
        /// </summary>
        [Fact]
        public void GET_LIST_WITH_PARMS_TEST()
        {
            string selectSql = @"SELECT * FROM dbo.T_DemoUsers where ID = @ID";
            var list = _dapperClient.GetList<DemoUsersModel>(selectSql, new { ID = "7512E204-BB35-4FF3-AA36-18C2052219FC" });
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 获取列表测试_带参数
        /// </summary>
        [Fact]
        public void GET_LIST_WITH_PARMS_MANY_TIMES_TEST()
        {
            for (int i = 0; i < 10; i++)
            {
                string selectSql = @"SELECT * FROM dbo.T_DemoUsers where ID = @ID";
                var list = _dapperClient.GetList<DemoUsersModel>(selectSql, new { ID = "7512E204-BB35-4FF3-AA36-18C2052219FC" });
                Assert.True(list.Count > 0);
            }
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        [Fact]
        public void CREATE_ONE_DATE_TEST()
        {
            string insertSql = @"insert into dbo.T_DemoUsers values (@ID,@UserName,@Mobile,@TryDate,@Status,@Leader,@Creator,@Enable,@CDT,@Modifier,@UDT,@SourceType)";
            DemoUsersModel model = new DemoUsersModel
            {
                ID = Guid.NewGuid(),
                UserName = "13170013000",
                Mobile = "13170013000",
                TryDate = DateTime.Now,
                Status = ExperienceAccountViewEnum.WaitingContact,
                SourceType = EnumSourceType.Web
            };
            var result = _dapperClient.CreateOne(insertSql, model);
            Assert.True(result == 1);
        }

        /// <summary>
        /// 添加一条数据_异步
        /// </summary>
        [Fact]
        public void CREATE_ONE_DATE_ASYNC_TEST()
        {
            string insertSql = @"insert into dbo.T_DemoUsers values (@ID,@UserName,@Mobile,@TryDate,@Status,@Leader,@Creator,@Enable,@CDT,@Modifier,@UDT,@SourceType)";
            DemoUsersModel model = new DemoUsersModel
            {
                ID = Guid.NewGuid(),
                UserName = "13170013000",
                Mobile = "13170013000",
                TryDate = DateTime.Now,
                Status = ExperienceAccountViewEnum.WaitingContact,
                SourceType = EnumSourceType.Web
            };
            var result = _dapperClient.CreateOneAsync(insertSql, model).GetAwaiter().GetResult();
            Assert.True(result == 1);
        }
    }
}
