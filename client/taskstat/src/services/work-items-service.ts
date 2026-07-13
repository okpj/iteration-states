import { TASK_API_URL } from "../constants";
import { ApiWorkItem, IApiWorkItem } from "../model/IWorkItem";

export function getWorkItemsByIterationPath(iterationPath: string): Promise<IApiWorkItem[]> {
    const requestOptions = {
        method: "GET",
    };
    const parameters = {
        iteration: iterationPath,
        team: "flex"
    };

    const queryParameters = new URLSearchParams(parameters);
    const url = `${TASK_API_URL}/Task/get-iterations-work-items?${queryParameters.toString()}`;
    return fetch(url, requestOptions)
        .then(async (response) => {
            const json = await response.json();
            return json.map((value: any) => {
                return new ApiWorkItem(value)
            })
        });
}