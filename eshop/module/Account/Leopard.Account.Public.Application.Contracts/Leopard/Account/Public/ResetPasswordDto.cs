using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Leopard.Account.Public
{
	/// <summary>
	/// 密码重置Dto
	/// </summary>
	public class ResetPasswordDto
	{
		public Guid UserId { get; set; }

		[Required]
		public string ResetToken { get; set; }

		/// <summary>
		/// 新密码
		/// </summary>
		[DisableAuditing]
		[Required]
		public string Password { get; set; }
	}
}
