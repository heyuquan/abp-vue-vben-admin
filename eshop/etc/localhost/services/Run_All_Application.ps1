start powershell .\01_EShop.Identity.HttpApi.Host.ps1
start powershell .\02_EShop.Identity.AuthServer.ps1

# 应用程序
start powershell .\06_EShop.Administration.HttpApi.Host.ps1

# gateway
start powershell .\21_BackendAdminAppGateway.Host.ps1
start powershell .\22_InternalGateway.Host.ps1
start powershell .\23_PublicWebSiteGateway.Host.ps1