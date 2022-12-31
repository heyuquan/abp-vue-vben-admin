using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Leopard.Identity
{
	/// <summary>
	/// 身份标识用户信息
	/// </summary>
	public class IdentityUserDto : ExtensibleEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp
	{
		/// <summary>
		/// 租户Id
		/// </summary>
		public Guid? TenantId { get; set; }
		/// <summary>
		/// 用户名称
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
		/// 是否确认邮箱
		/// </summary>
		public bool EmailConfirmed { get; set; }
		/// <summary>
		/// 手机号码
		/// </summary>
		public string PhoneNumber { get; set; }
		/// <summary>
		/// 是否确认手机号码
		/// </summary>
		public bool PhoneNumberConfirmed { get; set; }
		/// <summary>
		/// 是否启用双因素验证
		/// </summary>
		public bool TwoFactorEnabled { get; set; }
		/// <summary>
		/// 启用登录尝试失败后锁定账户
		/// </summary>
		public bool LockoutEnabled { get; set; }
		/// <summary>
		/// 是否已被锁定
		/// </summary>
		public bool IsLockedOut { get; set; }

		public string ConcurrencyStamp { get; set; }
	}
}
