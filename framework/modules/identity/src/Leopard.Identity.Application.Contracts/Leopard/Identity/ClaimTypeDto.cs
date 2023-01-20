using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Leopard.Identity
{
	/// <summary>
	/// 声明类型
	/// </summary>
	public class ClaimTypeDto : ExtensibleEntityDto<Guid>
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
		/// 是否静态
		/// </summary>
		public bool IsStatic { get; set; }
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
		/// <summary>
		/// ValueType字段的字符串表示
		/// </summary>
		public string ValueTypeAsString { get; set; }
	}
}
