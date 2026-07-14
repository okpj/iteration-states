import { useEffect, useState } from "react";
import { IIteration } from "../model/IIteration";
import { getIterations } from "../services/iteration-service";

export function useIterations() {
    const [iterations, setIterations] = useState<IIteration[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let cancelled = false;
        getIterations()
            .then(result => {
                if (cancelled)
                    return;
                setIterations(result.sort((a, b) => b.finishDate.getTime() - a.finishDate.getTime()));
            })
            .catch(err => {
                if (cancelled)
                    return;
                setError(err instanceof Error ? err.message : String(err));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });
        return () => { cancelled = true; };
    }, []);

    return { iterations, loading, error };
}
