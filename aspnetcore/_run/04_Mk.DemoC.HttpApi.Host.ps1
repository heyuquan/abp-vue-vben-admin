$x = Split-Path -Parent $MyInvocation.MyCommand.Definition
cd  $x
cd ../microservices/Mk.DemoC/src/Mk.DemoC.HttpApi.Host
$path=Get-Location
$launchSettings = (Get-Content "Properties/launchSettings.json") | ConvertFrom-Json
$host.UI.RawUI.WindowTitle = $path.Path.Substring($path.Path.LastIndexOf("\")+1)+"  Address:  "+ $launchSettings.iisSettings.iisExpress.applicationUrl
dotnet run
