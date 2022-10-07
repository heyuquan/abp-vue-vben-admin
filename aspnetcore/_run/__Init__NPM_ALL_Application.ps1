. "./__build-aspnetcore-common.ps1"

Write-Host ==================== Begin Init AuthServer.Host ========================
$solutionPath = $rootFolder + "/../services/SSO.AuthServer/src/SSO.AuthServer.IdentityServer"
Set-Location $solutionPath
yarn install
gulp

Write-Host End Init Mk.DemoC.HttpApi.Host ========================





Set-Location $rootFolder
pause