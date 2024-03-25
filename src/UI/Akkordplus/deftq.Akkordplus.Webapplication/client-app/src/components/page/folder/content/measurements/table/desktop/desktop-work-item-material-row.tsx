import { useTranslation } from "react-i18next";
import Checkbox from "@mui/material/Checkbox";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Chip from "@mui/material/Chip";
import { ProjectResponse, WorkItemResponse } from "api/generatedApi";
import { costumPalette } from "theme/palette";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { getHmsFromMilliSeconds, formatNumberToPrice, formatNumberToDkNumber } from "utils/formats";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { ShowWorkItemInfoDialog } from "../../info/show-work-item-info-dialog";
import { useDialog } from "shared/dialog/use-dialog";
import { useEditWorkItem } from "../../edit/hooks/use-edit-work-item";

interface Prop {
  project: ProjectResponse;
  folder: ExtendedProjectFolder;
  workItem: WorkItemResponse;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export const DesktopWorkItemMaterialTableRow = (props: Prop) => {
  const { project, folder, workItem, onCheck, checked } = props;
  const { t } = useTranslation();
  const { canSelectWorkItem, canEditWorkitem } = useWorkItemRestrictions(project);
  const [openInfoDialog] = useDialog(ShowWorkItemInfoDialog);
  const openEditDialog = useEditWorkItem({ project, folder, workItem });

  const showWorkItemInfo = () => {
    openInfoDialog({ project, folder, workItem });
  };

  return (
    <TableRow data-testid={`measurements-material-table-row-${workItem.workItemText}`} sx={{ cursor: "pointer" }}>
      <TableCell>
        <Checkbox
          data-testid="measurement-table-row-material-checkbox"
          onChange={(event) => onCheck(workItem.workItemId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
          disabled={!canSelectWorkItem(folder)}
        />
      </TableCell>
      <TableCell
        sx={{ color: "primary.main" }}
        data-testid="measurements-table-cell-material"
        title={workItem.workItemText ?? ""}
        onClick={showWorkItemInfo}
      >
        {workItem.workItemText}
      </TableCell>
      <TableCell sx={{ color: "primary.main" }} data-testid="measurements-table-cell-moutingcode" onClick={showWorkItemInfo}>
        {workItem?.workItemMaterial?.workItemMountingCode && workItem.workItemMaterial?.workItemMountingCode > 0
          ? workItem.workItemMaterial?.workItemMountingCodeText
          : ""}
      </TableCell>
      <TableCell sx={{ color: "primary.main" }} data-testid="measurements-table-cell-supplements" onClick={showWorkItemInfo}>
        {workItem.workItemMaterial?.supplementOperations && workItem.workItemMaterial?.supplementOperations.length > 0 && (
          <Chip
            data-testid="measurements-table-cell-supplements-chip-operation"
            sx={{ mb: "5px", mr: "5px", bgcolor: costumPalette.operationSupplementBg, color: costumPalette.operationSupplementColor }}
            label={t("content.measurements.table.cellValue.operationSupplement", { value: workItem.workItemMaterial?.supplementOperations.length })}
          />
        )}
        {workItem.supplements && workItem.supplements.length > 0 && (
          <Chip
            data-testid="measurements-table-cell-supplements-chip-supplement"
            sx={{ mb: "5px", bgcolor: costumPalette.supplementBg, color: costumPalette.supplementColor }}
            label={t("content.measurements.table.cellValue.supplement")}
          />
        )}
      </TableCell>

      {/* KTH 2/1-23 hide for better calculation <TableCell sx={{ color: "primary.main" }} data-testid="measurements-table-cell-operationtime">{getHmsFromMilliSeconds(workItem.workItemOperationTimeMilliseconds)}</TableCell> */}
      <TableCell
        sx={{ color: "primary.main", textAlign: "right", pr: 5 }}
        data-testid="measurements-table-cell-amount"
        onClick={canEditWorkitem(folder) ? openEditDialog : showWorkItemInfo}
      >
        {formatNumberToDkNumber(workItem.workItemAmount)}
      </TableCell>
      {/* KTH 14/4-23 hide until we have units <TableCell sx={{ color: "primary.main", textAlign: "right" }} data-testid="measurements-table-cell-unit" onClick={showWorkItemInfo}>
        TEST
      </TableCell> */}
      <TableCell
        sx={{ color: "primary.main", textAlign: "right", pr: 5 }}
        data-testid="measurements-table-cell-total-operationtime"
        onClick={showWorkItemInfo}
      >
        {getHmsFromMilliSeconds(workItem.workItemTotalOperationTimeMilliseconds)}
      </TableCell>
      <TableCell
        sx={{ color: "primary.main", textAlign: "right", pr: 5 }}
        data-testid="measurements-table-cell-total-payment"
        onClick={showWorkItemInfo}
      >
        {formatNumberToPrice(workItem.workItemTotalPaymentDkr)}
      </TableCell>
    </TableRow>
  );
};
