import { TableCell, TableRow } from "@mui/material";
import { GroupedWorkItemsResponse } from "api/generatedApi";
import { Typography2LinesStyled } from "shared/styles/typography-styled";
import { formatNumberToAmount } from "utils/formats";

interface Props {
  groupedWorkItem: GroupedWorkItemsResponse;
}

const GroupingWidgetRow = ({ groupedWorkItem }: Props) => {
  return (
    <TableRow data-testid={`overview-grouping-widget-row-${groupedWorkItem.text}`}>
      <TableCell data-testid="overview-grouping-widget-row-title" title={groupedWorkItem.text ?? ""}>
        <Typography2LinesStyled variant="body2">{groupedWorkItem.text}</Typography2LinesStyled>
      </TableCell>
      <TableCell sx={{ textAlign: "right", pr: 5 }} data-testid="overview-grouping-widget-row-quantity">
        {formatNumberToAmount(groupedWorkItem.amount)}
      </TableCell>
    </TableRow>
  );
};

export default GroupingWidgetRow;
