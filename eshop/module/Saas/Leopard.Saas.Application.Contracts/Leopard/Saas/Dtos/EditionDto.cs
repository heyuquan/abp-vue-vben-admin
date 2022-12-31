using System;
using Volo.Abp.Application.Dtos;

namespace Leopard.Saas.Dtos
{
	/// <summary>
	/// 版本Dto
	/// </summary>
	public class EditionDto : ExtensibleEntityDto<Guid>
	{
		/// <summary>
		/// 显示名称
		/// </summary>
		public string DisplayName { get; set; }
	}
}
