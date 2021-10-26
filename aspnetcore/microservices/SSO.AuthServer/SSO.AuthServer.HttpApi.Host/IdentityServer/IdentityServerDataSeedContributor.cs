using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace SSO.AuthServer.IdentityServer
{
    public class IdentityServerDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceDataSeeder _identityResourceDataSeeder;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IConfiguration _configuration;
        private readonly ICurrentTenant _currentTenant;

        public IdentityServerDataSeedContributor(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository,
            IIdentityResourceDataSeeder identityResourceDataSeeder,
            IGuidGenerator guidGenerator,
            IPermissionDataSeeder permissionDataSeeder,
            IConfiguration configuration,
            ICurrentTenant currentTenant)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _apiScopeRepository = apiScopeRepository;
            _identityResourceDataSeeder = identityResourceDataSeeder;
            _guidGenerator = guidGenerator;
            _permissionDataSeeder = permissionDataSeeder;
            _configuration = configuration;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                await _identityResourceDataSeeder.CreateStandardResourcesAsync();
                await CreateApiResourcesAsync();
                await CreateApiScopesAsync();
                await CreateClientsAsync();
            }
        }

        IEnumerable<String> AllApiScopes
        {
            get
            {
                var result = _configuration.GetSection("IdentityServer:ApiScopes").GetChildren().Select(x => x.Value);
                if (result == null)
                {
                    throw new Exception("�����ļ�ȱ�� IdentityServer:ApiScopes �ڵ�");
                }
                return result;
            }
        }

        private async Task CreateApiScopesAsync()
        {
            foreach (var item in AllApiScopes)
            {
                await CreateApiScopeAsync(item);
            }
        }

        private async Task CreateApiResourcesAsync()
        {
            var commonApiUserClaims = new[]
            {
                "email",
                "email_verified",
                "name",
                "phone_number",
                "phone_number_verified",
                "role"
            };

            foreach (var item in AllApiScopes)
            {
                await CreateApiResourceAsync(item, commonApiUserClaims);
            }
        }

        private async Task<ApiResource> CreateApiResourceAsync(string name, IEnumerable<string> claims)
        {
            var apiResource = await _apiResourceRepository.FindByNameAsync(name);
            if (apiResource == null)
            {
                apiResource = await _apiResourceRepository.InsertAsync(
                    new ApiResource(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }

            foreach (var claim in claims)
            {
                if (apiResource.FindClaim(claim) == null)
                {
                    apiResource.AddUserClaim(claim);
                }
            }

            return await _apiResourceRepository.UpdateAsync(apiResource);
        }

        private async Task<ApiScope> CreateApiScopeAsync(string name)
        {
            var apiScope = await _apiScopeRepository.GetByNameAsync(name);
            if (apiScope == null)
            {
                apiScope = await _apiScopeRepository.InsertAsync(
                    new ApiScope(
                        _guidGenerator.Create(),
                        name,
                        name + " API"
                    ),
                    autoSave: true
                );
            }

            return apiScope;
        }

        private async Task CreateClientsAsync()
        {
            var commonScopes = new[]
            {
                "email",
                "openid",
                "profile",
                "role",
                "phone",
                "address",
                "SSOAuthServerService"
            };

            // �����Ĳ���
            //{
            //    "ClientId": "",(����)
            //    "RedirectUris": [
            //      "",
            //      ""
            //     ],(����)
            //    "PostLogoutRedirectUris": [
            //      "",
            //      ""
            //     ],(ѡ�Ĭ�ϣ���)
            //    "Scopes": [],(ѡ�Ĭ�ϣ�������Χ)
            //    "GrantTypes": [ "authorization_code" ],(����)
            //    "RequirePkce": ��ѡ�Ĭ�ϣ�false��
            //    "RequireClientSecret":��ѡ�Ĭ�ϣ�true��
            //    "ClientSecret": ��ѡ�Ĭ�ϣ�1q2w3E*��
            //    "AllowAccessTokensViaBrowser"����ѡ�Ĭ�ϣ�false��
            //    "Permissions":��ѡ�Ĭ�ϣ��գ�
            //}
            var configurationSection = _configuration.GetSection("IdentityServer:Clients");
            foreach (var section in configurationSection.GetChildren())
            {
                var clientId = section["ClientId"];
                if (!clientId.IsNullOrWhiteSpace())
                {
                    bool allowAccessTokensViaBrowser = false;
                    IEnumerable<string> redirectUris = section.GetSection("RedirectUris").GetChildren().Select(x => x.Value); ;
                    IEnumerable<string> cors = section.GetSection("Cors").GetChildren().Select(x => x.Value);
                    IEnumerable<string> postLogoutRedirectUris = section.GetSection("PostLogoutRedirectUris").GetChildren().Select(x => x.Value); 
                    if (bool.TryParse(section["RequirePkce"], out bool requirePkce))
                    {
                        //����ֵ
                    }
                    bool requireClientSecret = true;
                    if (bool.TryParse(section["RequireClientSecret"], out requireClientSecret))
                    {
                        //����ֵ
                    }
                    if (bool.TryParse(section["AllowAccessTokensViaBrowser"], out allowAccessTokensViaBrowser))
                    {
                        //����ֵ
                    }
                    await CreateClientAsync(
                        clientId: clientId,
                        scopes: commonScopes.Union(section.GetSection("Scopes").GetChildren().Select(x => x.Value)).Distinct(),
                        grantTypes: section.GetSection("GrantTypes").GetChildren().Select(x => x.Value),
                        secret: IdentityServer4.Models.HashExtensions.Sha256(section["ClientSecret"] ?? "1q2w3E*"),
                        corsOrigins: cors,
                        requirePkce: requirePkce,
                        requireClientSecret: requireClientSecret,
                        redirectUris: redirectUris,
                        postLogoutRedirectUris: postLogoutRedirectUris,
                        allowAccessTokensViaBrowser: allowAccessTokensViaBrowser,
                        permissions: section.GetSection("Permissions").GetChildren().Select(x => x.Value)
                    );
                }
            }
        }

        /// <summary>
        /// ������ݱ�ʶ�ͻ���
        /// </summary>
        /// <param name="clientId">�ͻ���Id</param>
        /// <param name="scopes">������ʵ���Դ</param>
        /// <param name="grantTypes">����ͻ���ʹ�õ���Ȩ����</param>
        /// <param name="secret">�ͻ�������</param>
        /// <param name="requirePkce">ָ��ʹ�û�����Ȩ�������Ȩ���͵Ŀͻ����Ƿ���뷢��У����Կ</param>
        /// <param name="requireClientSecret">ָ���˿ͻ����Ƿ���Ҫ��Կ���ܴ����ƶ˵��������ƣ�Ĭ��Ϊtrue��</param>
        /// <param name="redirectUris"></param>
        /// <param name="corsOrigins"></param>
        /// <param name="postLogoutRedirectUris">ָ����ע�����ض��򵽵�����URI.(һ��clientId�����url)</param>
        /// <param name="frontChannelLogoutUri">ָ���ͻ��˵�ע��URI�������ڻ���HTTP��ǰ��ͨ��ע����</param>
        /// <param name="allowAccessTokensViaBrowser">����tokenͨ�����������</param>
        /// <param name="permissions">Ĭ�ϸ�Ȩ</param>
        /// <param name=""></param>
        /// <returns></returns>
        private async Task<Client> CreateClientAsync(
            string clientId,
            IEnumerable<string> scopes,
            IEnumerable<string> grantTypes,
            string secret = null,
            bool requirePkce = false,
            bool requireClientSecret = true,
            IEnumerable<string> redirectUris = null,
            IEnumerable<string> corsOrigins = null,
            IEnumerable<string> postLogoutRedirectUris = null,
            string frontChannelLogoutUri = null,
            bool allowAccessTokensViaBrowser = false,
            IEnumerable<string> permissions = null
            )
        {
            // �ο���Client - Identity Server 4 �����ĵ�(v1.0.0)
            // https://www.cnblogs.com/thinksjay/p/10787349.html

            var client = await _clientRepository.FindByClientIdAsync(clientId);
            if (client == null)
            {
                client = new Client(
                        _guidGenerator.Create(),
                        clientId
                    )
                    {
                        ClientName = clientId,
                        ProtocolType = IdentityServerConstants.ProtocolTypes.OpenIdConnect,
                        Description = clientId,
                        // ����ID_TOKEN����Claims
                        AlwaysIncludeUserClaimsInIdToken = true,
                        // ָ���˿ͻ����Ƿ��������ˢ�����ƣ�����offline_access��Χ��
                        AllowOfflineAccess = true,
                        // ����tokenͨ�����������
                        AllowAccessTokensViaBrowser = allowAccessTokensViaBrowser,

                        // ˢ�����Ƶ���������ڣ��룩��Ĭ��Ϊ2592000��/ 30��
                        AbsoluteRefreshTokenLifetime = 60 * 60 * 12, //1 days
                                                                     // ����ˢ�����Ƶ��������ڣ�����Ϊ��λ��Ĭ��Ϊ1296000��/ 15��
                        SlidingRefreshTokenLifetime = 60 * 60 * 12,//1 days
                                                                   // �������Ƶ��������ڣ�����Ϊ��λ��Ĭ��Ϊ3600��/ 1Сʱ��
                        AccessTokenLifetime = 60 * 60 * 12, //1 days
                                                            // ��Ȩ������������ڣ�����Ϊ��λ��Ĭ��Ϊ300��/ 5���ӣ�
                        AuthorizationCodeLifetime = 60 * 24 * 12,//1 days
                                                                 // ������Ƶ��������ڣ�����Ϊ��λ��Ĭ��Ϊ300��/ 5���ӣ�
                        IdentityTokenLifetime = 60 * 60 * 12,//1 days

                        // ָ���Ƿ���Ҫͬ����Ļ��Ĭ��Ϊtrue��
                        RequireConsent = false,                        
                        // ָ���ͻ��˵�ע��URI�������ڻ���HTTP��ǰ��ͨ��ע����
                        FrontChannelLogoutUri = frontChannelLogoutUri,
                        // ָ���˿ͻ����Ƿ���Ҫ��Կ���ܴ����ƶ˵��������ƣ�Ĭ��Ϊtrue��
                        RequireClientSecret = requireClientSecret,
                        // ָ��ʹ�û�����Ȩ�������Ȩ���͵Ŀͻ����Ƿ���뷢��У����Կ
                        RequirePkce = requirePkce
                    };
                client = await _clientRepository.InsertAsync(client, autoSave: true);
            }

            foreach (var scope in scopes)
            {
                if (client.FindScope(scope) == null)
                {
                    client.AddScope(scope);
                }
            }

            foreach (var grantType in grantTypes)
            {
                if (client.FindGrantType(grantType) == null)
                {
                    client.AddGrantType(grantType);
                }
            }

            if (!secret.IsNullOrEmpty())
            {
                if (client.FindSecret(secret) == null)
                {
                    client.AddSecret(secret);
                }
            }

            if (redirectUris != null)
            {
                foreach (var redirectUri in redirectUris)
                {
                    if (!redirectUri.IsNullOrWhiteSpace())
                    {
                        if (client.FindRedirectUri(redirectUri) == null)
                        {
                            client.AddRedirectUri(redirectUri);
                        }
                    }
                }
            }

            if (postLogoutRedirectUris != null)
            {
                foreach (var postLogoutRedirectUri in postLogoutRedirectUris)
                {
                    if (!postLogoutRedirectUri.IsNullOrWhiteSpace())
                    {
                        if (client.FindPostLogoutRedirectUri(postLogoutRedirectUri) == null)
                        {
                            client.AddPostLogoutRedirectUri(postLogoutRedirectUri);
                        }
                    }
                }
            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(
                    ClientPermissionValueProvider.ProviderName,
                    clientId,
                    permissions,
                    null
                );
            }

            if (corsOrigins != null)
            {
                foreach (var origin in corsOrigins)
                {
                    if (!origin.IsNullOrWhiteSpace() && client.FindCorsOrigin(origin) == null)
                    {
                        client.AddCorsOrigin(origin);
                    }
                }
            }

            return await _clientRepository.UpdateAsync(client);
        }
    }
}
