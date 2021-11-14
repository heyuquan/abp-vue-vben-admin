using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Leopard.Identity
{
    /// <summary>
    /// 组织单元服务
    /// </summary>
    public interface IOrganizationUnitAppService : IApplicationService, IRemoteService
    {
        /// <summary>
        /// 根据id获取组织
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrganizationUnitWithDetailsDto> GetAsync(Guid id);
        /// <summary>
        /// 根据过滤条件获取组织列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OrganizationUnitWithDetailsDto>> GetListAsync(GetOrganizationUnitInput input);
        /// <summary>
        /// 获取所有组织
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<OrganizationUnitWithDetailsDto>> GetListAllAsync();
        /// <summary>
        /// 获取没有加入指定组织的成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IdentityUserDto>> GetUnaddedMembersAsync(Guid id, OrganizationUnitGetUnaddedUserInput input);

        /// <summary>
        /// 获取组织成员列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IdentityUserDto>> GetMembersAsync(Guid id, GetIdentityUsersInput input);
        /// <summary>
        /// 移除组织成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        Task RemoveMemberAsync(Guid id, Guid memberId);
        /// <summary>
        /// 获取没有加入指定组织的角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IdentityRoleDto>> GetUnaddedRolesAsync(Guid id, OrganizationUnitGetUnaddedRoleInput input);

        /// <summary>
        /// 获取组织角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, PagedAndSortedResultRequestDto input);
        /// <summary>
        /// 移除组织角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task RemoveRoleAsync(Guid id, Guid roleId);
        /// <summary>
        /// 创建组织
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrganizationUnitWithDetailsDto> CreateAsync(OrganizationUnitCreateDto input);
        /// <summary>
        /// 删除组织
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
        /// <summary>
        /// 更新组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrganizationUnitWithDetailsDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input);
        /// <summary>
        /// 给组织添加角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddRolesAsync(Guid id, OrganizationUnitRoleInput input);
        /// <summary>
        /// 给组织添加成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AddMembersAsync(Guid id, OrganizationUnitUserInput input);
        /// <summary>
        /// 更换组织父节点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task MoveAsync(Guid id, OrganizationUnitMoveInput input);
    }
}
