$x = Split-Path -Parent $MyInvocation.MyCommand.Definition
cd  $x
cd ../microservices/SSO.AuthServer/src/SSO.AuthServer.DbMigrator
$path=Get-Location
$launchSettings = (Get-Content "Properties/launchSettings.json") | ConvertFrom-Json
$host.UI.RawUI.WindowTitle = $path.Path.Substring($path.Path.LastIndexOf("\")+1)+"  Address:  "+ $launchSettings.iisSettings.iisExpress.applicationUrl
dotnet run
