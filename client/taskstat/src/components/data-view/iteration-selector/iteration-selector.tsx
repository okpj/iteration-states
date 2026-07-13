import { Button, FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { useCallback, useState } from "react";
import { IIteration } from "../../../model/IIteration";

interface IIterationSelectorPros {
    Iterations: IIteration[],
    selectIteration(iteraion: IIteration): void
    Title: string
}

export function IterationSelector(props: IIterationSelectorPros) {
    const [iteration, setIteration] = useState({} as IIteration);

    const addIterationHandler = useCallback((iteration: IIteration) => {
        props.selectIteration(iteration);
    }, [props])

    return <FormControl fullWidth>
        <InputLabel id="iteraion-label">{props.Title}</InputLabel>
        <Select
            labelId="iteraion-label"
            value={iteration?.id ?? ''}
            label="Iteration"
            onChange={(event) => {
                const iteration = props.Iterations.find(x => x.id === event.target.value) ?? {} as IIteration;
                setIteration(iteration);
            }}
        >
            {props.Iterations?.map(x => {
                return <MenuItem value={x.id}>{x.name}</MenuItem>
            })}
        </Select>
        {
            iteration?.id ?
                (<Button onClick={(event) => addIterationHandler(iteration)}>Добавить</Button>)
                : null
        }
    </FormControl>
}