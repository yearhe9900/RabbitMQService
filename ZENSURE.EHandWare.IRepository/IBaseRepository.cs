using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ZENSURE.EHandWare.IRepository
{
    public interface IBaseRepository
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStr"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        List<T> GetList<T>(string sqlStr, object param = null) where T : class;

        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int CreateOne(string sql, object param = null);

        /// <summary>
        /// 创建一个用户_异步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> CreateOneAsync(string sql, object param = null);
    }
}
