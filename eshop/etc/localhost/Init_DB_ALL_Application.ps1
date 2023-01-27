. "./build-aspnetcore-common.ps1"

# 先安装  dotnet tool install --global dotnet-ef

foreach ($project in $projectArray) {  
    if ($project.IsMigration) {
        Write-Host "[ $($project.Name) ] begin ef database update, project-path:$($project.Path)" -ForegroundColor yellow
        Set-Location $project.Path   
        Write-Host dotnet ef database update --project $project.EfProject --startup-project $project.StartProject
        dotnet ef database update --project $project.EfProject --startup-project $project.StartProject
        Write-Host "success ef database update..." -ForegroundColor green
    }
    Write-Host "" # 换行
}

Set-Location $rootFolder

pause