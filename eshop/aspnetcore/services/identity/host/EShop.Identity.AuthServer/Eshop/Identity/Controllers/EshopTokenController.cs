using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Controllers;

namespace EShop.Identity.Controllers;

/// <summary>
/// 重写volo.TokenController，认证成功后处理程序，把clientId对应的scopes全部放入token中。  这样避免申请token端还要传入scope参数
/// </summary>
public class EshopTokenController : TokenController, ITransientDependency
{
    private readonly IOpenIddictApplicationManager _applicationManager;

    public EshopTokenController(IOpenIddictApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }

    [HttpPost(Order = -1)]
    public override Task<IActionResult> HandleAsync()
    {
        return base.HandleAsync();
    }

    protected override async Task<IActionResult> SetSuccessResultAsync(OpenIddictRequest request,
        IdentityUser user)
    {
        var principal = await SignInManager.CreateUserPrincipalAsync(user);
        var scopes = await GetClientAllScopesAsync(request);
        principal.SetScopes(scopes);
        principal.SetResources(await GetResourcesAsync(scopes));

        await SetClaimsDestinationsAsync(principal);

        await IdentitySecurityLogManager.SaveAsync(
            new IdentitySecurityLogContext
            {
                Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
                Action = OpenIddictSecurityLogActionConsts.LoginSucceeded,
                UserName = request.Username,
                ClientId = request.ClientId
            }
        );

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    protected override async Task<IActionResult> HandleClientCredentialsAsync(OpenIddictRequest request)
    {
        // Note: the client credentials are automatically validated by OpenIddict:
        // if client_id or client_secret are invalid, this action won't be invoked.
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId);
        if (application == null)
        {
            throw new InvalidOperationException(L["TheApplicationDetailsCannotBeFound"]);
        }

        // Create a new ClaimsIdentity containing the claims that
        // will be used to create an id_token, a token or a code.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.PreferredUsername, OpenIddictConstants.Claims.Role);

        // The Subject and PreferredUsername will be removed by <see cref="RemoveClaimsFromClientCredentialsGrantType"/>.
        // Use the client_id as the subject identifier.
        identity.AddClaim(OpenIddictConstants.Claims.Subject, await ApplicationManager.GetClientIdAsync(application));
        identity.AddClaim(OpenIddictConstants.Claims.PreferredUsername,
            await ApplicationManager.GetDisplayNameAsync(application));

        // Note: In the original OAuth 2.0 specification, the client credentials grant
        // doesn't return an identity token, which is an OpenID Connect concept.
        //
        // As a non-standardized extension, OpenIddict allows returning an id_token
        // to convey information about the client application when the "openid" scope
        // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
        // scope is not explicitly set, no identity token is returned to the client application.

        // Set the list of scopes granted to the client application in access_token.
        var principal = new ClaimsPrincipal(identity);
        var scopes = await GetClientAllScopesAsync(request);
        principal.SetScopes(scopes);
        principal.SetResources(await GetResourcesAsync(scopes));

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(GetDestinations(claim));
        }

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<ImmutableArray<string>> GetClientAllScopesAsync(OpenIddictRequest request)
    {
        var scopes = request.GetScopes();
        if (!scopes.Any() && request.ClientId != null)
        {
            var client = await _applicationManager.FindByClientIdAsync(request.ClientId);
            scopes = (await _applicationManager.GetPermissionsAsync(client)).Where(o => o.StartsWith("scp:"))
                .Select(o => o.Replace("scp:", "")).ToImmutableArray();
        }

        return scopes;
    }
}