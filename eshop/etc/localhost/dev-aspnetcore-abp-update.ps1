. "./build-aspnetcore-common.ps1"

# update volo.abp.cli
dotnet tool update -g Volo.Abp.Cli

# update all solutions
$solutionPath = $rootFolder + "/../../aspnetcore/"
Set-Location $solutionPath
abp update --version 7.1.0

$solutionPath = $rootFolder + "/../../../framework/"
Set-Location $solutionPath
abp update --version 7.1.0

Set-Location $rootFolder

pause

#、 如果升级net版本，则还需要修改
#、  1、csproj文件中的 <TargetFramework>net7.0</TargetFramework>
#、  2、etc\localhost\aspnetcore\common-file\sln\global.json  指向已安装的net sdk   （cmd  dotnet --info）

#、修改etc\localhost\aspnetcore\common-file\sln\Directory.Build.props  中<MicrosoftPackageVersion>

#、迁移数据库
