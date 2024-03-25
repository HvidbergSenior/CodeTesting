import Checkbox from "@mui/material/Checkbox";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
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

export const DesktopExtraWorkAgreementRow = (props: Props) => {
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
          data-testid="dashboard-extra-work-agreement-row-checkbox"
          onChange={() => onCheck(row.extraWorkAgreementId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
          disabled={!canSelectExtraWorkAgreements()}
        />
      </TableCell>
      <TableCell align="left" sx={{ pr: 5 }} data-testid="dashboard-extra-work-agreement-row-id">
        {row.extraWorkAgreementNumber}
      </TableCell>
      <TableCell data-testid="dashboard-extra-work-agreement-row-name">{row.name}</TableCell>
      <TableCell data-testid="dashboard-extra-work-agreement-row-type">{getTypeByAgreementFormatted(row)}</TableCell>
      <TableCell align="right" sx={{ pr: 5 }} data-testid="dashboard-extra-work-agreement-row-hours">
        {formatHoursAndMinutes(row.workTime?.hours, row.workTime?.minutes)}
      </TableCell>
      <TableCell align="right" sx={{ pr: 5 }} data-testid="dashboard-extra-work-agreement-row-payment">
        {formatNumberToPrice(row.paymentDkr)}
      </TableCell>
    </TableRow>
  );
};
