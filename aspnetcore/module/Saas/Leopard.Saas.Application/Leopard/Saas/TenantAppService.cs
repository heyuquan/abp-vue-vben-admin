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

            return ObjectMapper.Map<Tenant, SaasTenantDto>(tenant);
        }

        public virtual async Task<PagedResultDto<SaasTenantDto>> GetListAsync(GetTenantsInput input)
        {
            var list = await TenantRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
            var totalCount = await TenantRepository.GetCountAsync(input.Filter);

            return new PagedResultDto<SaasTenantDto>(
                totalCount,
                ObjectMapper.Map<List<Tenant>, List<SaasTenantDto>>(list)
                );
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

            return ObjectMapper.Map<Tenant, SaasTenantDto>(tenant);
        }

        [Authorize(SaasPermissions.Tenants.Update)]
        public virtual async Task<SaasTenantDto> UpdateAsync(Guid id, SaasTenantUpdateDto input)
        {
            var tenant = await TenantRepository.GetAsync(id);

            if (tenant.Name != input.Name)
                await TenantManager.ChangeNameAsync(tenant, input.Name);

            input.MapExtraPropertiesTo(tenant);
            tenant.EditionId = input.EditionId;

            return ObjectMapper.Map<Tenant, SaasTenantDto>(tenant);
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
        public virtual async Task<string> GetDefaultConnectionStringAsync(Guid id)
        {
            var tenant = await this.TenantRepository.GetAsync(id);
            return (tenant != null) ? tenant.FindDefaultConnectionString() : null;
        }

        [Authorize(SaasPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
        {
            var tenant = await this.TenantRepository.GetAsync(id);
            tenant.SetDefaultConnectionString(defaultConnectionString);
            await this.TenantRepository.UpdateAsync(tenant);
        }

        [Authorize(SaasPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task DeleteDefaultConnectionStringAsync(Guid id)
        {
            var tenant = await this.TenantRepository.GetAsync(id);
            tenant.RemoveDefaultConnectionString();
            await this.TenantRepository.UpdateAsync(tenant);
        }
    }
}
