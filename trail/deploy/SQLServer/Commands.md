# 1. Create Scecert

kubectl create secret generic mssql --from-literal=SA_PASSWORD="MyP@ssw0rd"

# 2. Create Persistent Volume

 kubectl apply -f pv-claim.yaml

# 3. Deploy the service

kubectl apply -f sqldeployment.yaml
