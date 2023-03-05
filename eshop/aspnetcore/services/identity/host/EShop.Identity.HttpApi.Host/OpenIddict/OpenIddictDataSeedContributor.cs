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

namespace EShop.Identity.OpenIddict;

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

    IEnumerable<Scopes> AllScopes
    {
        get
        {
            var result = _configuration.GetSection("OpenIddict:Scopes").Get<IEnumerable<Scopes>>().ToList();
            if (result == null)
            {
                throw new Exception("配置文件缺少 OpenIddict:Scopes 节点");
            }
            return result;
        }
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateScopesAsync();
        await CreateApplicationsAsync();
    }

    private async Task CreateScopesAsync()
    {
        foreach (var item in AllScopes)
        {
            await CreateApiScopeAsync(item);
        }
    }

    private async Task CreateApiScopeAsync(Scopes scope)
    {
        if (await _scopeManager.FindByNameAsync(scope.Name) == null)
        {
            var scopeDescriptor = new OpenIddictScopeDescriptor
            {
                Name = scope.Name,
                DisplayName = $"{scope.Name} API"
            };
            foreach (var resource in scope.Resources)
            {
                scopeDescriptor.Resources.Add(resource);
            }
            await _scopeManager.CreateAsync(scopeDescriptor);
        }
    }

    /// <summary>
    /// 判断输入uri是否为完整http地址，不完整，则拼接defaultRootUri
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="defaultRootUri"></param>
    /// <returns></returns>
    private string GetFullUri(string uri, string defaultRootUri)
    {
        if (uri.IsNullOrWhiteSpace())
            return defaultRootUri;

        return uri.IsUrlAddress() ? uri : $"{defaultRootUri}{uri}";
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
            "Common"
        };

        // 完整的参数
        //{
        //    "ClientId": "",               （必填）
        //    "ClientUri": "",              （必填）
        //    "ClientType": "",             （必填） 值为：OpenIddictConstants.ClientTypes
        //    "ConsentType": "",            （必填） 值为：OpenIddictConstants.ConsentTypes
        //    "ClientSecret": "",                   （选填） ClientTypes 为 confidential 时，才需要填写
        //    "Scopes": [ ],                （必填）
        //    "GrantTypes": [ ],            （必填） 值为：OpenIddictConstants.GrantTypes
        //    "RedirectUri": "",                    （选填）默认为：ClientUri
        //    "PostLogoutRedirectUri": ""           （选填）默认为：ClientUri
        //    "Permissions":                （选填）默认：空
        //}

        var configurationSection = _configuration.GetSection("OpenIddict:Applications");
        foreach (var section in configurationSection.GetChildren())
        {
            var clientId = section["ClientId"];
            if (!clientId.IsNullOrWhiteSpace())
            {
                string clientUri = section["ClientUri"].TrimEnd('/');
                await CreateApplicationAsync(
                    name: clientId,
                    clientUri: clientUri,
                    type: section["ClientType"],
                    consentType: section["ConsentType"],
                    displayName: clientId,
                    secret: section["ClientSecret"],
                    grantTypes: section.GetSection("GrantTypes").GetChildren().Select(x => x.Value).ToList(),
                    scopes: commonScopes.Union(section.GetSection("Scopes").GetChildren().Select(x => x.Value)).Distinct().ToList(),
                    redirectUri: GetFullUri(section["RedirectUri"], clientUri),
                    postLogoutRedirectUri: GetFullUri(section["PostLogoutRedirectUri"], clientUri),
                    permissions: section.GetSection("Permissions").GetChildren().Select(x => x.Value).ToList()
                );
            }
        }
    }

    // #、客户端类型 ClientType
    // 基于客户端是否有能力与授权服务器进行安全认证的能力（即是否有能力保障其客户端凭证的保密性），OAuth定义了两种客户端类型：
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

            if (new[] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }.All(grantTypes.Contains))
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

            var buildInScopes = new[]
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
