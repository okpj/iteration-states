import React from 'react';
import { IApiWorkItem } from '../model/IWorkItem';

export interface IWorkItemPops {
    workItem: IApiWorkItem
}

function WorkIItem(props: IWorkItemPops) {
    const workitem = props.workItem;
    return <div>
        <span>{workitem.id}</span>
        {renderFields(workitem)}
    </div>
}

function renderFields(workItem: IApiWorkItem) {
    const fields = Object.entries(workItem ?? {});
    return fields.map(([key, value]) => {
        return <div>
            <span>{key}</span>
            <span>{value}</span>
        </div>
    })
}

export default WorkIItem