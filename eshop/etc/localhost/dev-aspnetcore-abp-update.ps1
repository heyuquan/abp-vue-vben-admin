. "./build-aspnetcore-common.ps1"

# update volo.abp.cli
dotnet tool update -g Volo.Abp.Cli

# update all solutions
$solutionPath = $rootFolder + "/../../aspnetcore/"
Set-Location $solutionPath
abp update --version 6.0.2

$solutionPath = $rootFolder + "/../../../framework/"
Set-Location $solutionPath
abp update --version 6.0.2

Set-Location $rootFolder

pause