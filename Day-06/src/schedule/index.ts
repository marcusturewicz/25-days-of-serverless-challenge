import * as df from "durable-functions"
import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import { notifyScheduleCreated } from "../shared/slack"
var chrono = require('chrono-node');
var moment = require('moment-timezone');

const httpStart: AzureFunction = async function (context: Context, req: HttpRequest): Promise<any> {

    const client = df.getClient(context);
    var date = chrono.parseDate(req.body.text, moment().tz("Melbourne/Australia"));
    var input = {
        text: req.body.text,
        date: date
    }
    const instanceId = await client.startNew(req.params.functionName, undefined, input);

    context.log(`Started orchestration with ID = '${instanceId}'.`);

    // Notify that schedule has been created
    await notifyScheduleCreated(req.body.text);

    return client.createCheckStatusResponse(context.bindingData.req, instanceId);
};

export default httpStart;
