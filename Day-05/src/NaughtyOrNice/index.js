const CognitiveServicesCredentials = require("@azure/ms-rest-js");
const TextAnalyticsAPIClient = require("@azure/cognitiveservices-textanalytics");
const TranslatorTextAPIClient = require("@azure/cognitiveservices-translatortext");

/************************** Azure Functon ************************/

module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    // Detect language from message
    const trans_result = await translatorText(req.body);

    // Get sentiment from  language + message
    const sent_result = await sentimentAnalysis(trans_result);

    // Combine result
    const result = combineResult(req.body, trans_result, sent_result);

    // Return result
    context.res = { body: result };
};

/************************** Cognitive services ************************/

async function translatorText(body) {

    var subscription_key = process.env["TRANSLATOR_TEXT_SUBSCRIPTION_KEY"];
    var endpoint = process.env['TRANSLATOR_TEXT_ENDPOINT'];
    const creds = new CognitiveServicesCredentials.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': subscription_key } });
    const client = new TranslatorTextAPIClient.TranslatorTextClient(creds, endpoint);

    const input = body.map(x => ({ text: x.message }));

    const result = await client.translator.translate(['en'], input);

    return result.map((x, i) => ({ id: i.toString(), language: x.detectedLanguage.language, text: x.translations[0].text }));

}

async function sentimentAnalysis(docs) {
    const subscription_key = process.env['TEXT_ANALYTICS_SUBSCRIPTION_KEY'];
    const endpoint = process.env['TEXT_ANALYTICS_ENDPOINT'];

    const creds = new CognitiveServicesCredentials.ApiKeyCredentials({ inHeader: { 'Ocp-Apim-Subscription-Key': subscription_key } });
    const client = new TextAnalyticsAPIClient.TextAnalyticsClient(creds, endpoint);

    const input = {
        documents: docs
    };

    const result = await client.sentiment({
        multiLanguageBatchInput: input
    });

    return result.documents
}

function combineResult(body, trans, sent) {
    var result = [];
    for (let i = 0; i < body.length; i++) {
        result.push(
            {
                'who': body[i].who,
                'message_translated': trans[i].text,
                'sentiment': sent[i].score,
                'result': (sent[i].score >= 0.5 ? 'nice' : 'naughty')
            })
    }
    return result;
}