# Build Images

## Build Biz.API
```
cd ../src
docker build -t  al/biz-api:0.2.1 -f ./Services/Biz.API/Dockerfile .
```

##  Build Identity.API
```
cd ../src
docker build -t  al/identity-api:0.2.2 -f ./Services/Identity/Identity.API/Dockerfile .
```

# Deployment
## 0. Create namespace
```
cd ../deploy
kubectl apply -f ./infra/namespace.yaml
```

## 1.Install NGINX Ingress Controller
```
# check whether the pod is ready
kubectl get pods --namespace=ingress-nginx  

helm upgrade --install ingress-nginx ingress-nginx --repo https://kubernetes.github.io/ingress-nginx  --namespace ingress-nginx --create-namespace
```

## 2. Deploy Biz.API
helm upgrade --install biz-api ./k8s/helm/biz-api --namespace=eshop --set image.tag="0.2.1" --dry-run

## 3. Deploy Identity.API
helm upgrade --install identity-api ./k8s/helm/identity-api --namespace=eshop --set image.tag="0.2.1" --dry-run

## Verify
Open blowser to visit: http://account.eshop.local/ 
Note: better with Edge, as Chrome will froce to redirect to HTTPS


# Troubleshooting

