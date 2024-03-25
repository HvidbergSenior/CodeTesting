import Checkbox from "@mui/material/Checkbox";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { GetProjectResponse, CompensationResponse } from "api/generatedApi";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { formatNumberToPrice } from "utils/formats";
import { useCompensationPaymentFormatter } from "./hooks/use-format-participants";
import { useDialog } from "shared/dialog/use-dialog";
import { CompensationPaymentInfoDialog } from "./info/info-dialog";

interface Props {
  project: GetProjectResponse;
  row: CompensationResponse;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export const DesktopCompensationPaymentRow = (props: Props) => {
  const { project, row, onCheck, checked } = props;
  const { canSelectCompensationPayments } = useDashboardRestrictions(project);
  const { formatParticipantList } = useCompensationPaymentFormatter();
  const [infoDialog] = useDialog(CompensationPaymentInfoDialog);

  const onclick = () => {
    infoDialog({ compensationPayment: row });
  };
  return (
    <TableRow data-testid={`dashboard-compensation-row-${row.projectCompensationId}`} onClick={onclick}>
      <TableCell>
        <Checkbox
          data-testid="dashboard-compensation-payment-row-checkbox"
          onChange={() => onCheck(row.projectCompensationId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
          disabled={!canSelectCompensationPayments()}
        />
      </TableCell>
      <TableCell data-testid="dashboard-compensation-payment-row-period">
        <Typography variant="body2">
          {row.startDate} - {row.endDate}
        </Typography>
      </TableCell>
      <TableCell data-testid="dashboard-compensation-payment-row-users">
        <Typography variant="body2">{formatParticipantList(10, row.compensationParticipant)}</Typography>
      </TableCell>
      <TableCell data-testid="dashboard-compensation-payment-row-amount" align="right" sx={{ pr: 5 }}>
        <Typography variant="body2">{formatNumberToPrice(row.compensationPaymentDkr)}</Typography>
      </TableCell>
    </TableRow>
  );
};
