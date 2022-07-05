### 1. Create Scecert
```
kubectl create secret generic mssql --from-literal=SA_PASSWORD="MyP@ssw0rd"
```

### 2. Create Persistent Volume
```
kubectl apply -f ./SQLServer/pv-claim.yaml
```

### 3. Deploy the service
```
kubectl apply -f ./SQLServer/sqldeployment.yaml
```

### Others
- Connect from Local: tcp:127.0.0.1,64005
