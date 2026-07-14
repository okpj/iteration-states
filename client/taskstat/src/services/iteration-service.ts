import { TASK_API_URL, TFS_API_TEAM } from "../constants";
import { IIteration, Iterations } from "../model/IIteration";
import { IIterationStatApi } from "../model/IIterationStat";

async function fetchJson(url: string) {
    const response = await fetch(url, { method: "GET" });
    if (!response.ok) {
        throw new Error(`Request to ${url} failed with status ${response.status}`);
    }
    return response.json();
}

export async function getIterations(): Promise<IIteration[]> {
    const queryParameters = new URLSearchParams({ team: TFS_API_TEAM ?? "" });
    const url = `${TASK_API_URL}/Iterations/get-iterations?${queryParameters.toString()}`;
    const json = await fetchJson(url);
    return json.map((value: any) => new Iterations(
        value.id,
        value.name,
        value.path,
        value.startDate,
        value.finishDate,
        value.timeFrame));
}

export async function getIterationStat(iterationId: string): Promise<IIterationStatApi> {
    const queryParameters = new URLSearchParams({ iterationId, team: TFS_API_TEAM ?? "" });
    const url = `${TASK_API_URL}/Iterations/get-stat?${queryParameters.toString()}`;
    return await fetchJson(url) as IIterationStatApi;
}
