using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Leopard.Saas
{
    public class TenantConnectionString : Entity
	{
		public virtual Guid TenantId { get; protected set; }

		public virtual string Name { get; protected set; }

		public virtual string Value { get; protected set; }

		public TenantConnectionString(Guid tenantId, string name, string value)
		{
			this.TenantId = tenantId;
			this.Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConnectionStringConsts.MaxNameLength, 0);
			this.SetValue(value);
		}

		public virtual void SetValue(string value)
		{
			this.Value = Check.NotNullOrWhiteSpace(value, nameof(value), TenantConnectionStringConsts.MaxValueLength, 0);
		}

		public override object[] GetKeys()
		{
			return new object[]
			{
				this.TenantId,
				this.Name
			};
		}
	}
}
