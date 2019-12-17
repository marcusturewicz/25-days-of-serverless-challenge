var Twitter = require('twitter');

module.exports = async function (context, eventHubMessages) {
    context.log(`JavaScript eventhub trigger function called for message array ${eventHubMessages}`);
    
    eventHubMessages.forEach((message, index) => {
        if (message.temperature > 31) {
            var client = new Twitter({
                consumer_key: process.env.TWITTER_CONSUMER_KEY,
                consumer_secret: process.env.TWITTER_CONSUMER_SECRET,
                access_token_key: process.env.TWITTER_ACCESS_TOKEN_KEY,
                access_token_secret: process.env.TWITTER_ACCESS_TOKEN_SECRET
            });
            client.post('statuses/update', { status: 'Temprature is ' + Math.round(message.temperature) + ': ready for the beach?' })
                .then(function (tweet) {
                    console.log(tweet);
                })
                .catch(function (error) {
                    throw error;
                })
        }
    });
};