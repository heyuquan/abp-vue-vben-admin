. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($project in $projectArray) {  
    Write-Host "[ $($project.Name) ] begin build, project-path:$($project.Path)" -ForegroundColor yellow
    if (Test-Path $project.Path) {
        Set-Location $project.Path        
        # CS1573 : 类似：参数“task”在“TaskExtensions.Timeout<T>(Task<T>, int)”的 XML 注释中没有匹配的 param 标记(但其他参数有) 
        dotnet build --no-cache -nowarn:CS1573
        Write-Host "success build..." -ForegroundColor green
    } else {
        Write-Host "can not find path:$($project.Path)" -ForegroundColor red
    }
    Write-Host "" # 换行
}

Set-Location $rootFolder

pause