import { useTranslation } from "react-i18next";
import Checkbox from "@mui/material/Checkbox";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { GetProjectResponse, GetProjectSpecificOperationResponse } from "api/generatedApi";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { formatNumberToPrice } from "utils/formats";
import { useUpdateProjectSpecificOperation } from "./edit/hooks/use-update-projects-specific-operation";

interface Props {
  project: GetProjectResponse;
  row: GetProjectSpecificOperationResponse;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export const DesktopProjectSpecificOperationRow = (props: Props) => {
  const { project, row, onCheck, checked } = props;
  const { t } = useTranslation();
  const { canSelectProjectSpecificOperations } = useDashboardRestrictions(project);
  const updateDialog = useUpdateProjectSpecificOperation({ operation: row, project });

  const onclick = () => {
    updateDialog();
  };
  return (
    <TableRow data-testid={`dashboard-project-specific-operation-row-${row.projectSpecificOperationId}`} onClick={onclick}>
      <TableCell>
        <Checkbox
          data-testid="dashboard-project-specific-operation-row-checkbox"
          onChange={() => onCheck(row.projectSpecificOperationId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
          disabled={!canSelectProjectSpecificOperations()}
        />
      </TableCell>
      <TableCell data-testid="dashboard-project-specific-operation-row-extra-work-agrrement-number" align="left">
        <Typography variant="body2">{row.extraWorkAgreementNumber}</Typography>
      </TableCell>
      <TableCell data-testid="dashboard-project-specific-operation-row-name" align="left">
        <Typography variant="body2">{row.name}</Typography>
      </TableCell>
      <TableCell data-testid="dashboard-project-specific-operation-row-time" align="left">
        <Typography variant="body2">
          {row.operationTimeMs && !row.workingTimeMs
            ? t("dashboard.projectSpecificOperations.types.operationTime")
            : t("dashboard.projectSpecificOperations.types.workingTime")}
        </Typography>
      </TableCell>
      <TableCell data-testid="dashboard-project-specific-operation-row-payment" align="right" sx={{ pr: 5 }}>
        <Typography variant="body2">{formatNumberToPrice(row.payment)}</Typography>
      </TableCell>
    </TableRow>
  );
};
