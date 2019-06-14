using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZENSURE.EHandWare.ICommon.Dapper;
using ZENSURE.EHandWare.IRepository;

namespace ZENSURE.EHandWare.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private IDapperClient _dapperClient;

        public BaseRepository(IDapperClient dapperClient)
        {
            _dapperClient = dapperClient;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStr"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string sqlStr, object param = null) where T : class
        {
            return _dapperClient.GetList<T>(sqlStr, param);
        }

        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int CreateOne(string sql, object param = null)
        {
            return _dapperClient.CreateOne(sql, param);
        }

        /// <summary>
        /// 创建一个用户_异步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> CreateOneAsync(string sql, object param = null)
        {
            return await _dapperClient.CreateOneAsync(sql, param);
        }
    }
}
