using Volo.Abp.Application.Dtos;

namespace Leopard.Saas.Dtos
{
	/// <summary>
	/// 获取版本信息Input
	/// </summary>
	public class GetEditionsInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }
	}
}
