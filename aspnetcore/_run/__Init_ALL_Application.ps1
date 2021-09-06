cd ..

Write-Host ==================== Begin Init AuthServer.Host ========================
cd ./microservices/SSO.AuthServer/src/SSO.AuthServer.IdentityServer
yarn install
gulp

cd ../../../../

Write-Host try update db
cd ./microservices/SSO.AuthServer/src/SSO.AuthServer.HttpApi.Host
dotnet ef database update -p ../SSO.AuthServer.EntityFrameworkCore

cd ../../../../
dir
Write-Host End Init Mk.DemoC.HttpApi.Host ========================


Write-Host ==================== Begin Init AuthServer.Host ========================

Write-Host try update db
cd ./microservices/Mk.DemoC/src/Mk.DemoC.HttpApi.Host
dotnet ef database update -p ./Mk.DemoC.HttpApi.Host

cd ../../../../
Write-Host End Mk.DemoC.HttpApi.Host ========================


Write-Host Enter any key to close
pause