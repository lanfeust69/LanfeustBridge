# LanfeustBridge

Web application to manage bridge tournaments

This is mostly an experimentation around Asp.Net Core and Angular,
however it may eventually come to something useful, who knows ?

## Requirements

- [.NET 10](https://dotnet.microsoft.com)
- [nodejs](https://nodejs.org)

## Running from the command line

```bash
# Running the server
cd LanfeustBridge.Server && dotnet run

# Running backend tests
cd MovementTests && dotnet test

# Running frontend unit tests
cd lanfeustbridge.client
npm test

# Running frontend end-to-end tests
# --silent is to avoid spurious npm error message in case of failure
cd lanfeustbridge.client
npm run e2e --silent
```
