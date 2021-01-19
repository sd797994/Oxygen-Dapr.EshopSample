dotnet build ../../Oxygen-Dapr.EshopSample.sln
kubectl create ns dapreshop
kubectl delete -f Deploy.yaml
kubectl apply -f Deploy.yaml
kubectl get po -w -n dapreshop