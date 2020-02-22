FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

RUN apt-get update
RUN apt-get install -y build-essential
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash - && apt-get install nodejs

WORKDIR /source

COPY LanfeustBridge.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000/tcp 5001/tcp

ENV ASPNETCORE_URLS "http://0.0.0.0:5000;https://0.0.0.0:5001"
ENTRYPOINT dotnet LanfeustBridge.dll
