import { TableCell, TableRow, Typography } from "@mui/material";
import { GroupedWorkItemsResponse } from "api/generatedApi";
import { formatNumberToAmount, formatNumberToPrice } from "utils/formats";

interface Props {
  groupedWorkItem: GroupedWorkItemsResponse;
}

export const GroupingWorkitemRow = ({ groupedWorkItem }: Props) => {
  return (
    <TableRow data-testid={`overview-grouping-dialog-row-${groupedWorkItem.text}`}>
      <TableCell sx={{ maxWidth: "300px" }} data-testid="overview-grouping-dialog-row-title" title={groupedWorkItem.text ?? ""}>
        <Typography variant="body2">{groupedWorkItem.text}</Typography>
        <Typography variant="caption" color={"primary.main"}>
          {groupedWorkItem.id}
        </Typography>
      </TableCell>
      <TableCell sx={{ textAlign: "right", pr: 5.5 }} data-testid="overview-grouping-dialog-row-amount">
        {formatNumberToAmount(groupedWorkItem.amount)}
      </TableCell>
      <TableCell sx={{ textAlign: "right", pr: 5.5 }} data-testid="overview-grouping-dialog-row-payment">
        {formatNumberToPrice(groupedWorkItem.paymentDkr)}
      </TableCell>
    </TableRow>
  );
};
