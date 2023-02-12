#!/bin/bash
source DeploymentVariables.txt

docker login $REGISTRY_HOST -u $REGISTRY_USER -p $REGISTRY_PW
docker build -t $REGISTRY_HOST/edap .
docker push $REGISTRY_HOST/edap

ssh -T $APP_SERVER_USER@$APP_SERVER <<EOL
    docker stop edap
    docker rm edap
    docker login $REGISTRY_HOST -u $REGISTRY_USER -p $REGISTRY_PW
    docker pull $REGISTRY_HOST/edap
    docker run --name edap -d -p 5850:80 --mount source=edap_volume,target=/app/Data $REGISTRY_HOST/edap
EOL