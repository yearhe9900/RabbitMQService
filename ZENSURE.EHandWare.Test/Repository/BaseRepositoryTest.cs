using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ZENSURE.EHandWare.Doomain;
using ZENSURE.EHandWare.IRepository;
using ZENSURE.EHandWare.TestModel;
using ZENSURE.LogSystem.Test;

namespace ZENSURE.EHandWare.Test.Repository
{
    [Collection("ServiceTest")]
    public class BaseRepositoryTest
    {
        private IBaseRepository _baseRepository;

        public BaseRepositoryTest(UnitTestyApplication application)
        {
            _baseRepository = application.GetService<IBaseRepository>();
        }

        /// <summary>
        /// 获取列表测试_不带参数
        /// </summary>
        [Fact]
        public void GET_LIST_WITHOUT_PARMS_TEST()
        {
            string selectSql = @"SELECT * FROM dbo.T_DemoUsers";
            var list = _baseRepository.GetList<DemoUsersModel>(selectSql);
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
                var list = _baseRepository.GetList<DemoUsersModel>(selectSql);
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
            var list = _baseRepository.GetList<DemoUsersModel>(selectSql, new { ID = "7512E204-BB35-4FF3-AA36-18C2052219FC" });
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
                var list = _baseRepository.GetList<DemoUsersModel>(selectSql, new { ID = "7512E204-BB35-4FF3-AA36-18C2052219FC" });
                Assert.True(list.Count > 0);
            }
        }
    }
}
