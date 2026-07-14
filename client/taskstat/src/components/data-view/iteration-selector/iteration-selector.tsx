import { Button, FormControl, InputLabel, MenuItem, Select } from "@mui/material";
import { useCallback, useState } from "react";
import { IIteration } from "../../../model/IIteration";

interface IIterationSelectorProps {
    iterations: IIteration[];
    selectIteration(iteration: IIteration): void;
    title: string;
}

export function IterationSelector(props: IIterationSelectorProps) {
    const [iteration, setIteration] = useState({} as IIteration);

    const addIterationHandler = useCallback((iteration: IIteration) => {
        props.selectIteration(iteration);
    }, [props])

    return <FormControl fullWidth>
        <InputLabel id="iteraion-label">{props.title}</InputLabel>
        <Select
            labelId="iteraion-label"
            value={iteration?.id ?? ''}
            label="Iteration"
            onChange={(event) => {
                const iteration = props.iterations.find(x => x.id === event.target.value) ?? {} as IIteration;
                setIteration(iteration);
            }}
        >
            {props.iterations?.map(x => {
                return <MenuItem key={x.id} value={x.id}>{x.name}</MenuItem>
            })}
        </Select>
        {
            iteration?.id ?
                (<Button onClick={() => addIterationHandler(iteration)}>Добавить</Button>)
                : null
        }
    </FormControl>
}
