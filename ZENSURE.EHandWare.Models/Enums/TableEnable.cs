using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZENSURE.EHandWare.Models.Enums
{
    /// <summary>
    /// 数据状态
    /// </summary>
    public enum TableEnable
    {
        [Description("禁用")]
        Disable = 0,

        [Description("启用")]
        Enable = 1,

        [Description("删除")]
        Delete = 2
    }
}
