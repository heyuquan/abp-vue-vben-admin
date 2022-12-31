. "./__build-aspnetcore-common.ps1"

# 先安装  dotnet tool install --global dotnet-ef

Write-Host ==================== Begin Init AuthServer.Host ========================
$solutionPath = $rootFolder + "/../services/SSO.AuthServer/SSO.AuthServer.HttpApi.Host"
Set-Location $solutionPath
dotnet ef database update -p ./

Write-Host End Leopard.BackendAdmin.HttpApi.Host ========================
$solutionPath = $rootFolder + "/../services/BackendAdmin"
Set-Location $solutionPath
dotnet ef database update --project ./Leopard.BackendAdmin.EntityFrameworkCore/Leopard.BackendAdmin.EntityFrameworkCore.csproj --startup-project ./Leopard.BackendAdmin.HttpApi.Host/Leopard.BackendAdmin.HttpApi.Host.csproj


Write-Host ==================== Begin Mk.DemoC.HttpApi.Host ========================
# $solutionPath = $rootFolder + "/../services/Mk.DemoC/src/Mk.DemoC.HttpApi.Host"
# Set-Location $solutionPath
# dotnet ef database update -p ./

Write-Host End Mk.DemoC.HttpApi.Host ========================


Set-Location $rootFolder
pause