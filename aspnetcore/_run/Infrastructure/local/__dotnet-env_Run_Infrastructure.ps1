cd ../../../environment/dotnet-env
docker-compose -f docker-compose.yml -f docker-compose.override.local.yml up -d
cd ../../_run/Infrastructure/local