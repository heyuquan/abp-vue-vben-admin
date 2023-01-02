using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace EShop.AuthServer.IdentityServer;

/* Creates initial data that is needed to property run the application
 * and make client-to-server communication possible.
 */
public class OpenIddictDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IConfiguration _configuration;
    private readonly IAbpApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IStringLocalizer<OpenIddictResponse> L;

    public OpenIddictDataSeedContributor(
        IConfiguration configuration,
        IAbpApplicationManager applicationManager,
        IOpenIddictScopeManager scopeManager,
        IPermissionDataSeeder permissionDataSeeder,
        IStringLocalizer<OpenIddictResponse> l)
    {
        _configuration = configuration;
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
        _permissionDataSeeder = permissionDataSeeder;
        L = l;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateScopesAsync();
        await CreateApplicationsAsync();
    }

    private async Task CreateScopesAsync()
    {
        // 新建 EShop.AuthServer.Service 范围，后续设置为公共的
        if (await _scopeManager.FindByNameAsync("EShop.AuthServer.Service") == null)
        {
            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "EShop.AuthServer.Service",
                DisplayName = "EShop.AuthServer.Service API",
                Resources =
                {
                    "EShop.AuthServer.Service"
                }
            });
        }
    }

    private async Task CreateApplicationsAsync()
    {
        var commonScopes = new List<string>
        {
            OpenIddictConstants.Permissions.Scopes.Address,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles,
            "EShop.AuthServer.Service"
        };

        var configurationSection = _configuration.GetSection("OpenIddict:Applications");

        // EShop.AuthServer.Service.Swagger Client
        var authServerSwaggerClientIdName = "EShop.AuthServer.Service.Swagger";
        var authServerSwaggerClientId = configurationSection[$"{authServerSwaggerClientIdName}:ClientId"];
        if (!authServerSwaggerClientId.IsNullOrWhiteSpace())
        {
            var swaggerRootUrl = configurationSection[$"{authServerSwaggerClientIdName}:RootUrl"].TrimEnd('/');

            await CreateApplicationAsync(
                name: authServerSwaggerClientId,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: authServerSwaggerClientIdName,
                secret: null,
                grantTypes: new List<string>
                {
                    OpenIddictConstants.GrantTypes.AuthorizationCode,
                },
                scopes: commonScopes,
                redirectUri: $"{swaggerRootUrl}/swagger/oauth2-redirect.html",
                clientUri: swaggerRootUrl
            );
        }

        //EShop.AuthServer.IdentityServer.Web Client
        var identityServerClientIdName = "EShop.AuthServer.IdentityServer.Web";
        var identityServerClientId = configurationSection[$"{identityServerClientIdName}:ClientId"];
        if (!identityServerClientId.IsNullOrWhiteSpace())
        {
            var identityServerRootUrl = configurationSection[$"{identityServerClientIdName}:RootUrl"].EnsureEndsWith('/');

            /* oidctest_Web client is only needed if you created a tiered
             * solution. Otherwise, you can delete this client. */
            await CreateApplicationAsync(
                name: identityServerClientId,
                type: OpenIddictConstants.ClientTypes.Confidential,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: identityServerClientIdName,
                secret: configurationSection[$"{identityServerClientIdName}:ClientSecret"] ?? "1q2w3e*",
                grantTypes: new List<string> 
                {
                    OpenIddictConstants.GrantTypes.AuthorizationCode,
                    //OpenIddictConstants.GrantTypes.Implicit
                },
                scopes: commonScopes,
                redirectUri: $"{identityServerRootUrl}signin-oidc",
                clientUri: identityServerRootUrl,
                postLogoutRedirectUri: $"{identityServerRootUrl}signout-callback-oidc"
            );
        }
        // EShop.Administration.Web
        var administrationClientIdName = "EShop.Administration.Web";
        var administrationClientId = configurationSection[$"{administrationClientIdName}:ClientId"];
        if (!administrationClientId.IsNullOrWhiteSpace())
        {
            var administrationClientRootUrl = configurationSection[$"{administrationClientIdName}:RootUrl"]?.TrimEnd('/');
            await CreateApplicationAsync(
                name: administrationClientId,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: administrationClientIdName,
                secret: null,
                grantTypes: new List<string>
                {
                    OpenIddictConstants.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.GrantTypes.Password,
                    //OpenIddictConstants.GrantTypes.ClientCredentials,
                    OpenIddictConstants.GrantTypes.RefreshToken
                },
                scopes: commonScopes,
                redirectUri: administrationClientRootUrl,
                clientUri: administrationClientRootUrl,
                postLogoutRedirectUri: administrationClientRootUrl
            );
        }
    }

    // #、客户端类型 ClientType
    // 私密客户端：能够保障其凭证的保密性的客户端（如在安全的服务器上部署的客户端，对其凭证的访问受到限制），或者有办法通过别的方式进行安全认证的客户端。
    // 公开客户端：客户端无法保障其凭证的保密性（如客户端运行在资源所有者的设备上，比如是本地应用或基于web浏览器的应用），也无法通过别的方式进行安全认证的客户端。

    /// <summary>
    /// Application客户端注册
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="consentType"></param>
    /// <param name="displayName"></param>
    /// <param name="secret"></param>
    /// <param name="grantTypes">允许客户端使用的授权类型</param>
    /// <param name="scopes">允许访问的资源</param>
    /// <param name="clientUri"></param>
    /// <param name="redirectUri"></param>
    /// <param name="postLogoutRedirectUri">指定在注销后重定向到的允许URI</param>
    /// <param name="permissions">默认赋权</param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    private async Task CreateApplicationAsync(
        [NotNull] string name,
        [NotNull] string type,
        [NotNull] string consentType,
        string displayName,
        string secret,
        List<string> grantTypes,
        List<string> scopes,
        string clientUri = null,
        string redirectUri = null,
        string postLogoutRedirectUri = null,
        List<string> permissions = null)
    {
        if (!string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException(L["NoClientSecretCanBeSetForPublicApplications"]);
        }

        if (string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException(L["TheClientSecretIsRequiredForConfidentialApplications"]);
        }

        if (!string.IsNullOrEmpty(name) && await _applicationManager.FindByClientIdAsync(name) != null)
        {
            return;
            //throw new BusinessException(L["TheClientIdentifierIsAlreadyTakenByAnotherApplication"]);
        }

        var client = await _applicationManager.FindByClientIdAsync(name);
        if (client == null)
        {
            var application = new AbpApplicationDescriptor
            {
                ClientId = name,
                Type = type,
                ClientSecret = secret,
                ConsentType = consentType,
                DisplayName = displayName,
                ClientUri = clientUri,
            };

            Check.NotNullOrEmpty(grantTypes, nameof(grantTypes));
            Check.NotNullOrEmpty(scopes, nameof(scopes));

            if (new [] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }.All(grantTypes.Contains))
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);

                if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
                }
            }

            if (!redirectUri.IsNullOrWhiteSpace() || !postLogoutRedirectUri.IsNullOrWhiteSpace())
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);
            }

            foreach (var grantType in grantTypes)
            {
                if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
                }

                if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode || grantType == OpenIddictConstants.GrantTypes.Implicit)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                }

                if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                    grantType == OpenIddictConstants.GrantTypes.ClientCredentials ||
                    grantType == OpenIddictConstants.GrantTypes.Password ||
                    grantType == OpenIddictConstants.GrantTypes.RefreshToken ||
                    grantType == OpenIddictConstants.GrantTypes.DeviceCode)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                }

                if (grantType == OpenIddictConstants.GrantTypes.ClientCredentials)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                }

                if (grantType == OpenIddictConstants.GrantTypes.Implicit)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
                }

                if (grantType == OpenIddictConstants.GrantTypes.Password)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                }

                if (grantType == OpenIddictConstants.GrantTypes.RefreshToken)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
                }

                if (grantType == OpenIddictConstants.GrantTypes.DeviceCode)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
                }

                if (grantType == OpenIddictConstants.GrantTypes.Implicit)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
                    if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                    }
                }
            }

            var buildInScopes = new []
            {
                OpenIddictConstants.Permissions.Scopes.Address,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Phone,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles
            };

            foreach (var scope in scopes)
            {
                if (buildInScopes.Contains(scope))
                {
                    application.Permissions.Add(scope);
                }
                else
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
                }
            }

            if (redirectUri != null)
            {
                if (!redirectUri.IsNullOrEmpty())
                {
                    if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                    {
                        throw new BusinessException(L["InvalidRedirectUri", redirectUri]);
                    }

                    if (application.RedirectUris.All(x => x != uri))
                    {
                        application.RedirectUris.Add(uri);
                    }
                }
            }

            if (postLogoutRedirectUri != null)
            {
                if (!postLogoutRedirectUri.IsNullOrEmpty())
                {
                    if (!Uri.TryCreate(postLogoutRedirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                    {
                        throw new BusinessException(L["InvalidPostLogoutRedirectUri", postLogoutRedirectUri]);
                    }

                    if (application.PostLogoutRedirectUris.All(x => x != uri))
                    {
                        application.PostLogoutRedirectUris.Add(uri);
                    }
                }
            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(
                    ClientPermissionValueProvider.ProviderName,
                    name,
                    permissions,
                    null
                );
            }

            await _applicationManager.CreateAsync(application);
        }
    }
}
