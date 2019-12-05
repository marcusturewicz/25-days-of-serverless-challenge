# Day 05 of [25 days of serverless](https://www.25daysofserverless.com)

[SMART APPS: Naught Or Nice](https://25daysofserverless.com/calendar/5)

Node.js Azure Function with Congitive Service (Translator Text + Text Analytics) using respective Node.js SDK's.

Live app here: https://naughtyornice.azurewebsites.net/api/NaughtyOrNice?code=ovNdlP6KpcBnpMTxB92tr9HEY3AJvEG2wVp4j5El2dR9gPIlXBn2ZA==

## Prerequisites
1. Create Translator Text service in Azure, [docs here](https://docs.microsoft.com/en-us/azure/cognitive-services/translator/quickstart-translate?pivots=programming-language-nodejs).
2. Create Text Analysis service in Azure, [docs here](https://docs.microsoft.com/en-gb/azure/cognitive-services/text-analytics/quickstarts/text-analytics-sdk?pivots=programming-language-nodejs)

## To run locally
1. Install Node.js, VS Code and Azure Functions Tools extension
2. Create a file `src/local.settings.json` that looks like this:
    ```json
    {
        "IsEncrypted": false,
        "Values": {
            "AzureWebJobsStorage": "",
            "FUNCTIONS_WORKER_RUNTIME": "node",
            "TRANSLATOR_TEXT_SUBSCRIPTION_KEY": "TRANSLATOR_TEXT_SUBSCRIPTION_KEY_VALUE",
            "TRANSLATOR_TEXT_ENDPOINT": "TRANSLATOR_TEXT_ENDPOINT_VALUE",
            "TEXT_ANALYTICS_SUBSCRIPTION_KEY": "TEXT_ANALYTICS_SUBSCRIPTION_KEY_VALUE",
            "TEXT_ANALYTICS_ENDPOINT": "TEXT_ANALYTICS_ENDPOINT_VALUE"
        }
    }
    ```
    where the values are the keys from your services in Azure.
2. Press F5
3. Open Postman
4. Set method as POST, put url as http://localhost:7071/api/naughtyornice and copy+paste into the body the `sample-input.json`.
5. Hit 'Send'
6. Observe sample output below.

## To run in Azure
1. Right click in VS Code left menu and click `Deploy to Function App...` 
2. Go to portal and add the following keys to the function app's APP SETTINGS, [docs here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-use-azure-function-app-settings).
```text
    "TRANSLATOR_TEXT_SUBSCRIPTION_KEY": "TRANSLATOR_TEXT_SUBSCRIPTION_KEY_VALUE",
    "TRANSLATOR_TEXT_ENDPOINT": "TRANSLATOR_TEXT_ENDPOINT_VALUE",
    "TEXT_ANALYTICS_SUBSCRIPTION_KEY": "TEXT_ANALYTICS_SUBSCRIPTION_KEY_VALUE",
    "TEXT_ANALYTICS_ENDPOINT": "TEXT_ANALYTICS_ENDPOINT_VALUE"
```

## Output from function
```json
[
  {
    "who": "Adam",
    "message_translated": "my little brother is so annoying and stupid",
    "sentiment": 0.0031304657459259033,
    "result": "naughty"
  },
  {
    "who": "Adam",
    "message_translated": "I really like the bike I got for a present",
    "sentiment": 0.83194559812545776,
    "result": "nice"
  },
  {
    "who": "Adam",
    "message_translated": "The food is really bad at home, I'd rather go to McDonalds",
    "sentiment": 0.030931025743484497,
    "result": "naughty"
  },
  {
    "who": "Eva",
    "message_translated": "I hate cleaning my room",
    "sentiment": 0.48057103157043457,
    "result": "naughty"
  },
  {
    "who": "Eva",
    "message_translated": "my teacher is cool",
    "sentiment": 0.54762023687362671,
    "result": "nice"
  },
  {
    "who": "Eva",
    "message_translated": "the neighbor dog is stupid and smelly",
    "sentiment": 0.56255745887756348,
    "result": "nice"
  },
  {
    "who": "jenifer",
    "message_translated": "my parents are really not good at technical things",
    "sentiment": 0.48471596837043762,
    "result": "naughty"
  },
  {
    "who": "jenifer",
    "message_translated": "my little sister is a pain in the",
    "sentiment": 0.52615559101104736,
    "result": "nice"
  },
  {
    "who": "jenifer",
    "message_translated": "can we eat something other than meatballs?",
    "sentiment": 0.54856812953948975,
    "result": "nice"
  },
  {
    "who": "sergio",
    "message_translated": "I like my football.",
    "sentiment": 0.54792875051498413,
    "result": "nice"
  },
  {
    "who": "sergio",
    "message_translated": "I wish I had a dog.",
    "sentiment": 0.50392687320709229,
    "result": "nice"
  },
  {
    "who": "sergio",
    "message_translated": "dad and Santa are similar",
    "sentiment": 0.59321731328964233,
    "result": "nice"
  },
  {
    "who": "tracy",
    "message_translated": "Dad and Santa are very similar.",
    "sentiment": 0.5,
    "result": "nice"
  },
  {
    "who": "tracy",
    "message_translated": "I hate rice.",
    "sentiment": 0.30615311861038208,
    "result": "naughty"
  },
  {
    "who": "tracy",
    "message_translated": "I like my little sister.",
    "sentiment": 0.63586056232452393,
    "result": "nice"
  }
]
```

-- Created with VS Code.

