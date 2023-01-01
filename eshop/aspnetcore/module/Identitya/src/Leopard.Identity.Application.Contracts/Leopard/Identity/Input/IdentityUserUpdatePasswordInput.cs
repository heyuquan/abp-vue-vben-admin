namespace Leopard.Identity
{
	public class IdentityUserUpdatePasswordInput
	{
		/// <summary>
		/// 新密码
		/// </summary>
		public string NewPassword { get; set; }

		public IdentityUserUpdatePasswordInput(string newPassword)
		{
			NewPassword = newPassword;
		}
	}
}
