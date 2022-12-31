using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Leopard.Identity
{
	public class IdentityRoleCreateOrUpdateDtoBase : ExtensibleObject
	{
		/// <summary>
		/// 角色名字
		/// </summary>
		[DynamicStringLength(typeof(IdentityRoleConsts), "MaxNameLength", null)]
		[Required]
		public string Name { get; set; }
		/// <summary>
		/// 是否默认
		/// </summary>
		public bool IsDefault { get; set; }
		/// <summary>
		/// 是否公开
		/// </summary>
		public bool IsPublic { get; set; }

		protected IdentityRoleCreateOrUpdateDtoBase()
			: base(false)
		{
		}
	}
}
