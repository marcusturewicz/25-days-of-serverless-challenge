import { AzureFunction, Context } from "@azure/functions"
import { notifyScheduleHappening } from "../shared/slack";

const activityFunction: AzureFunction = async function (context: Context): Promise<string> {

    // Notify that schedule has been created
    await notifyScheduleHappening(context.bindingData.data);

    return 'done';
};

export default activityFunction;
