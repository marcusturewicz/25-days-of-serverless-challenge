import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import { Database } from "../Shared/Database";

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    
    const db = new Database();
    await db.connect();

    context.res = {
        body: {
            "dishes": await db.listDishes()
        }
    }
};

export default httpTrigger;
