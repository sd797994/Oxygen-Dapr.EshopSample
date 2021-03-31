PowerShell -Command "kubectl delete po $(kubectl get po -n infrastructure|findstr elasticsearch|%%{$_.split()[0]}) -n infrastructure" 
PowerShell -Command "kubectl delete po $(kubectl get po -n infrastructure|findstr redis|%%{$_.split()[0]}) -n infrastructure"
PowerShell -Command "kubectl delete po $(kubectl get po -n infrastructure|findstr postgres|%%{$_.split()[0]}) -n infrastructure"
kubectl delete -f Deploy.yaml
kubectl apply -f Deploy.yaml
docker system prune -f