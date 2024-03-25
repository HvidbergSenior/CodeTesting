import { useTranslation } from "react-i18next";
import Typography from "@mui/material/Typography";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Checkbox from "@mui/material/Checkbox";
import { GetCompensationPaymentParticipantResponse } from "api/generatedApi";
import { formatNumberToAmount, formatNumberToPrice } from "utils/formats";

interface Props {
  row: GetCompensationPaymentParticipantResponse;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export function CompensationPaymentParticipantSelectorStepTableRow(props: Props) {
  const { row, onCheck, checked } = props;
  const { t } = useTranslation();

  return (
    <TableRow>
      <TableCell>
        <Checkbox
          data-testid="dashboard-compensation-payments-table-row-checkbox"
          onChange={() => onCheck(row.projectParticipantId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
        />
      </TableCell>
      <TableCell>
        <Typography variant="body2">{row.name}</Typography>
        <Typography variant="caption">{row.email}</Typography>
      </TableCell>
      <TableCell align="right">
        <Typography variant="body2">{t("valueLabels.hours", { hours: formatNumberToAmount(row.hours) })}</Typography>
        <Typography variant="caption">{t("valueLabels.price", { price: formatNumberToPrice(row.payment) })}</Typography>
      </TableCell>
    </TableRow>
  );
}
