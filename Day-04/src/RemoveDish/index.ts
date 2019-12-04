import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import { Dish } from "../Shared/Dish";
import { Database } from "../Shared/Database";

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    
    const dish = req.body as Dish;

    if (!dish)
    {
        context.res = {
            body: {
                "message": "Dish is not in the correct format"
            },
            status: 400
        }
        return;
    }

    const db = new Database();
    await db.connect();

    await db.removeDish(dish);

    context.res = {
        body: {
            "message": `Dish deleted`
        }
    }
};

export default httpTrigger;
