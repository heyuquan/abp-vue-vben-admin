. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($project in $projectArray) {  
    Set-Location $project.Path
    Write-Host $project.Name $project.Path -ForegroundColor green
    dotnet build --no-cache
}

Set-Location $rootFolder

pause