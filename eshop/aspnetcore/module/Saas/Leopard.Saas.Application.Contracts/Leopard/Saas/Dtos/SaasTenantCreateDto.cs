using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Leopard.Saas.Dtos
{
	/// <summary>
	/// 创建租户信息
	/// </summary>
	public class SaasTenantCreateDto : SaasTenantCreateOrUpdateDtoBase
	{
		/// <summary>
		/// 租户管理员邮件地址 
		/// </summary>
		[StringLength(256)]
		[Required]
		[EmailAddress]
		public string AdminEmailAddress { get; set; }
		/// <summary>
		/// 租户管理员密码
		/// </summary>
		[StringLength(128)]
		[Required]
		[DisableAuditing]
		public string AdminPassword { get; set; }
	}
}
