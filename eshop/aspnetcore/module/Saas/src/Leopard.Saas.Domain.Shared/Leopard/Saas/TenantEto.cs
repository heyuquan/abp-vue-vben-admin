using System;

namespace Leopard.Saas
{
    [Serializable]
	public class TenantEto
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public Guid? EditionId { get; set; }
	}
}
