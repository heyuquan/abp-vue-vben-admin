cd ..
. "./build-aspnetcore-common.ps1"

$project = $projectArray | Where {$_.Name -eq "identityserver-web" }
Set-Location $project.Path
$path=Get-Location
$launchSettings = (Get-Content "Properties/launchSettings.json") | ConvertFrom-Json
$host.UI.RawUI.WindowTitle = $project.Name +"  Address:  "+ $launchSettings.iisSettings.iisExpress.applicationUrl
dotnet watch run
