FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

WORKDIR /source

COPY LanfeustBridge.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -o /app -v n --no-restore -p:SkipRunWebpack=true

FROM node:lts-alpine AS build-client

WORKDIR /app

COPY ClientApp/package*.json .
RUN npm install
COPY ClientApp/ .
RUN npm run build -- --configuration=production

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

WORKDIR /app

COPY --from=build /app ./
COPY --from=build-client /app/dist ./ClientApp/dist

EXPOSE 5000/tcp 5001/tcp

ENV ASPNETCORE_URLS "http://0.0.0.0:5000;https://0.0.0.0:5001"
ENTRYPOINT dotnet LanfeustBridge.dll
