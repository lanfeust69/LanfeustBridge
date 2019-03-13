FROM microsoft/dotnet:latest

RUN apt-get update
RUN apt-get install -y build-essential
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - && apt-get install nodejs

WORKDIR /app

COPY LanfeustBridge.csproj .
RUN ["dotnet", "restore"]

COPY . /app
RUN ["dotnet", "publish"]

EXPOSE 5000/tcp 5001/tcp

ENV ASPNETCORE_URLS "http://0.0.0.0:5000;https://0.0.0.0:5001"
ENTRYPOINT ["dotnet", "run", "--no-launch-profile"]
