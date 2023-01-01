using Volo.Abp.ObjectExtending;

namespace Leopard.Identity
{
	/// <summary>
	/// 个人信息
	/// </summary>
	public class ProfileDto : ExtensibleObject
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { get; set; }
		/// <summary>
		/// 邮箱
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// 名
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 姓
		/// </summary>
		public string Surname { get; set; }
		/// <summary>
		/// 手机号码
		/// </summary>
		public string PhoneNumber { get; set; }
		/// <summary>
		/// 手机号码是否已确认
		/// </summary>
		public bool PhoneNumberConfirmed { get; set; }
	}
}
