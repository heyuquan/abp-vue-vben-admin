. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($project in $projectArray) {  
    Write-Host "begin build 【$($project.Name)】 path:$($project.Path)" -ForegroundColor yellow
    if (Test-Path $project.Path) {
        Set-Location $project.Path        
        dotnet build --no-cache
        Write-Host "success build $($project.Name)" -ForegroundColor green
    } else {
        Write-Host "can not find path:$($project.Path)" -ForegroundColor red
    }
    Write-Host "" # 换行
}

Set-Location $rootFolder

pause