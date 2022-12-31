using Volo.Abp.Application.Dtos;

namespace Leopard.Saas.Dtos
{
	/// <summary>
	/// 获取租户信息Input
	/// </summary>
	public class GetTenantsInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }
		/// <summary>
		/// 是否获取版本名称
		/// </summary>
		public bool GetEditionNames { get; set; }

		public GetTenantsInput()
		{
			this.GetEditionNames = true;
		}
	}
}
