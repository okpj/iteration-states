export interface IApiWorkItem {
    id: number;
    title: string | null;
    IiterationPath: string | null;
    activatedDate: Date | null;
    closedDate: Date | null;
    storyPoints: number | null;
    state: string | null;
    workItemType: string | null;
    parentId: number | null;
    parent: IApiWorkItem | null;
}

export class ApiWorkItem implements IApiWorkItem {
    id!: number;
    title!: string | null;
    IiterationPath!: string | null;
    activatedDate!: Date | null;
    closedDate!: Date | null;
    storyPoints!: number | null;
    state!: string | null;
    workItemType!: string | null;
    parentId!: number | null;
    parent!: IApiWorkItem | null;

    constructor(value: any) {
        if (value) {
            this.id = value.id;
            this.title = value.title;
            this.activatedDate = value.activatedDate ? new Date(value.activatedDate) : null;
            this.closedDate = value.closedDate ? new Date(value.closedDate) : null;
            this.IiterationPath = value.iterationPath;
            this.storyPoints = value.storyPoints;
            this.state = value.state;
            this.parentId = value.parentId;
            this.parent = value.parent ? new ApiWorkItem(value.parent) : null;
            this.workItemType = value.workItemType;
        }
    }
}