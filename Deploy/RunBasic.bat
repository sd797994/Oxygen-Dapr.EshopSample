cd GateWayConf
docker build . -t apigateway:latest
cd ../
kubectl create ns infrastructure
kubectl create ns dapreshop
kubectl delete -f Basic.yaml
kubectl apply -f Basic.yaml