using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Leopard.Account
{
	/// <summary>
	/// 注册账号Dto
	/// </summary>
	public class RegisterDto : ExtensibleObject
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxUserNameLength", null)]
		[Required]
		public string UserName { get; set; }

		/// <summary>
		/// 邮箱地址
		/// </summary>
		[Required]
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxEmailLength", null)]
		[EmailAddress]
		public string EmailAddress { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[DisableAuditing]
		[Required]
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxPasswordLength", null)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		/// <summary>
		/// AppName
		/// </summary>
		[Required]
		public string AppName { get; set; }
	}
}
