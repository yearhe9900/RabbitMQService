using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using ZENSURE.EHandWare.ICommon.Dapper;
using ZENSURE.EHandWare.Models.Enums;
using ZENSURE.EHandWare.Models.Options;

namespace ZENSURE.EHandWare.Common.Dapper
{
    public class DapperClient : IDapperClient
    {
        private IDbConnection _dbConnection;
        private readonly string _connectStr;
        private readonly int _commonTimeout;
        private readonly DatabaseType _databaseType;

        public DapperClient(IOptions<DBContextOption> options)
        {
            var dbContext = options.Value;
            _connectStr = dbContext.ConnectionString;
            _databaseType = dbContext.DBType;
            _commonTimeout = dbContext.CommonTimeout;
        }

        private IDbConnection IDbConnection
        {
            get
            {
                switch (_databaseType)
                {
                    case DatabaseType.SqlServer:
                        _dbConnection = new SqlConnection(_connectStr);
                        break;
                    case DatabaseType.MySql:
                        _dbConnection = new MySqlConnection(_connectStr);
                        break;
                    case DatabaseType.Oracle:
                        _dbConnection = new OracleConnection(_connectStr);
                        break;
                    default:
                        _dbConnection = new SqlConnection(_connectStr);
                        break;
                }
                return _dbConnection;
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string sql, object param = null) where T : class
        {
            var list = new List<T>();
            using (var db = IDbConnection as DbConnection)
            {
                IEnumerable<T> it = db.Query<T>(sql, param, null, true, _commonTimeout, CommandType.Text);
                if (it != null)
                {
                    list = it.AsList();
                }
            }
            return list;
        }

        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int CreateOne(string sql, object param = null)
        {
            var result = 0;
            using (var db = IDbConnection as DbConnection)
            {
                result = db.Execute(sql, param, null, _commonTimeout, CommandType.Text);
            }
            return result;
        }

        /// <summary>
        /// 创建一个用户_异步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> CreateOneAsync(string sql, object param = null)
        {
            int result;
            using (var db = IDbConnection as DbConnection)
            {
                result = await db.ExecuteAsync(sql, param, null, _commonTimeout, CommandType.Text);
            }
            return result;
        }
    }
}
