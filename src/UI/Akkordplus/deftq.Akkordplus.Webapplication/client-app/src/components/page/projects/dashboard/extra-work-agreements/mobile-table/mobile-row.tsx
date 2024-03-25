import Checkbox from "@mui/material/Checkbox";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { ExtraWorkAgreementResponse, GetProjectResponse } from "api/generatedApi";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { formatHoursAndMinutes, formatNumberToPrice } from "utils/formats";
import { useUpdateExtraWorkAgreement } from "../edit/hooks/use-update-extra-work-agreement";
import { useExtraWorkAgreementFormatter } from "../hooks/format";

interface Props {
  project: GetProjectResponse;
  row: ExtraWorkAgreementResponse;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export const MobileExtraWorkAgreementRow = (props: Props) => {
  const { project, row, onCheck, checked } = props;
  const { canSelectExtraWorkAgreements } = useDashboardRestrictions(project);
  const { getTypeByAgreementFormatted } = useExtraWorkAgreementFormatter();
  const openUpdateExtraWorkAgreementDialog = useUpdateExtraWorkAgreement({ project, agreement: row });

  return (
    <TableRow
      data-testid={`dashboard-extra-work-agreement-row-${row.name}`}
      onClick={() => {
        openUpdateExtraWorkAgreementDialog();
      }}
      sx={{ cursor: "pointer" }}
    >
      <TableCell>
        <Checkbox
          data-testid="dashboard-extrawork-agreement-row-checkbox"
          onChange={() => onCheck(row.extraWorkAgreementId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
          disabled={!canSelectExtraWorkAgreements()}
        />
      </TableCell>
      <TableCell data-testid="dashboard-extra-work-agreement-row-name" title={row.name ?? ""}>
        <Typography variant="body2">{row.name}</Typography>
        <Typography variant="body2" color={"primary.main"}>
          {row.extraWorkAgreementNumber}
        </Typography>
        <Typography variant="caption" color={"primary.main"}>
          {getTypeByAgreementFormatted(row)}
        </Typography>
      </TableCell>
      <TableCell data-testid="dashboard-extra-work-agreement-row-payment" title={row.name ?? ""} align="right" sx={{ pr: 5 }}>
        <Typography variant="body2">{formatNumberToPrice(row.paymentDkr)}</Typography>
        <Typography variant="caption" color={"primary.main"}>
          {formatHoursAndMinutes(row.workTime?.hours, row.workTime?.minutes)}
        </Typography>
      </TableCell>
    </TableRow>
  );
};
