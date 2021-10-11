using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Leopard.Saas
{
	/// <summary>
	/// 版本
	/// </summary>
    public class Edition : FullAuditedAggregateRoot<Guid>
	{
		public virtual string DisplayName { get; protected set; }

		public Edition(Guid id, string displayName)
		{
			this.Id = id;
			this.SetDisplayName(displayName);
		}

		public virtual void SetDisplayName(string displayName)
		{
			this.DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), EditionConsts.MaxDisplayNameLength, 0);
		}
	}
}
