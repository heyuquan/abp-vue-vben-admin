cd ../../../environment/dotnet-env
docker-compose -f docker-compose.yml -f docker-compose.override.local.yml down
cd ../../_run/Infrastructure/local