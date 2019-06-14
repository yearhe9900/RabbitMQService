using System;
using System.Collections.Generic;
using ZENSURE.EHandWare.Doomain;
using ZENSURE.EHandWare.IRepository;
using ZENSURE.EHandWare.IService;

namespace ZENSURE.EHandWare.Service
{
    public class DemoUsersService : IDemoUsersService
    {
        private readonly IBaseRepository _baseRepository;

        public DemoUsersService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        /// <summary>
        /// 添加一条体验用户账户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddDemoUser(DemoUsersModel model)
        {
            string insertSql = @"insert into dbo.T_DemoUsers values (@ID,@UserName,@Mobile,@TryDate,@Status,@Leader,@Creator,@Enable,@CDT,@Modifier,@UDT,@SourceType)";
            return _baseRepository.CreateOne(insertSql, model);
        }
    }
}
