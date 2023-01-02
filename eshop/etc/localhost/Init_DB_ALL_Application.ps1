. "./build-aspnetcore-common.ps1"

# 先安装  dotnet tool install --global dotnet-ef

Write-Host ==================== Begin Init AuthServer.Host ========================
$project = $projectArray | Where {$_.Name -eq "auth-server" }
Set-Location $project.Path
dotnet ef database update -p ./

Write-Host End Leopard.BackendAdmin.HttpApi.Host ========================
$solutionPath = $rootFolder + "/../../aspnetcore/services/administration"
Set-Location $solutionPath
dotnet ef database update --project ./src/EShop.Administration.EntityFrameworkCore/EShop.Administration.EntityFrameworkCore.csproj --startup-project ./host/EShop.Administration.HttpApi.Host/EShop.Administration.HttpApi.Host.csproj




Set-Location $rootFolder

pause