start powershell .\01_SSO.AuthServer.HttpApi.Host.ps1
start powershell .\02_SSO.AuthServer.IdentityServer.ps1

# 应用程序
start powershell .\03_Mk.DemoB.HttpApi.Host.ps1
start powershell .\04_Mk.DemoC.HttpApi.Host.ps1

# gateway
start powershell .\21_BackendAdminAppGateway.Host.ps1
start powershell .\22_InternalGateway.Host.ps1
start powershell .\23_PublicWebSiteGateway.Host.ps1