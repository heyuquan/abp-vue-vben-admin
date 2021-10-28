. "./__build-aspnetcore-common.ps1"

Write-Host ==================== Begin Init AuthServer.Host ========================
$solutionPath = $rootFolder + "/../microservices/SSO.AuthServer/src/SSO.AuthServer.HttpApi.Host"
Set-Location $solutionPath
dotnet ef database update -p ./

Write-Host End Init AuthServer.Host ========================



Write-Host ==================== Begin Mk.DemoC.HttpApi.Host ========================
$solutionPath = $rootFolder + "/../microservices/Mk.DemoC/src/Mk.DemoC.HttpApi.Host"
Set-Location $solutionPath
dotnet ef database update -p ./

Write-Host End Mk.DemoC.HttpApi.Host ========================


Set-Location $rootFolder
pause