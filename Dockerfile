FROM microsoft/dotnet:latest

RUN apt-get update
RUN apt-get install -y build-essential nodejs nodejs-legacy

WORKDIR /app

COPY LanfeustBridge.csproj .
RUN ["dotnet", "restore"]

COPY . /app
RUN ["dotnet", "publish"]

EXPOSE 5000/tcp

ENTRYPOINT ["dotnet", "run", "--server.urls", "http://0.0.0.0:5000"]
