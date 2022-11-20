. "./__build-aspnetcore-common.ps1"

Write-Host ==================== Begin Init AuthServer.Host ========================
$solutionPath = $rootFolder + "/../services/SSO.AuthServer/SSO.AuthServer.IdentityServer"
Set-Location $solutionPath
abp install-libs

Write-Host End Init Mk.DemoC.HttpApi.Host ========================





Set-Location $rootFolder
pause