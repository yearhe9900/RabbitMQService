using System;
using System.Collections.Generic;
using System.Text;
using ZENSURE.EHandWare.Models.Enums;

namespace ZENSURE.EHandWare.Models.Options
{
    /// <summary>
    /// 数据库配置项
    /// </summary>
    public class DBContextOption
    {
        /// <summary>
        /// 数据库连接字段
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 超时设置
        /// </summary>
        public int CommonTimeout { get; set; }

        /// <summary>
        /// 数据库连接方式
        /// </summary>
        public DatabaseType DBType { get; set; }
    }
}
