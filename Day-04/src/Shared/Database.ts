import { MongoClient, Db, Collection } from "mongodb";
import { Dish } from "./Dish";
import { UpdateDish } from "./UpdateDish";

// TODO: put in appsettings
const url = 'mongodb://localhost:27017';
const dbName = "dishdb";
const collectionName = "dishes";

export class Database
{
    private client: MongoClient;
    private db: Db;
    private coll: Collection;

    public async connect()
    {
        this.client = await MongoClient.connect(url);
        this.db = this.client.db(dbName);
        this.coll = this.db.collection(collectionName);
    }

    public disconnect()
    {
        this.client.close();
    }

    public async addDish(dish: Dish) : Promise<string>
    {
        const fr = await this.coll.find(dish).toArray();
        if (!fr || fr.length == 0)
        {
            const res = await this.coll.insertOne(dish);
            return res.insertedId.toHexString();
        }
        return null;
    }    

    public async removeDish(dish: Dish)
    {
        await this.coll.remove(dish);
    }  
    
    public async updateDish(updateDish: UpdateDish)
    {
        await this.coll.updateOne(updateDish.dishOld, { $set: updateDish.dishNew });
    }    

    public async listDishes() : Promise<Dish[]>
    {
        return await this.coll.find({}).toArray();
    }


}


  