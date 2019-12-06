import * as df from "durable-functions"

const orchestrator = df.orchestrator(function* (context) {
    const input: any = context.df.getInput()
    yield context.df.createTimer(new Date(input.date));
    return yield context.df.callActivity('sendNotification', input.text);
});

export default orchestrator;
