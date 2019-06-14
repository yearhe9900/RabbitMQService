using System;
using System.Collections.Generic;
using System.Text;
using ZENSURE.EHandWare.Models.Enums;

namespace ZENSURE.EHandWare.Doomain
{
    public class DemoUsersModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public System.Guid ID { get; set; }

        /// <summary>
    	/// 账号
    	/// </summary>
    	public string UserName { get; set; }

        /// <summary>
    	/// 手机号码
    	/// </summary>
    	public string Mobile { get; set; }

        /// <summary>
    	/// 试用时间
    	/// </summary>
    	public DateTime? TryDate { get; set; }

        /// <summary>
    	/// 状态:0(待接触)，1（无意向）2（待深挖）3（已合作）
    	/// </summary>
    	public ExperienceAccountViewEnum Status { get; set; }

        /// <summary>
    	/// 负责人
    	/// </summary>
    	public string Leader { get; set; }

        /// <summary>
    	/// 创建人
    	/// </summary>
    	public string Creator { get; set; }

        /// <summary>
        /// 系统创建时间
        /// </summary>
        public DateTime? CDT { get; set; } = DateTime.Now;

        /// <summary>
        /// 启用状态： 0禁用 ，1启用（默认启用），2删除
        /// </summary>
        public TableEnable Enable { get; set; } = TableEnable.Enable;

        /// <summary>
    	/// 系统修改者
    	/// </summary>
    	public string Modifier { get; set; }

        /// <summary>
    	/// 系统修改时间
    	/// </summary>
    	public DateTime? UDT { get; set; }

        /// <summary>
        /// 来源类型：1（PC）2（App供货端）3（App采购端）
        /// </summary>
        public EnumSourceType SourceType { get; set; }
    }
}
