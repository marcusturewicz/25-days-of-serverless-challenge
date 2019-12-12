const showdown  = require('showdown');
const Octokit = require("@octokit/rest");
var redis = require("redis");
var bluebird = require("bluebird");
bluebird.promisifyAll(redis.RedisClient.prototype);
bluebird.promisifyAll(redis.Multi.prototype);

const octokit = new Octokit();
const converter = new showdown.Converter();
const redisClient = redis.createClient(process.env.RedisPort, process.env.RedisHost,
    { auth_pass: process.env.RedisKey, tls: { servername: process.env.RedisHost } });  

module.exports = async function (context, req) {
    const card = req.query.card;
    var html = "";
  
    const cardCached = await redisClient.getAsync(card);
    
    if (cardCached)
        html = cardCached;
    else {
        var gist = await octokit.gists.get({
            gist_id: card 
        });
        const markdown = gist.data.files[Object.keys(gist.data.files)[0]].content;
        html = converter.makeHtml(markdown);
        await redisClient.setAsync(card, html);
    }

    context.res = {
        status: 200,
        body: html,
        headers: {
            'Content-Type': 'text/html'
        }
    };
};