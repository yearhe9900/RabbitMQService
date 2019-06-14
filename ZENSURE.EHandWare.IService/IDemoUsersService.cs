using System;
using ZENSURE.EHandWare.Doomain;

namespace ZENSURE.EHandWare.IService
{
    public interface IDemoUsersService
    {
        /// <summary>
        /// 添加一条体验用户账户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddDemoUser(DemoUsersModel model);
    }
}
