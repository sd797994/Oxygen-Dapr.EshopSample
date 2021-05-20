docker rmi oauthservice:release tradeservice:release publicservice:release jobservice:release imageservice:release goodsservice:release accountservice:release
docker system prune -f
cd ../../
docker build . -t accountservice:release --target accountservice 
docker build . -t goodsservice:release --target goodsservice
docker build . -t imageservice:release --target imageservice
docker build . -t jobservice:release --target jobservice
docker build . -t publicservice:release --target publicservice
docker build . -t tradeservice:release --target tradeservice
docker build . -t oauthservice:release --target oauthservice
