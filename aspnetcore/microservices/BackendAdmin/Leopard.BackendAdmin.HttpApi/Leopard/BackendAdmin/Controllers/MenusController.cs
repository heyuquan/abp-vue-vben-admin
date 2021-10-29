using Leopard.UI.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Leopard.BackendAdmin.Controllers
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
        /// 获取所有后台管理菜单列表（当前用户有权限的）
        /// </summary>
        /// <returns></returns>
        // GET: api/<controller>
        [HttpGet]
        [Route("user-all")]
        public async Task<ApplicationMenu> GetUserAllAsync()
        {
            return await _menuManager.GetAsync(LeopardStandardMenus.Main);
        }        
    }
}
