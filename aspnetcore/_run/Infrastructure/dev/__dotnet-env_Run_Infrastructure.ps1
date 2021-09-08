cd ../../../environment/dotnet-env
docker-compose -f docker-compose.yml -f docker-compose.override.dev.yml up -d
cd ../../_run/Infrastructure/dev