using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Leopard.Identity
{
    /// <summary>
    /// 组织单元服务
    /// </summary>
    [Route("api/identity/organization-units")]
    [ControllerName("OrganizationUnit")]
    [RemoteService(true, Name = IdentityProRemoteServiceConsts.RemoteServiceName)]
    [Area("identity")]
    public class OrganizationUnitController : AbpController, IOrganizationUnitAppService, IApplicationService, IRemoteService
    {
        protected IOrganizationUnitAppService OrganizationUnitAppService { get; }

        public OrganizationUnitController(IOrganizationUnitAppService organizationUnitAppService)
        {
            OrganizationUnitAppService = organizationUnitAppService;
        }

        /// <summary>
        /// 给组织添加角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/roles")]
        [HttpPut]
        public virtual Task AddRolesAsync(Guid id, OrganizationUnitRoleInput input)
        {
            return this.OrganizationUnitAppService.AddRolesAsync(id, input);
        }

        /// <summary>
        /// 给组织添加成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/members")]
        [HttpPut]
        public virtual Task AddMembersAsync(Guid id, OrganizationUnitUserInput input)
        {
            return this.OrganizationUnitAppService.AddMembersAsync(id, input);
        }

        /// <summary>
        /// 创建组织
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual Task<OrganizationUnitWithDetailsDto> CreateAsync(OrganizationUnitCreateDto input)
        {
            return this.OrganizationUnitAppService.CreateAsync(input);
        }

        /// <summary>
        /// 删除组织
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpDelete]
        public virtual Task DeleteAsync(Guid id)
        {
            return this.OrganizationUnitAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 根据id获取组织
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public virtual Task<OrganizationUnitWithDetailsDto> GetAsync(Guid id)
        {
            return this.OrganizationUnitAppService.GetAsync(id);
        }

        /// <summary>
        /// 根据过滤条件获取组织列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual Task<PagedResultDto<OrganizationUnitWithDetailsDto>> GetListAsync(GetOrganizationUnitInput input)
        {
            return this.OrganizationUnitAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取所有组织
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public virtual Task<ListResultDto<OrganizationUnitWithDetailsDto>> GetListAllAsync()
        {
            return this.OrganizationUnitAppService.GetListAllAsync();
        }

        /// <summary>
        /// 获取组织角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/roles")]
        [HttpGet]
        public virtual Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            return this.OrganizationUnitAppService.GetRolesAsync(id, input);
        }

        /// <summary>
        /// 获取没有加入指定组织的角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/unadded-roles")]
        [HttpGet]
        public virtual Task<PagedResultDto<IdentityRoleDto>> GetUnaddedRolesAsync(Guid id, OrganizationUnitGetUnaddedRoleInput input)
        {
            return this.OrganizationUnitAppService.GetUnaddedRolesAsync(id, input);
        }

        /// <summary>
        /// 获取组织成员列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/members")]
        [HttpGet]
        public virtual Task<PagedResultDto<IdentityUserDto>> GetMembersAsync(Guid id, GetIdentityUsersInput input)
        {
            return this.OrganizationUnitAppService.GetMembersAsync(id, input);
        }
        /// <summary>
        /// 获取没有加入指定组织的成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/unadded-members")]
        [HttpGet]
        public virtual Task<PagedResultDto<IdentityUserDto>> GetUnaddedMembersAsync(Guid id, OrganizationUnitGetUnaddedUserInput input)
        {
            return this.OrganizationUnitAppService.GetUnaddedMembersAsync(id, input);
        }

        /// <summary>
        /// 更换组织父节点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("{id}/move")]
        [HttpPut]
        public virtual Task MoveAsync(Guid id, OrganizationUnitMoveInput input)
        {
            return this.OrganizationUnitAppService.MoveAsync(id, input);
        }

        /// <summary>
        /// 更新组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public virtual Task<OrganizationUnitWithDetailsDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
        {
            return this.OrganizationUnitAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 移除组织成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [Route("{id}/members/{memberId}")]
        [HttpDelete]
        public virtual Task RemoveMemberAsync(Guid id, Guid memberId)
        {
            return this.OrganizationUnitAppService.RemoveMemberAsync(id, memberId);
        }

        /// <summary>
        /// 移除组织角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [Route("{id}/roles/{roleId}")]
        [HttpDelete]
        public virtual Task RemoveRoleAsync(Guid id, Guid roleId)
        {
            return this.OrganizationUnitAppService.RemoveRoleAsync(id, roleId);
        }
    }
}
