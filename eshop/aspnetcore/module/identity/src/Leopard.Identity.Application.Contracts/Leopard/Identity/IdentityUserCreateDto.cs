using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识用户创建
	/// </summary>
	public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
	{
		[DisableAuditing]
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxPasswordLength", null)]
		[Required]
		public string Password { get; set; }
	}
}
