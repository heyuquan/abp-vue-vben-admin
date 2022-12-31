using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Leopard.Saas
{
    public class Tenant : FullAuditedAggregateRoot<Guid>
	{
		public virtual string Name { get; protected set; }

		public virtual Guid? EditionId { get; set; }

		public virtual List<TenantConnectionString> ConnectionStrings { get; protected set; }

		protected internal Tenant(Guid id, string name, Guid? editionId = null)
		{
			this.Id = id;
			this.SetName(name);
			this.EditionId = editionId;
			this.ConnectionStrings = new List<TenantConnectionString>();
		}

		public virtual string FindDefaultConnectionString()
		{
			return this.FindConnectionString("Default");
		}

		public virtual string FindConnectionString(string name)
		{
			TenantConnectionString tenantConnectionString = this.ConnectionStrings.FirstOrDefault(x => x.Name == name);
			if (tenantConnectionString == null)
			{
				return null;
			}
			return tenantConnectionString.Value;
		}

		public virtual void SetDefaultConnectionString(string connectionString)
		{
			this.SetConnectionString("Default", connectionString);
		}

		public virtual void SetConnectionString(string name, string connectionString)
		{
			TenantConnectionString tenantConnectionString = this.ConnectionStrings.FirstOrDefault(x => x.Name == name);
			if (tenantConnectionString != null)
			{
				tenantConnectionString.SetValue(connectionString);
				return;
			}
			this.ConnectionStrings.Add(new TenantConnectionString(this.Id, name, connectionString));
		}

		public virtual void RemoveDefaultConnectionString()
		{
			this.RemoveConnectionString("Default");
		}

		public virtual void RemoveConnectionString(string name)
		{
			TenantConnectionString tenantConnectionString = this.ConnectionStrings.FirstOrDefault(x => x.Name == name);
			if (tenantConnectionString != null)
			{
				this.ConnectionStrings.Remove(tenantConnectionString);
			}
		}

		protected internal virtual void SetName(string name)
		{
			this.Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConsts.MaxNameLength, 0);
		}
	}
}
