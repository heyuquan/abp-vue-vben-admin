using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Leopard.BackendAdmin.Dto.Output
{
    /// <summary>
    /// 适配vben菜单
    /// </summary>
    public class ApplicationMenuForVben
    {
        public ApplicationMenuForVben(string name, string displayName = null)
        {
            Name = Check.NotNullOrEmpty(name, nameof(name));
            DisplayName = displayName;
        }

        public string Name { get; }

        public string DisplayName { get; }

        public IEnumerable<RouteItemForVben> Items { get; set; }
    }
}
