export interface IIteration {
    id: string,
    name: string,
    path: string,
    startDate: Date;
    finishDate: Date;
    timeFrame: string;
}

export class Iterations implements IIteration {
    id: string;
    name: string;
    path: string;
    startDate: Date;
    finishDate: Date;
    timeFrame: string;

    constructor(id: string, name: string, path: string, startDate: any, finishDate: Date, timeFrame: string) {

        this.id = id;
        this.name = name;
        this.path = path;
        this.startDate = new Date(startDate)
        this.finishDate = new Date(finishDate)
        this.timeFrame = timeFrame
    }
}