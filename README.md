# LanfeustBridge

Web application to manage bridge tournaments

This is mostly an experimentation around Asp.Net Core and Angular,
however it may eventually come to something useful, who knows ?

## Requirements

- [.Net Core 2.1](https://www.microsoft.com/net/core)
- [nodejs](https://nodejs.org)

## Running from the command line

```
# Running the server
dotnet run

# Running backend tests
dotnet test

# Running frontend unit tests
cd ClientApp
npm test

# Running frontend end-to-end tests
# - if not in development mode (where data is only kept in memory), remove old data : rm data/*
# - run the server in another terminal : dotnet run
# - finally run the test, --silent is to avoid spurious npm error message in case of failure
cd ClientApp
npm run e2e --silent
```
