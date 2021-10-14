using System.ComponentModel.DataAnnotations;

namespace Leopard.Account.Public
{
	/// <summary>
	/// 确认电话号码Input
	/// </summary>
	public class ConfirmPhoneNumberInput
	{
		[Required]
		public string Token { get; set; }
	}
}
