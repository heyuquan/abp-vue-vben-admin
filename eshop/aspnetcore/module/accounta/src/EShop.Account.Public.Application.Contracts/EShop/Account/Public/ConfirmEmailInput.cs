using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Account.Public
{
	/// <summary>
	/// 确认邮箱Input
	/// </summary>
    public class ConfirmEmailInput
	{
		[Required]
		public Guid UserId { get; set; }

		[Required]
		public string Token { get; set; }
	}
}
