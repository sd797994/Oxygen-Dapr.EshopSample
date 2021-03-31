kubectl delete -f Deploy.yaml
docker system prune -f
cd ../../
docker build . -t accountservice:release --target accountservice --no-cache
docker build . -t goodsservice:release --target goodsservice
docker build . -t imageservice:release --target imageservice
docker build . -t jobservice:release --target jobservice
docker build . -t publicservice:release --target publicservice
docker build . -t tradeservice:release --target tradeservice
cd Public/WebPage/Admin
docker build . -t adminfrontend:latest --no-cache
cd ../www 
docker build . -t mobilefrontend:latest --no-cache