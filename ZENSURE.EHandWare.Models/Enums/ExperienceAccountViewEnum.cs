using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZENSURE.EHandWare.Models.Enums
{
    /// <summary>
    /// 试用账号枚举
    /// </summary>
    public enum ExperienceAccountViewEnum
    {
        /// <summary>
        /// 待接触
        /// </summary>
        [Description("待接触")]
        WaitingContact = 0,

        /// <summary>
        /// 无意向
        /// </summary>
        [Description("无意向")]
        NoIntention = 1,

        /// <summary>
        /// 待深挖
        /// </summary>
        [Description("待深挖")]
        WaitingDig = 2,

        /// <summary>
        /// 已合作
        /// </summary>
        [Description("已合作")]
        Cooperated = 3
    }
}
