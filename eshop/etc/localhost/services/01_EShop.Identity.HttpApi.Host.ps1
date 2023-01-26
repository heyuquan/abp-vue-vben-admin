cd ..
. "./build-aspnetcore-common.ps1"

$project = $projectArray | Where {$_.Name -eq "identity" }
Set-Location $project.RunPath
$launchSettings = (Get-Content "Properties/launchSettings.json") | ConvertFrom-Json
$host.UI.RawUI.WindowTitle = $project.Name +"  Address:  "+ $launchSettings.iisSettings.iisExpress.applicationUrl
dotnet watch run


