import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { IIterationStatApi } from "../../model/IIterationStat";

interface IStatTableProps {
    items: IIterationStatApi[];
}

export function StatTable({ items }: IStatTableProps) {
    if (items.length === 0) {
        return null;
    }

    return <TableContainer component={Paper}>
        <Table>
            <TableHead>
                <TableRow>
                    <TableCell>Спринт</TableCell>
                    <TableCell>Кол-во SP</TableCell>
                    <TableCell>Закрыли SP</TableCell>
                    <TableCell>% выполнения SP</TableCell>
                    <TableCell>Кол-во US</TableCell>
                    <TableCell>Закрыли US</TableCell>
                    <TableCell>% выполнения US</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
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
