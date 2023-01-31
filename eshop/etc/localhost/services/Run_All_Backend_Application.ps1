# 使用 Start-Sleep 命令，避免项目编译共用的引用项目时，出现写入锁，写入失败而启动不了应用程序
# https://www.cnblogs.com/thescentedpath/p/StartSleep.html

# identity
start powershell .\01_EShop.Identity.HttpApi.Host.ps1 
Start-Sleep -s 5
start powershell .\02_EShop.Identity.AuthServer.ps1
Start-Sleep -s 5

# 应用程序
start powershell .\06_EShop.Administration.HttpApi.Host.ps1
Start-Sleep -s 5

# gateway
start powershell .\21_BackendAdminAppGateway.Host.ps1
#start powershell .\22_InternalGateway.Host.ps1
