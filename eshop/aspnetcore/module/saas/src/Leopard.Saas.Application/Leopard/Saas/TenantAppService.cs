using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Leopard.Saas.Dtos;
using Leopard.Saas.Permissions;
using System.Linq;

namespace Leopard.Saas
{
    [Authorize(SaasPermissions.Tenants.Default)]
    public class TenantAppService : SaasAppServiceBase, IDeleteAppService<Guid>, IRemoteService, IApplicationService, IUpdateAppService<SaasTenantDto, Guid, SaasTenantUpdateDto>, ICreateAppService<SaasTenantDto, SaasTenantCreateDto>, ICreateUpdateAppService<SaasTenantDto, Guid, SaasTenantCreateDto, SaasTenantUpdateDto>, IReadOnlyAppService<SaasTenantDto, SaasTenantDto, Guid, GetTenantsInput>, ICrudAppService<SaasTenantDto, SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>, ICrudAppService<SaasTenantDto, Guid, GetTenantsInput, SaasTenantCreateDto, SaasTenantUpdateDto>, ITenantAppService
    {
        protected IEditionRepository EditionRepository { get; }

        protected IDataSeeder DataSeeder { get; }

        protected ITenantRepository TenantRepository { get; }

        protected ITenantManager TenantManager { get; }

        protected IDistributedEventBus DistributedEventBus { get; }


        public TenantAppService(ITenantRepository tenantRepository, IEditionRepository editionRepository,
            ITenantManager tenantManager, IDataSeeder dataSeeder,
            IDistributedEventBus distributedEventBus)
        {
            this.EditionRepository = editionRepository;
            this.DataSeeder = dataSeeder;
            this.TenantRepository = tenantRepository;
            this.TenantManager = tenantManager;
            DistributedEventBus = distributedEventBus;
        }

        public virtual async Task<SaasTenantDto> GetAsync(Guid id)
        {
            var tenant = await TenantRepository.GetAsync(id);

            var returnData = ObjectMapper.Map<Tenant, SaasTenantDto>(tenant);

            if (tenant.EditionId.HasValue)
            {
                returnData.EditionName = (await EditionRepository.GetAsync(tenant.EditionId.Value))?.DisplayName;
            }

            return returnData;
        }

        public virtual async Task<PagedResultDto<SaasTenantDto>> GetListAsync(GetTenantsInput input)
        {
            var list = await TenantRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
            var totalCount = await TenantRepository.GetCountAsync(input.Filter);

            var editionIds = list.Select(x => x.EditionId).ToList();
            var editionDic = (await EditionRepository.GetListAsync(x => editionIds.Contains(x.Id)))
                              .ToDictionary(x => x.Id, x => x.DisplayName);

            var returnData = new PagedResultDto<SaasTenantDto>(
                totalCount,
                ObjectMapper.Map<List<Tenant>, List<SaasTenantDto>>(list)
                );

            foreach (var item in returnData.Items)
            {
                if (item.EditionId.HasValue &&
                    editionDic.TryGetValue(item.EditionId.Value, out string editionName))
                {
                    item.EditionName = editionName;
                }
            }

            return returnData;
        }

        [Authorize(SaasPermissions.Tenants.Create)]
        public virtual async Task<SaasTenantDto> CreateAsync(SaasTenantCreateDto input)
        {
            var tenant = await TenantManager.CreateAsync(input.Name, input.EditionId);
            input.MapExtraPropertiesTo(tenant);

            await TenantRepository.InsertAsync(tenant);

            await CurrentUnitOfWork.SaveChangesAsync();

            await DistributedEventBus.PublishAsync(
                new TenantCreatedEto
                {
                    Id = tenant.Id,
                    Name = tenant.Name,
                    Properties =
                    {
                        { "AdminEmail", input.AdminEmailAddress },
                        { "AdminPassword", input.AdminPassword }
                    }
                });

            using (CurrentTenant.Change(tenant.Id, tenant.Name))
            {
                //TODO: Handle database creation?
                // TODO: Seeder might be triggered via event handler.
                await DataSeeder.SeedAsync(
                                new DataSeedContext(tenant.Id)
                                    .WithProperty("AdminEmail", input.AdminEmailAddress)
                                    .WithProperty("AdminPassword", input.AdminPassword)
                                );
            }

            var returnData = ObjectMapper.Map<Tenant, SaasTenantDto>(tenant);

            if (tenant.EditionId.HasValue)
            {
                returnData.EditionName = (await EditionRepository.GetAsync(tenant.EditionId.Value))?.DisplayName;
            }

            return returnData;
        }

        [Authorize(SaasPermissions.Tenants.Update)]
        public virtual async Task<SaasTenantDto> UpdateAsync(Guid id, SaasTenantUpdateDto input)
        {
            var tenant = await TenantRepository.GetAsync(id);

            if (tenant.Name != input.Name)
                await TenantManager.ChangeNameAsync(tenant, input.Name);

            input.MapExtraPropertiesTo(tenant);
            tenant.EditionId = input.EditionId;

            await TenantRepository.UpdateAsync(tenant);

            var returnData = ObjectMapper.Map<Tenant, SaasTenantDto>(tenant);

            if (tenant.EditionId.HasValue)
            {
                returnData.EditionName = (await EditionRepository.GetAsync(tenant.EditionId.Value))?.DisplayName;
            }

            return returnData;
        }

        [Authorize(SaasPermissions.Tenants.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var tenant = await this.TenantRepository.FindAsync(id);
            if (tenant != null)
            {
                await this.TenantRepository.DeleteAsync(tenant);
            }
        }

        [Authorize(SaasPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringListAsync(Guid id)
        {
            var tenant = await this.TenantRepository.GetAsync(id);
            return (tenant != null)
                ? new ListResultDto<TenantConnectionStringDto>(ObjectMapper.Map<List<TenantConnectionString>, List<TenantConnectionStringDto>>(tenant.ConnectionStrings))
                : null;
        }

        [Authorize(SaasPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task<TenantConnectionStringDto> GetConnectionStringAsync(Guid id, string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            var tenant = await this.TenantRepository.GetAsync(id);
            return (tenant != null)
                ? ObjectMapper.Map<TenantConnectionString, TenantConnectionStringDto>(tenant.ConnectionStrings.FirstOrDefault(x => x.Name == name))
                : null;
        }

        [Authorize(SaasPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task UpdateConnectionStringAsync(Guid id, TenantConnectionStringUpdateDto dto)
        {
            Check.NotNullOrEmpty(dto.Name, nameof(dto.Name));
            var tenant = await this.TenantRepository.GetAsync(id);
            tenant.SetConnectionString(dto.Name, dto.Value);
            await this.TenantRepository.UpdateAsync(tenant);
        }

        [Authorize(SaasPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task DeleteConnectionStringAsync(Guid id, string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            var tenant = await this.TenantRepository.GetAsync(id);
            tenant.RemoveConnectionString(name);
            await this.TenantRepository.UpdateAsync(tenant);
        }
    }
}
