# Prepare

## 0.Install NGINX Ingress Controller
>https://docs.rancherdesktop.io/how-to-guides/setup-NGINX-Ingress-Controller

```
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/cloud/1.23/deploy.yaml
# check whether the pod is ready
kubectl get pods --namespace=ingress-nginx  
```

### a. Deploy a deomo to verify NGINX Ingress

- Uncheck Enable Traefik from the Kubernetes Settings page to disable Traefik. You may need to exit and restart Rancher Desktop for the change to take effect.

- Deploy nginx service & ingress
    ```
    kubectl create deployment demo --image=nginx --port=80
    kubectl expose deployment demo

    kubectl create ingress demo-localhost --class=nginx --rule="demo.localdev.me/*=demo:80"
    ```

- Add '127.0.0.1   demo.localdev.me' to hosts (Win10: C:\Windows\System32\drivers\etc)

- Forward a local port to the ingress controller
    ```
    kubectl port-forward --namespace=ingress-nginx service/ingress-nginx-controller 8080:80
    ```

- If you access <http://demo.localdev.me:8080/>, you should see NGINX Welcome page.

## 1. Create namespace
```
kubectl apply -f ./infra/namespace.yaml
```

# Deploy Sevices

## 0. Deploy DB
Refer to ./SQLServer Commands

## 1. Deploy Identity.API
- Deploy the service
    ```
    kubectl get pods --all-namespaces -- PreCheck DB services
    kubectl apply -f ./IdentityAPI/file-store-pv-claim.yaml
    kubectl apply -f ./IdentityAPI/identity-api-config-map.yaml
    kubectl apply -f ./IdentityAPI/identity-api-secret-test.yaml

    kubectl apply -f ./IdentityAPI/identity-api-deployment.yaml
    ```
- Apply the ingress
    ```
    kubectl apply -f ./IdentityAPI/identity-ingress.yaml
    ```
- Browser http://account.eshop.local:8080/ 
(*Note: ensure the domain add to local hosts file*)

-  ***Roll out an upgrade***
    ```
    kubectl edit deployment identity-api -- After making the change, save and close the file
    ```

# Reference
```
kubectl get deploy
kubectl delete deploy <deployment name>

kubectl get pods
kubectl cluster-info
kubectl config view
kubectl get events
kubectl logs <pod-name>

kubectl get services

kubectl scale deployment hello-dotnet --replicas=4
kubectl delete pod hello-dotnet-714049816-g4azy

-- PVC
kubectl get pvc
kubectl delete pvc --all / pvc-name
```

## Cleanup

```
kubectl delete service,deployment identity-api
```
