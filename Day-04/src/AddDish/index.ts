import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import { Dish } from "../Shared/Dish";
import { Database } from "../Shared/Database";

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    
    const dish = req.body as Dish;

    if (!dish.dishName || !dish.friendName)
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

    const res = await db.addDish(dish);

    if (!res)
    {
        context.res = {
            body: {
                "message": "Dish already exists"
            },
            status: 409
        }
        return;
    }

    context.res = {
        body: {
            "message": `Dish added with id ${await db.addDish(dish)}`  
        }
    }
};

export default httpTrigger;
