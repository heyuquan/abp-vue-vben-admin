. "./build-aspnetcore-common.ps1"

# update volo.abp.cli
dotnet tool update -g Volo.Abp.Cli

# update all solutions
$solutionPath = $rootFolder + "/../../aspnetcore/"
Set-Location $solutionPath
abp update

$solutionPath = $rootFolder + "/../../../framework/leopard/"
Set-Location $solutionPath
abp update

Set-Location $rootFolder