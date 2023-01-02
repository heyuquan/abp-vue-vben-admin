. "./build-aspnetcore-common.ps1"

Write-Host ==================== Begin Init AuthServer.Host ========================
$project = $projectArray | Where {$_.Name -eq "identityserver-web" }
Set-Location $project.Path
abp install-libs





Set-Location $rootFolder

pause