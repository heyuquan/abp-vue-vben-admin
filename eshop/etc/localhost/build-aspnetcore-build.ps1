. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($project in $projectArray) {  
    Set-Location $project.Path
    dotnet build --no-cache
}

Set-Location $rootFolder