import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { GetProjectSpecificOperationResponse } from "api/generatedApi";
// import { useDialog } from "shared/dialog/use-dialog";
import { formatNumberToPrice } from "utils/formats";

interface Props {
  row: GetProjectSpecificOperationResponse;
}

export const MobileProjectSpecificOperationRow = (props: Props) => {
  const { row } = props;
  // const [infoDialog] = useDialog(CompensationPaymentInfoDialog);

  const onclick = () => {
    // infoDialog({ compensationPayment: row });
  };

  return (
    <TableRow data-testid={`dashboard-project-specific-operation-row-${row.name}`} onClick={onclick}>
      <TableCell data-testid="dashboard-project-specific-operation-row-name" align="left">
        <Typography variant="body2">{row.name}</Typography>
        <Typography variant="body2" color="primary.main">
          {row.extraWorkAgreementNumber}
        </Typography>
      </TableCell>
      <TableCell data-testid="dashboard-project-specific-operation-row-payment" align="right">
        <Typography variant="body2">{formatNumberToPrice(row.payment)}</Typography>
      </TableCell>
    </TableRow>
  );
};
