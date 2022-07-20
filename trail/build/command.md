# Build Images

## Build Biz.API
```
cd src
docker build -t  al/biz-api:0.2.0 -f ./Services/Biz.API/Dockerfile .
```

##  Build Identity.API
```
cd src
docker build -t  al/identity-api:0.1.3 -f ./Services/Identity/Identity.API/Dockerfile .
```

# Reference
https://code.visualstudio.com/docs/containers/quickstart-aspnet-core