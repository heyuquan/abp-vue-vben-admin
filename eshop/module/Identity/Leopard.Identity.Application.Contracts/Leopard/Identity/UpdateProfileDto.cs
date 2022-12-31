using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Leopard.Identity
{
	/// <summary>
	/// 更新个人信息Dto
	/// </summary>
	public class UpdateProfileDto : ExtensibleObject
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[Required]
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxUserNameLength", null)]
		public string UserName { get; set; }
		/// <summary>
		/// 邮箱
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxEmailLength", null)]
		public string Email { get; set; }
		/// <summary>
		/// 名
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxNameLength", null)]
		public string Name { get; set; }
		/// <summary>
		/// 姓
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxSurnameLength", null)]
		public string Surname { get; set; }
		/// <summary>
		/// 手机号码
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxPhoneNumberLength", null)]
		public string PhoneNumber { get; set; }
	}
}
