import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { CompensationResponse } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { formatNumberToPrice } from "utils/formats";
import { CompensationPaymentInfoDialog } from "./info/info-dialog";

interface Props {
  row: CompensationResponse;
}

export const MobileCompensationPaymentRow = (props: Props) => {
  const { row } = props;
  const [infoDialog] = useDialog(CompensationPaymentInfoDialog);

  const onclick = () => {
    infoDialog({ compensationPayment: row });
  };

  return (
    <TableRow data-testid={`dashboard-compensation-payment-row-${row.projectCompensationId}`} onClick={onclick}>
      <TableCell data-testid="dashboard-compensation-payment-row-period">
        <Typography variant="body2">
          {row.startDate} - {row.endDate}
        </Typography>
      </TableCell>
      <TableCell data-testid="dashboard-compensation-payment-row-amount" align="right">
        <Typography variant="body2">{formatNumberToPrice(row.compensationPaymentDkr)}</Typography>
      </TableCell>
    </TableRow>
  );
};
