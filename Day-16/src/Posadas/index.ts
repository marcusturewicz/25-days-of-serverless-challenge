import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import * as moment from 'moment-timezone';

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {

    var now = moment().tz('Mexico/Mexico_City').toDate();
    var month = now.getMonth();
    context.res = {
        status: 200,
        body: []
    };
    if (month != 11)
        return;
    var day = now.getDate();
    context.res.body = posadas.filter(x => x.day >= day)
};

export default httpTrigger;

class Posada {
    day: number;
    location: string;
    host: string;
}

/********************** Add Posada's here ********************/
const posadas: Posada[] = [
    { day: 16, location: 'Mexico City', host: 'Marcus Turewicz' },
    { day: 17, location: 'Ecatepec', host: 'Marcus Turewicz' },
    { day: 18, location: 'Guadalajara', host: 'Marcus Turewicz' },
    { day: 19, location: 'Puebla', host: 'Marcus Turewicz' },
    // { day: 20, location: 'Juárez', host: 'Marcus Turewicz' },
    // { day: 21, location: 'Tijuana', host: 'Marcus Turewicz' },
    // { day: 22, location: 'León', host: 'Marcus Turewicz' },
    // { day: 23, location: 'Monterrey', host: 'Marcus Turewicz' },
    // { day: 24, location: 'Zapopan', host: 'Marcus Turewicz' },
    // { day: 25, location: 'Nezahualcóyotl', host: 'Marcus Turewicz' }
]
/*************************************************************/
