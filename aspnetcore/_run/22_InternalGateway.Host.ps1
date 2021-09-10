$x = Split-Path -Parent $MyInvocation.MyCommand.Definition
cd  $x
cd ../gateways/InternalGateway.Host
$path=Get-Location
$launchSettings = (Get-Content "Properties/launchSettings.json") | ConvertFrom-Json
$host.UI.RawUI.WindowTitle = $path.Path.Substring($path.Path.LastIndexOf("\")+1)+"  Address:  "+ $launchSettings.iisSettings.iisExpress.applicationUrl
dotnet watch run
