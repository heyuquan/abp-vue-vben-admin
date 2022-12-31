using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Administration.Dto.Output
{
    /// <summary>
    /// 适配 vben 的菜单模型
    /// </summary>
    public class RouteItemForVben
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 显示页面路径 
        /// 父级为排版eg：Layout
        /// 子级为路径eg：/@/views/saas/tenant/index.vue
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 菜单标识名（英文），全局唯一
        /// </summary>
        public string Name { get; set; }

        public RouteItemMetaForVben Meta { get; set; }
        ///// <summary>
        ///// 如果只有一级菜单，那么需要配置Redirect
        ///// eg：Path=/about   Redirect=/about/index
        ///// </summary>
        public string Redirect { get; set; }
        /// <summary>
        /// 子路由
        /// </summary>
        public IEnumerable<RouteItemForVben> Children { get; set; }
    }

    public class RouteItemMetaForVben
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public string Icon { get; set; }

        public string CssClass { get; set; }
        ///// <summary>
        ///// 是否无效
        ///// </summary>
        //public bool IsDisabled { get; set; }
        // 隐藏所有子菜单
        public bool HideChildrenInMenu { get; set; }
        /// <summary>
        /// 当前路由不再菜单显示
        /// </summary>
        public bool HideMenu { get; set; }
        /// <summary>
        /// 当前路由不再标签页显示
        /// </summary>
        public bool HideTab { get; set; }
    }
}
