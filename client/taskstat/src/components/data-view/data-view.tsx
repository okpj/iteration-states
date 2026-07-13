import { IIteration } from "../../model/IIteration";
import { useEffect, useState } from "react";
import { getIterations, GetIterationStat } from "../../services/iteration-service";
import { IterationSelector } from "./iteration-selector/iteration-selector";
import './data-view.css'
import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { IApiWorkItem } from "../../model/IWorkItem";
import { IIterationStatApi } from "../../model/IIterationStat";

export function DataView() {
    const [iterations, setIterations] = useState([] as IIteration[])
    const [statItems, setStatItems] = useState([] as IIterationStatApi[]);

    useEffect(() => {
        getIterations()
            .then(result => {
                setIterations(result.sort((a, b) => b.finishDate.getTime() - a.finishDate.getTime()))
            })
    }, [])

    const selectIteration = ((iteration: IIteration) => {
        GetIterationStat(iteration.id)
            .then(response => {
                setStatItems(current => [...current, response]);
            });
    })

    return <div className="data-view-contaier content">
        {renderIterationSelector(iterations, selectIteration)}
        {renderStatTable(statItems)}
    </div>
}

function renderIterationSelector(iterations: IIteration[], selectIteration: (iteration: IIteration) => void) {
    return <IterationSelector
        Iterations={iterations}
        selectIteration={selectIteration}
        Title="Итерация"
    ></IterationSelector>
}

function renderStatTable(items: IIterationStatApi[]) {
    if (items.length === 0) {
        return <div></div>
    }

    return <TableContainer component={Paper}>
        <Table>
            <TableHead>
                <TableRow key="header-key">
                    <TableCell>Спринт</TableCell>
                    <TableCell>Кол-во SP</TableCell>
                    <TableCell>Закрыли SP</TableCell>
                    <TableCell>% выполнения SP</TableCell>
                    <TableCell>Кол-во US</TableCell>
                    <TableCell>Закрыли US</TableCell>
                    <TableCell>% выполнения US</TableCell>
                </TableRow>
            </TableHead>
            <TableBody key="table-body">
                {items.map((row) => (
                    <TableRow key={row.iteration}>
                        <TableCell>{row.iteration}</TableCell>
                        <TableCell>{row.countSP}</TableCell>
                        <TableCell>{row.countClosedSP}</TableCell>
                        <TableCell>{row.percentClosedSP}</TableCell>
                        <TableCell>{row.countUS}</TableCell>
                        <TableCell>{row.countClosedUS}</TableCell>
                        <TableCell>{row.percentClosedUS}</TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    </TableContainer>
}

function renderTable(items: IApiWorkItem[]) {
    if (items.length === 0) {
        return <div></div>
    }
    return <TableContainer component={Paper}>
        <Table>
            <TableHead>
                <TableRow key="header-key">
                    <TableCell>Id</TableCell>
                    <TableCell>Title</TableCell>
                    <TableCell>WorkItemType</TableCell>
                    <TableCell>ActivatedDate</TableCell>
                    <TableCell>ClosedDate</TableCell>
                    <TableCell>StoryPoints</TableCell>
                    <TableCell>IterationPath</TableCell>
                    <TableCell>ParentId</TableCell>
                    <TableCell>State</TableCell>
                </TableRow>
            </TableHead>
            <TableBody key="table-body">
                {items.map((row) => (
                    <TableRow key={row.id}>
                        <TableCell>{row.id}</TableCell>
                        <TableCell>{row.title}</TableCell>
                        <TableCell>{row.workItemType}</TableCell>
                        <TableCell>{row.activatedDate?.toDateString()}</TableCell>
                        <TableCell>{row.closedDate?.toDateString()}</TableCell>
                        <TableCell>{row.storyPoints}</TableCell>
                        <TableCell>{row.IiterationPath}</TableCell>
                        <TableCell>{row.parentId}</TableCell>
                        <TableCell>{row.state}</TableCell>

                    </TableRow>
                ))}
            </TableBody>
        </Table>
    </TableContainer>
}
