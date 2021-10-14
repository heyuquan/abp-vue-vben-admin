using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Leopard.Identity
{
	/// <summary>
	/// 更新声明类型
	/// </summary>
	public class UpdateClaimTypeDto : ExtensibleObject
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 是否必要
		/// </summary>
		public bool Required { get; set; }
		/// <summary>
		/// 正则表达式
		/// </summary>
		public string Regex { get; set; }
		/// <summary>
		/// 正则描述
		/// </summary>
		public string RegexDescription { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// 值类型
		/// </summary>
		public IdentityClaimValueType ValueType { get; set; }

		public UpdateClaimTypeDto()
			: base(false)
		{
		}
	}
}
