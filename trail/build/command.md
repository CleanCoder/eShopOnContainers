# Build Images

## Build Biz.API
```
cd src
docker build -t  al/biz-api:0.2.0 -f ./Services/Biz.API/Dockerfile .
```

##  Build Identity.API
```
cd src
docker build -t  al/identity-api:0.0.1 -f ./Services/Identity/Identity.API/Dockerfile .
```

# Helm Package
> https://docs.microsoft.com/en-us/azure/container-registry/container-registry-helm-repos

```
cd deploy
helm package ./k8s/helm/biz-api -d ./helm-local/
helm package ./k8s/helm/identity-api -d ./helm-local/

# GO to Azure console to get identity
helm registry login $ACR_NAME.azurecr.io --username $USER_NAME  --password $PASSWORD  
helm push ./helm-local/biz-api-0.2.0.tgz oci://$ACR_NAME.azurecr.io/helm
```
## Helm Install
```
helm upgrade --install biz-api oci://$ACR_NAME.azurecr.io/helm/biz-api --version 0.2.1 --set image.tag="0.2.1" --namespace eshop 

# helm list --all-namespaces
# helm status biz-api -n eshop
# helm get manifest biz-api
# helm uninstall biz-api
# helm pull oci://$ACR_NAME.azurecr.io/helm/biz-api --version 0.2.1
```
# Reference
https://code.visualstudio.com/docs/containers/quickstart-aspnet-core