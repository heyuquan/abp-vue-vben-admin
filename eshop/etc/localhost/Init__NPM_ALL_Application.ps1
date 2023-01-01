. "./build-aspnetcore-common.ps1"

Write-Host ==================== Begin Init AuthServer.Host ========================
$solutionPath = $rootFolder + "/../../aspnetcore/services/auth-server/host/EShop.AuthServer/EShop.AuthServer.IdentityServer"
Set-Location $solutionPath
abp install-libs





Set-Location $rootFolder
pause