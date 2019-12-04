import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import { Dish } from "../Shared/Dish";
import { Database } from "../Shared/Database";
import { UpdateDish } from "../Shared/UpdateDish";

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    
    const updateDish = req.body as UpdateDish;

    if (!updateDish.dishNew.dishName || !updateDish.dishNew.friendName
        || !updateDish.dishOld.dishName || !updateDish.dishOld.friendName)
    {
        context.res = {
            body: {
                "message": "Body is not in the correct format"
            },
            status: 400
        }
        return;
    }

    const db = new Database();
    await db.connect();

    await db.updateDish(updateDish);

    context.res = {
        body: {
            "message": `Dish was updated`  
        }
    }
};

export default httpTrigger;
