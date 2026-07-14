import { useCallback, useState } from "react";
import { IIteration } from "../model/IIteration";
import { IIterationStatApi } from "../model/IIterationStat";
import { getIterationStat } from "../services/iteration-service";

export function useIterationStats() {
    const [statItems, setStatItems] = useState<IIterationStatApi[]>([]);
    const [addedIterationIds, setAddedIterationIds] = useState<Set<string>>(new Set());
    const [error, setError] = useState<string | null>(null);

    const addIteration = useCallback((iteration: IIteration) => {
        if (addedIterationIds.has(iteration.id)) {
            return;
        }
        setAddedIterationIds(current => new Set(current).add(iteration.id));
        getIterationStat(iteration.id)
            .then(stat => setStatItems(current => [...current, stat]))
            .catch(err => setError(err instanceof Error ? err.message : String(err)));
    }, [addedIterationIds]);

    return { statItems, addIteration, error };
}
