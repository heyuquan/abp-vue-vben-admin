. "./__build-aspnetcore-common.ps1"

$solutionPath = $rootFolder + "/../microservices/SSO.AuthServer/SSO.AuthServer.IdentityServer"
Set-Location $solutionPath
$path=Get-Location
$launchSettings = (Get-Content "Properties/launchSettings.json") | ConvertFrom-Json
$host.UI.RawUI.WindowTitle = $path.Path.Substring($path.Path.LastIndexOf("\")+1)+"  Address:  "+ $launchSettings.iisSettings.iisExpress.applicationUrl
dotnet run
