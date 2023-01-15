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

"IdentityServer": {
    // EShop.AuthServer.IdentityServer.Web 自身不需要在ApiScopes资源里面，
    // 但是其 EShop.AuthServer.HttpApi.Host 是需要在ApiScopes资源里面的
    "ApiScopes": [
      "SSOAuthServerService",
      "AdministrationService",

      "MkDemoBService",
      "MkDemoCService",

      "AdministrationAppGateway",
      "InternalGateway",
      "PublicWebSiteGateway"
    ],

    "Clients": [
      // Service ====================================================================================
      {
        "ClientId": "MkDemoBService",
        "RedirectUris": [
          "https://localhost:44305/swagger/oauth2-redirect.html",
          "https://159.75.253.251:44305/swagger/oauth2-redirect.html",
          "http://159.75.253.251:44302/swagger/oauth2-redirect.html"
        ],
        "Scopes": [ "MkDemoBService", "SSOAuthServerService", "InternalGateway", "MkDemoCService" ],
        "GrantTypes": [ "authorization_code" ],
        "ClientSecret": "1q2w3E*"
      },
      {
        "ClientId": "MkDemoCService",
        "RedirectUris": [
          "https://localhost:44405/swagger/oauth2-redirect.html",
          "https://159.75.253.251:44405/swagger/oauth2-redirect.html",
          "http://159.75.253.251:44402/swagger/oauth2-redirect.html"
        ],
        "Scopes": [ "MkDemoCService", "SSOAuthServerService", "InternalGateway" ],
        "GrantTypes": [ "authorization_code" ],
        "ClientSecret": "1q2w3E*"
      },
      {
        "ClientId": "SSOAuthServerService",
        "RedirectUris": [
          "https://localhost:44105/swagger/oauth2-redirect.html",
          "https://159.75.253.251:44105/swagger/oauth2-redirect.html",
          "http://159.75.253.251:44102/swagger/oauth2-redirect.html"
        ],
        "Scopes": [ "SSOAuthServerService" ],
        "GrantTypes": [ "authorization_code" ],
        "ClientSecret": "1q2w3E*"
      },
      {
        "ClientId": "AdministrationService",
        "RedirectUris": [
          "https://localhost:44145/swagger/oauth2-redirect.html",
          "https://159.75.253.251:44145/swagger/oauth2-redirect.html",
          "http://159.75.253.251:44142/swagger/oauth2-redirect.html"
        ],
        "Scopes": [ "AdministrationService" ],
        "GrantTypes": [ "authorization_code" ],
        "ClientSecret": "1q2w3E*"
      },
      // gateway ====================================================================================
      {
        "ClientId": "AdministrationAppGateway",
        "RedirectUris": [
          "https://localhost:44805/swagger/oauth2-redirect.html",
          "https://159.75.253.251:44805/swagger/oauth2-redirect.html",
          "http://159.75.253.251:44802/swagger/oauth2-redirect.html"
        ],
        "Scopes": [ "AdministrationAppGateway", "SSOAuthServerService", "AdministrationService" ],
        "GrantTypes": [ "authorization_code" ],
        "ClientSecret": "1q2w3E*"
      },
      {
        "ClientId": "InternalGateway",
        "RedirectUris": [
          "https://localhost:44815/swagger/oauth2-redirect.html",
          "https://159.75.253.251:44815/swagger/oauth2-redirect.html",
          "http://159.75.253.251:44812/swagger/oauth2-redirect.html"
        ],
        "Scopes": [ "InternalGateway", "SSOAuthServerService" ],
        "GrantTypes": [ "authorization_code" ],
        "ClientSecret": "1q2w3E*"
      },
      {
        "ClientId": "PublicWebSiteGateway",
        "RedirectUris": [
          "https://localhost:44825/swagger/oauth2-redirect.html",
          "https://159.75.253.251:44825/swagger/oauth2-redirect.html",
          "http://159.75.253.251:44822/swagger/oauth2-redirect.html"
        ],
        "Scopes": [ "PublicWebSiteGateway", "SSOAuthServerService" ],
        "GrantTypes": [ "authorization_code" ],
        "ClientSecret": "1q2w3E*"
      },
      // client ====================================================================================
      {
        "ClientId": "VbenAdminWeb",
        "RedirectUris": [
          "https://localhost:44344/signin-oidc",
          "https://159.75.253.251:44344/signin-oidc",
          "http://159.75.253.251:44342/signin-oidc"
        ],
        "Scopes": [ "AdministrationAppGateway" ],
        "GrantTypes": [ "authorization_code" ],
        "RequirePkce": true,
        "ClientSecret": "1q2w3E*"
      },
      {
        "ClientId": "Administration_Web",
        "RedirectUris": [
          "https://localhost:4200/signin-oidc",
          "https://159.75.253.251/signin-oidc",
          "http://159.75.253.251/signin-oidc"
        ],
        "Scopes": [ "AdministrationAppGateway", "SSOAuthServerService", "AdministrationService" ],
        "GrantTypes": [ "password" ],
        "RequirePkce": true,
        "ClientSecret": "1q2w3E*",
        "Cors": [
          "http://159.75.253.251",
          "http://localhost:4200"
        ]
      }
    ]
  }

namespace EShop.Identity.IdentityServer.Web
{
    // 原来Identityserver4的种子，留个备份
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
                    throw new Exception("配置文件缺少 IdentityServer:ApiScopes 节点");
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
            var apiScope = await _apiScopeRepository.FindByNameAsync(name);
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

            // 完整的参数
            //{
            //    "ClientId": "",(必填)
            //    "RedirectUris": [
            //      "",
            //      ""
            //     ],(必填)
            //    "PostLogoutRedirectUris": [
            //      "",
            //      ""
            //     ],(选填，默认：空)
            //    "Scopes": [],(选填，默认：公共范围)
            //    "GrantTypes": [ "authorization_code" ],(必填)
            //    "RequirePkce": （选填，默认：false）
            //    "RequireClientSecret":（选填，默认：true）
            //    "ClientSecret": （选填，默认：1q2w3E*）
            //    "AllowAccessTokensViaBrowser"：（选填，默认：false）
            //    "Permissions":（选填，默认：空）
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
                        //设置值
                    }
                    bool requireClientSecret = true;
                    if (bool.TryParse(section["RequireClientSecret"], out requireClientSecret))
                    {
                        //设置值
                    }
                    if (bool.TryParse(section["AllowAccessTokensViaBrowser"], out allowAccessTokensViaBrowser))
                    {
                        //设置值
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
        /// 创建身份标识客户端
        /// </summary>
        /// <param name="clientId">客户端Id</param>
        /// <param name="scopes">允许访问的资源</param>
        /// <param name="grantTypes">允许客户端使用的授权类型</param>
        /// <param name="secret">客户端密码</param>
        /// <param name="requirePkce">指定使用基于授权代码的授权类型的客户端是否必须发送校验密钥</param>
        /// <param name="requireClientSecret">指定此客户端是否需要密钥才能从令牌端点请求令牌（默认为true）</param>
        /// <param name="redirectUris"></param>
        /// <param name="corsOrigins"></param>
        /// <param name="postLogoutRedirectUris">指定在注销后重定向到的允许URI.(一个clientId，多个url)</param>
        /// <param name="frontChannelLogoutUri">指定客户端的注销URI，以用于基于HTTP的前端通道注销。</param>
        /// <param name="allowAccessTokensViaBrowser">允许将token通过浏览器传递</param>
        /// <param name="permissions">默认赋权</param>
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
            // 参考：Client - Identity Server 4 中文文档(v1.0.0)
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
                        // 允许ID_TOKEN附带Claims
                        AlwaysIncludeUserClaimsInIdToken = true,
                        // 指定此客户端是否可以请求刷新令牌（请求offline_access范围）
                        AllowOfflineAccess = true,
                        // 允许将token通过浏览器传递
                        AllowAccessTokensViaBrowser = allowAccessTokensViaBrowser,

                        // 刷新令牌的最长生命周期（秒）。默认为2592000秒/ 30天
                        AbsoluteRefreshTokenLifetime = 60 * 60 * 12, //1 days
                                                                     // 滑动刷新令牌的生命周期，以秒为单位。默认为1296000秒/ 15天
                        SlidingRefreshTokenLifetime = 60 * 60 * 12,//1 days
                                                                   // 访问令牌的生命周期，以秒为单位（默认为3600秒/ 1小时）
                        AccessTokenLifetime = 60 * 60 * 12, //1 days
                                                            // 授权代码的生命周期，以秒为单位（默认为300秒/ 5分钟）
                        AuthorizationCodeLifetime = 60 * 24 * 12,//1 days
                                                                 // 身份令牌的生命周期，以秒为单位（默认为300秒/ 5分钟）
                        IdentityTokenLifetime = 60 * 60 * 12,//1 days

                        // 指定是否需要同意屏幕。默认为true。
                        RequireConsent = false,                        
                        // 指定客户端的注销URI，以用于基于HTTP的前端通道注销。
                        FrontChannelLogoutUri = frontChannelLogoutUri,
                        // 指定此客户端是否需要密钥才能从令牌端点请求令牌（默认为true）
                        RequireClientSecret = requireClientSecret,
                        // 指定使用基于授权代码的授权类型的客户端是否必须发送校验密钥
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
