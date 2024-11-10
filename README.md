# Kubernetes Bitcoin Price Tracker

## Project Overview
A Kubernetes-based microservices application that tracks Bitcoin prices and calculates rolling averages. The project consists of two services:
- Service A: Bitcoin price tracker and analyzer
- Service B: Simple web service

## Architecture
```
                                    +----------------+
                                    |    Ingress     |
                                    |    Controller  |
                                    +----------------+
                                           |
                                           |
                    +----------------------+----------------------+
                    |                                           |
            +---------------+                           +---------------+
            |  Service A    |                           |  Service B    |
            | (Bitcoin      |                           | (Web Service) |
            |  Tracker)     |                           |               |
            +---------------+                           +---------------+
```

## Prerequisites
- Minikube
- kubectl
- Docker
- .NET SDK 7.0

## Project Structure
```
K8S-Cluster-Solution/
├── Minikube/
│   └── RBAC/
│       ├── rbac.yaml
│       └── role_binding.yaml
├── Services/
│   ├── Service-A/
│   │   ├── deployment.yaml
│   │   ├── service.yaml
│   │   ├── configmap.yaml
│   │   └── script/
│   │       ├── Program.cs
│   │       ├── Dockerfile
│   │       └── ServiceA.csproj
│   ├── Service-B/
│   │   ├── deployment.yaml
│   │   ├── service.yaml
│   │   └── script/
│   │       ├── Program.cs
│   │       ├── Dockerfile
│   │       └── ServiceB.csproj
├── Ingress/
│   └── ingress.yaml
├── NetworkPolicy/
│   └── deny_service_a_to_service_b.yaml
├── setup-cluster.sh
├── setup-cluster.ps1
└── README.md
```

## Features

- Real-time Bitcoin price tracking
- 10-minute rolling average calculation
- High availability with multiple replicas
- Health monitoring (liveness and readiness probes)
- Network policy for service isolation
- Configurable through ConfigMaps
- RBAC for security

## Quick Start
### Using the automatic script
#### Linux - Ubuntu
```
#Run setup-cluster.sh
chmod +x test-deployment.sh
./test-deployment.sh
```
#### Windows
```
#Run setup-cluster.ps1
.\test-deployment.ps1
```
### Run manually
1. Start Minikube:
```
minikube start
minikube addons enable ingress
```
2. Set up Docker environment:
```
eval $(minikube docker-env) 
```
3. Build Services:
```
# Build Service A
cd Services/Service-A/script
docker build -t service-a-image .

# Build Service B
cd ../../Service-B/script
docker build -t service-b-image .
```
5. Deploy Services:
```
# Aplly ConfigMap
kubectl apply -f Services/Service-A/configmap.yaml
# Apply RBAC
kubectl apply -f RBAC/rbac.yaml
kubectl apply -f RBAC/role_binding.yaml

# Deploy services
kubectl apply -f Services/Service-A/service.yaml
kubectl apply -f Services/Service-B/service.yaml

# Deploy applications
kubectl apply -f Services/Service-A/deployment.yaml
kubectl apply -f Services/Service-B/deployment.yaml

# Apply network policies
kubectl apply -f NetworkPolicy/deny_service_a_to_service_b.yaml

# Create ingress
kubectl apply -f Ingress/ingress.yaml
```
## Testing the Services
1. kubectl get pods,svc,ingress
```
kubectl get pods,svc,ingress
minikube ip
```
2. Test endpoints
```
# Test Service A
curl http://<minikube-ip>/service-a

# Test Service B
curl http://<minikube-ip>/service-b

# Health check
curl http://<minikube-ip>/service-a/health
```

## Check status/health
Check service status
```
# Get all resources
kubectl get all

# Check pods
kubectl get pods

# Check logs
kubectl logs -l app=service-a
kubectl logs -l app=service-b
```
## Cleanup
Remove all resources
```
kubectl delete -f .
```
Stop Minikube
```
minikube stop
```
## Security Features
- RBAC configuration for service accounts
- Network policies for service isolation
- ConfigMap for sensitive configuration
- Health monitoring and automated recovery
## Production Considerations
- Increase replica count for higher availability
- Update supportability
## License
MIT License
