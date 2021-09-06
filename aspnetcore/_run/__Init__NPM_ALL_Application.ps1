cd ..

Write-Host ==================== Begin Init AuthServer.Host ========================
cd ./microservices/SSO.AuthServer/src/SSO.AuthServer.IdentityServer
yarn install
gulp

cd ../../../../
Write-Host End Init Mk.DemoC.HttpApi.Host ========================






pause