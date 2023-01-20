using Volo.Abp.ObjectExtending;

namespace Leopard.Saas.Dtos
{
	public abstract class EditionCreateOrUpdateDtoBase : ExtensibleObject
	{
		/// <summary>
		/// 显示名称
		/// </summary>
		public string DisplayName { get; set; }

		protected EditionCreateOrUpdateDtoBase()
			: base(false)
		{
		}
	}
}
