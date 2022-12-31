using JetBrains.Annotations;
using EShop.Administration.Dto.Output;
using Leopard.UI.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.UI.Navigation;

namespace EShop.Administration.Controllers
{
    [Route("api/backend-admin/menus")]
    [Authorize]
    public class MenusController : BackendAdminController
    {
        private readonly IMenuManager _menuManager;

        public MenusController(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        /// <summary>
        /// 获取后台管理主菜单列表（当前用户有权限的）
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        [Route("user-main")]
        public async Task<ApplicationMenu> GetUserMainAsync()
        {
            var menus = await this.GetUserMenuAsync(LeopardStandardMenus.Main);

            return menus;
        }

        /// <summary>
        /// （适配vben）获取后台管理主菜单列表（当前用户有权限的）
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        [Route("vben/user-main")]
        public async Task<List<RouteItemForVben>> GetUserMainForVbenAsync()
        {
            var applicationMenu = await this.GetUserMenuAsync(LeopardStandardMenus.Main);
            // 适配vben
            if (applicationMenu != null)
            {
                return ConvertToRouteItemForVben(applicationMenu.Items[0].Items);
            }
            return null;
        }

        private List<RouteItemForVben> ConvertToRouteItemForVben(ApplicationMenuItemList menuItems)
        {
            if (menuItems == null || !menuItems.Any())
                return null;

            List<RouteItemForVben> result = new List<RouteItemForVben>();
            foreach (var menuItem in menuItems)
            {

                PropertyInfo[] props = menuItem.CustomData.GetType()
                                                    .GetProperties(BindingFlags.GetField |
                                                                    BindingFlags.Public |
                                                                    BindingFlags.Instance);

                bool.TryParse(props.FirstOrDefault(x => x.Name == "IsGroup")?.GetValue(menuItem.CustomData).ToString(), out bool IsGroup);

                if (!IsGroup || (IsGroup && menuItem.Items.Count > 0))   // 没有子菜单的一级IsGroup菜单，就不需要显示了
                {
                    RouteItemForVben routeItem = new RouteItemForVben
                    {
                        Name = menuItem.Name,
                        Path = menuItem.Url,
                        Component = props.FirstOrDefault(x => x.Name == "Component")?.GetValue(menuItem.CustomData).ToString(),
                        Redirect = props.FirstOrDefault(x => x.Name == "Redirect")?.GetValue(menuItem.CustomData).ToString(),
                        Meta = new RouteItemMetaForVben
                        {
                            OrderNo = menuItem.Order,
                            Title = menuItem.DisplayName,
                            Icon = menuItem.Icon,
                            CssClass = menuItem.CssClass,
                            HideChildrenInMenu = Convert.ToBoolean(props.FirstOrDefault(x => x.Name == "HideChildrenInMenu")?.GetValue(menuItem.CustomData)),
                            HideMenu = Convert.ToBoolean(props.FirstOrDefault(x => x.Name == "HideMenu")?.GetValue(menuItem.CustomData)),
                            HideTab = Convert.ToBoolean(props.FirstOrDefault(x => x.Name == "HideTab")?.GetValue(menuItem.CustomData)),
                        },
                        Children = ConvertToRouteItemForVben(menuItem.Items)
                    };

                    result.Add(routeItem);
                }
            }

            return result;
        }

        private async Task<ApplicationMenu> GetUserMenuAsync([NotNull] string standardMenus)
        {
            Check.NotNullOrEmpty(standardMenus, nameof(standardMenus));

            var menus = await _menuManager.GetAsync(standardMenus);

            return menus;
        }
    }
}
