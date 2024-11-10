# Start Minikube
#minikube start
# Or
minikube start --cpus=2 --memory=2200 --disk-size=8g --driver=docker

# Enable required addons
minikube addons enable ingress
minikube addons enable metrics-server

# Switch to Minikube's Docker daemon
    # Windows
    & minikube -p minikube docker-env --shell powershell | Invoke-Expression

# Build Docker images
cd Services/Service-A/script
docker build -t service-a-image . 
cd ../../Service-B/script
docker build -t service-b-image .
cd ../../..

# Apply configurations
kubectl apply -f Services/Service-A/configmap.yaml
kubectl apply -f RBAC/rbac.yaml
kubectl apply -f RBAC/role_binding.yaml
kubectl apply -f Services/Service-A/service.yaml
kubectl apply -f Services/Service-B/service.yaml
kubectl apply -f Services/Service-A/deployment.yaml
kubectl apply -f Services/Service-B/deployment.yaml
kubectl apply -f NetworkPolicy/deny_service_a_to_service_b.yaml
kubectl apply -f Ingress/ingress.yaml

# Wait for pods to be ready
kubectl wait --for=condition=ready pod -l app=service-a --timeout=120s
kubectl wait --for=condition=ready pod -l app=service-b --timeout=120s

# Apply autoscaling
kubectl autoscale deployment/service-a --min=2 --max=5 --cpu-percent=80

# Final verification
kubectl get pods
kubectl get services
kubectl get ingress
kubectl get hpa

# Setup complete
echo "Setup complete!"
MINIKUBE_IP=$(minikube ip)
echo "Minikube IP: $MINIKUBE_IP"
echo "You can access the services at:"
echo "Service A: http://$MINIKUBE_IP/service-a"
echo "Service B: http://$MINIKUBE_IP/service-b"

Start-Sleep -Seconds 3

# Api requests calls
curl http://$MINIKUBE_IP/service-a
curl http://$MINIKUBE_IP/service-a/health
curl http://$MINIKUBE_IP/service-b
curl http://$MINIKUBE_IP/service-b/health