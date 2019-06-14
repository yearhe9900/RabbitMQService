using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZENSURE.EHandWare.Models.Enums
{
    /// <summary>
    /// 来源类型枚举
    /// </summary>
    public enum EnumSourceType
    {

        [Description("Web端")]
        Web = 1,

        [Description("App供货端")]
        AppAgent = 2,

        [Description("App采购端")]
        AppUser = 3,
    }
}
