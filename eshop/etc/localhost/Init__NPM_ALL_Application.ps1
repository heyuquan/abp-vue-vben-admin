. "./build-aspnetcore-common.ps1"

Write-Host ==================== Begin Init EShop.Identity.AuthServer ========================
$project = $projectArray | Where {$_.Name -eq "identity-auth-server" }
Set-Location $project.Path
abp install-libs





Set-Location $rootFolder

pause