using System.Collections.Generic;

namespace Leopard.Saas
{
	/// <summary>
	/// 获取版本使用统计计数结果
	/// </summary>
    public class GetEditionUsageStatisticsResult
	{
		public Dictionary<string, int> Data { get; set; }
	}
}
