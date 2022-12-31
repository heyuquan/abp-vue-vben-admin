using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Leopard.Account.Public
{
	/// <summary>
	/// 发送密码重置码Dto
	/// </summary>
	public class SendPasswordResetCodeDto
	{
		/// <summary>
		/// 邮箱
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxEmailLength", null)]
		[EmailAddress]
		[Required]
		public string Email { get; set; }

		/// <summary>
		/// AppName
		/// </summary>
		[Required]
		public string AppName { get; set; }
	}
}
