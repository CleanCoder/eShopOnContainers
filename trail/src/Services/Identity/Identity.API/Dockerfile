#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY "eShop.sln" "eShop.sln"

COPY "Common/API.Common/API.Common.csproj" "Common/API.Common/API.Common.csproj"
COPY ["Services/Identity/Identity.API/Identity.API.csproj", "Services/Identity/Identity.API/"]
#RUN dotnet restore "Services/Identity/Identity.API/Identity.API.csproj"
#COPY . .
#WORKDIR "/Services/Identity/Identity.API"
#RUN dotnet build "Identity.API.csproj" -c Release -o /app/build

COPY "NuGet.config" "NuGet.config"
RUN dotnet restore "eShop.sln"

COPY . .
WORKDIR /src/Services/Identity/Identity.API
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.API.dll"]
