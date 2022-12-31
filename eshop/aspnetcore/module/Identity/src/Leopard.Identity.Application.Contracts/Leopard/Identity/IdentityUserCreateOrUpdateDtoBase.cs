using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Leopard.Identity
{
	public abstract class IdentityUserCreateOrUpdateDtoBase : ExtensibleObject
	{
		/// <summary>
		/// 用户名称
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxUserNameLength", null)]
		[Required]
		public string UserName { get; set; }
		/// <summary>
		/// 名
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxNameLength", null)]
		public string Name { get; set; }
		/// <summary>
		/// 姓
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxSurnameLength", null)]
		public string Surname { get; set; }
		/// <summary>
		/// 邮箱
		/// </summary>
		[EmailAddress]
		[Required]
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxEmailLength", null)]
		public string Email { get; set; }
		/// <summary>
		/// 手机号码
		/// </summary>
		[DynamicStringLength(typeof(IdentityUserConsts), "MaxPhoneNumberLength", null)]
		public string PhoneNumber { get; set; }
		/// <summary>
		/// 启用双因素验证
		/// </summary>
		public bool TwoFactorEnabled { get; set; }
		/// <summary>
		/// 启用登录尝试失败后锁定账户
		/// </summary>
		public bool LockoutEnabled { get; set; }
		/// <summary>
		/// 角色名
		/// </summary>
		public string[] RoleNames { get; set; }
		/// <summary>
		/// 组织Id集合
		/// </summary>
		public Guid[] OrganizationUnitIds { get; set; }

		protected IdentityUserCreateOrUpdateDtoBase()
			: base(false)
		{
		}
	}
}
