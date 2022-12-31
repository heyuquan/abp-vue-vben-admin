using System;
using System.ComponentModel.DataAnnotations;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识密码设置
	/// </summary>
	public class IdentityPasswordSettingsDto
	{
		/// <summary>
		/// 要求长度
		/// </summary>
		[Range(2, 128)]
		public int RequiredLength { get; set; }
		/// <summary>
		/// 要求唯一字符数量
		/// </summary>
		[Range(1, 128)]
		public int RequiredUniqueChars { get; set; }
		/// <summary>
		/// 要求非字母数字
		/// </summary>
		public bool RequireNonAlphanumeric { get; set; }
		/// <summary>
		/// 要求小写字母
		/// </summary>
		public bool RequireLowercase { get; set; }
		/// <summary>
		/// 要求大写字母
		/// </summary>
		public bool RequireUppercase { get; set; }
		/// <summary>
		/// 要求数字
		/// </summary>
		public bool RequireDigit { get; set; }
	}
}
