import { IterationSelector } from "./iteration-selector/iteration-selector";
import { StatTable } from "./stat-table";
import { useIterations } from "../../hooks/useIterations";
import { useIterationStats } from "../../hooks/useIterationStats";
import './data-view.css'

export function DataView() {
    const { iterations, error: iterationsError } = useIterations();
    const { statItems, addIteration, error: statError } = useIterationStats();

    return <div className="data-view-contaier content">
        <IterationSelector
            iterations={iterations}
            selectIteration={addIteration}
            title="Итерация"
        />
        {iterationsError && <div>Не удалось загрузить список итераций: {iterationsError}</div>}
        {statError && <div>Не удалось загрузить статистику: {statError}</div>}
        <StatTable items={statItems} />
    </div>
}
