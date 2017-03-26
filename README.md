# LanfeustBridge

Web application to manage bridge tournaments

This is mostly an experimentation around Asp.Net Core and AngularJs 2,
however it may eventually come to something useful, who knows ?


## Requirements

- [.Net Core 1.1](https://www.microsoft.com/net/core)
- [nodejs](https://nodejs.org)

## Running from the command line

```
# Setup the environment
npm install
./node_modules/.bin/webpack --config webpack.config.vendor.js
./node_modules/.bin/webpack --config webpack.config.js
dotnet restore
export ASPNETCORE_ENVIRONMENT=Development # or equivalent, depending on platform/shell

# Running the server
dotnet run

# Running backend tests
dotnet test

# Running frontend unit tests
./node_modules/.bin/karma start ./ClientApp/test/karma.conf.js

# Running frontend end-to-end tests
# run the server in another terminal, then :
./node_modules/.bin/webdriver-manager update
./node_modules/.bin/protractor
```
