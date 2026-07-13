import { TASK_API_URL } from "../constants";
import { IIteration, Iterations } from "../model/IIteration";
import { IIterationStatApi } from "../model/IIterationStat";


// export function getIterationsFromTFS(): Promise<IIteration[]> {
//     const credentials = btoa(`:${TFS_API_PAT}`);
//     const auth = { "Authorization": `Basic ${credentials}` }
//     const url = `${TFS_API_URL}/${TFS_API_ORGANIZATION}/${TFS_API_PROJECT}/${TFS_API_TEAM}/_apis/work/teamsettings/iterations`;
//     return fetch(url, { headers: auth })
//         .then(async response => {
//             const json = await response.json();
//             return json.value.map((value: any) => {
//                 return {
//                     Id: value.id,
//                     Name: value.name,
//                     Path: value.path,
//                     Attributes: new IterationAttributes(
//                         value.attributes.startDate,
//                         value.attributes.finishDate,
//                         value.attributes.timeFrame)
//                 } as IIteration
//             });
//         })
// }

export function getIterations(): Promise<IIteration[]> {
    const requestOptions = {
        method: "GET",
    };
    const parameters = {
        team: "flex"
    };

    const queryParameters = new URLSearchParams(parameters);
    const url = `${TASK_API_URL}/Iterations/get-iterations?${queryParameters.toString()}`;
    return fetch(url, requestOptions)
        .then(async (response) => {
            const json = await response.json();
            return json.map((value: any) => {
                return new Iterations(value.id,
                    value.name,
                    value.path,
                    value.startDate,
                    value.finishDate,
                    value.timeFrame)
            });
        });
}

export async function GetIterationStat(iterationId: string) {
    const requestOptions = {
        method: "GET",
    };
    const parameters = {
        iterationId: iterationId,
        team: "flex"
    };

    const queryParameters = new URLSearchParams(parameters);
    const url = `${TASK_API_URL}/Iterations/get-stat?${queryParameters.toString()}`;
    return fetch(url, requestOptions)
        .then(async (response) => {
            const json = await response.json();
            return json as IIterationStatApi;
        });
}