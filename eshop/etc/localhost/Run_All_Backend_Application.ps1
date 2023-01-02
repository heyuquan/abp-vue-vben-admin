start powershell .\services\01_EShop.AuthServer.HttpApi.Host.ps1
start powershell .\services\02_EShop.AuthServer.IdentityServer.Web.ps1

# 应用程序
start powershell .\services\06_EShop.Administration.HttpApi.Host.ps1

# gateway
start powershell .\services\21_BackendAdminAppGateway.Host.ps1
#start powershell .\services\22_InternalGateway.Host.ps1
