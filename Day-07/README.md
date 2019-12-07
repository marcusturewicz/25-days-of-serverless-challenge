# Day 07 of [25 days of serverless](https://www.25daysofserverless.com)

[API ENDPOINT - PICTURE CHALLENGE](https://25daysofserverless.com/calendar/7)

C# Azure Function with Unsplash image search using open source [Unsplash SDK](https://github.com/rootasjey/unsplasharp).

## Prerequisites
1. Register and create a Unsplash app, [docs here](https://unsplash.com/developers)

## To run locally
1. Install VS Code, C# extension and Azure Functions Tools extension
2. Create a file `src/local.settings.json` that looks like this:
```json
    {
        "IsEncrypted": false,
        "Values": {
            "AzureWebJobsStorage": "",
            "FUNCTIONS_WORKER_RUNTIME": "dotnet",
            "UNSPLASH_ACCESS_KEY": "<<YOUR_UNSPLASH_ACCESS_KEY>>",
            "UNSPLASH_SECRET_KEY": "<<YOUR_UNSPLASH_SECRET_KEY>>"        
        }
    }
```
2. Press F5
3. Open browser and go to page http://localhost:7071/api/imagesearch?s=[search]
    where [search] is your search string e.g. http://localhost:7071/api/imagesearch?s=soccer`
4. File will be downloaded, see example of image below:
![soccer](soccer.png)


## To run in Azure
1. Right click in VS Code left menu and click `Deploy to Function App...` 
2. Go to portal and add the following keys to the function app's APP SETTINGS, [docs here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings).
```text
            "UNSPLASH_ACCESS_KEY": "<<YOUR_UNSPLASH_ACCESS_KEY>>"
            "UNSPLASH_SECRET_KEY": "<<YOUR_UNSPLASH_SECRET_KEY>>" 
```

-- Created with VS Code.
