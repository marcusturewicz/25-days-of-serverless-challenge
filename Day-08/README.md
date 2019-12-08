# Day 08 of [25 days of serverless](https://www.25daysofserverless.com)

[BUILD AN INCIDENT STATUS PAGE](https://25daysofserverless.com/calendar/8)

C# Azure Functions with Azure Storage of status data, SignalR service for real-time updates to an Angular app.

## Prerequisites
1. Create an Azure Storage account, [docs here](https://docs.microsoft.com/en-us/azure/storage/common/storage-quickstart-create-account?tabs=azure-portal)
2. Create a public container called `container` in Blob storage.
3. Create a file in the container called `status.json` with the following contents:
```json
    {
        "status": "closed"
    }
```
    This is where the status message is stored and updated.
2. Create an Azure SignalR service, [docs here](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-quickstart-dotnet-core)

## To run locally
1. Install Node.js, Azure Functions Tools, Angular, VS Code, C# extension and Azure Functions Tools extension
2. Create a file `src/function/local.settings.json` that looks like this:
```json
    {
        "IsEncrypted": false,
        "Values": {
            "AzureWebJobsStorage": "<<AzureStorageAccountConnectionString>>",
            "FUNCTIONS_WORKER_RUNTIME": "dotnet",
            "AzureSignalRConnectionString": "<<AzureSignalRConnectionString>>"
        },
        "Host": {
            "CORS": "*"
        }
    }
```
2. Start the function app (F5)
3. Open a terminal and go into the Angular project: `cd src/client`, and start the app: `ng serve`.
4. Open browser and go to http://localhost:4200
5. Change the status value in the json file in the storage account - the web app will be updated live with the new value.

![demo](demo.gif)

-- Created with VS Code.
