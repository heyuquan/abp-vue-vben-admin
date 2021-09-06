
Write-Host ==================== Begin Init AuthServer.Host ========================
$x = Split-Path -Parent $MyInvocation.MyCommand.Definition
cd $x
cd ../microservices/SSO.AuthServer/src/SSO.AuthServer.HttpApi.Host
dotnet ef database update -p ../SSO.AuthServer.EntityFrameworkCore

Write-Host init AuthServer.Host SendData
$x = Split-Path -Parent $MyInvocation.MyCommand.Definition
cd $x
cd ../microservices/SSO.AuthServer/src/SSO.AuthServer.DbMigrator
dotnet run

Write-Host End Init Mk.DemoC.HttpApi.Host ========================



Write-Host ==================== Begin Mk.DemoC.HttpApi.Host ========================
$x = Split-Path -Parent $MyInvocation.MyCommand.Definition
cd $x
cd ../microservices/Mk.DemoC/src/Mk.DemoC.HttpApi.Host
dotnet ef database update -p ./

Write-Host End Mk.DemoC.HttpApi.Host ========================



pause