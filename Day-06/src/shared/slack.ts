import { IncomingWebhook } from "@slack/webhook";

export async function notifyScheduleCreated(text: string) {
    // Read a url from the environment variables
    const url = process.env.SLACK_WEBHOOK_URL;
    // Initialize
    const webhook = new IncomingWebhook(url);
    await webhook.send(
        {
            text: `*${text}*' has been scheduled!`
        }
    )
}

export async function notifyScheduleHappening(text: string) {
    // Read a url from the environment variables
    const url = process.env.SLACK_WEBHOOK_URL;
    
    // Initialize
    const webhook = new IncomingWebhook(url);

    await webhook.send(
        {
            text: `You scheduled *${text}* to happen right now!`
        }
    )
}