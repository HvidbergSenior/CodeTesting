import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Checkbox from "@mui/material/Checkbox";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Chip from "@mui/material/Chip";
import { useTranslation } from "react-i18next";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { formatNumberToPrice, formatNumberToDkNumber } from "utils/formats";
import { costumPalette } from "theme/palette";
import { ProjectResponse, WorkItemResponse } from "api/generatedApi";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { useDialog } from "shared/dialog/use-dialog";
import { ShowWorkItemInfoDialog } from "../../info/show-work-item-info-dialog";
import { TableCellStyled, Typography2LinesStyled } from "./mobile-work-item-row-styles";
import { useEditWorkItem } from "../../edit/hooks/use-edit-work-item";

interface Prop {
  project: ProjectResponse;
  folder: ExtendedProjectFolder;
  workItem: WorkItemResponse;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export const MobileWorkItemOperationTableRow = (props: Prop) => {
  const { project, folder, workItem, onCheck, checked } = props;
  const { t } = useTranslation();
  const { canSelectWorkItem, canEditWorkitem } = useWorkItemRestrictions(project);
  const [openInfoDialog] = useDialog(ShowWorkItemInfoDialog);
  const openEditDialog = useEditWorkItem({ project, folder, workItem });

  const showWorkItemInfo = () => {
    openInfoDialog({ project, folder, workItem });
  };

  return (
    <TableRow sx={{ cursor: "pointer" }}>
      <TableCell sx={{ p: 0, pt: 0.5, verticalAlign: "top" }}>
        <Checkbox
          data-testid="mobile-measurement-table-operation-row-checkbox"
          onChange={(event) => onCheck(workItem.workItemId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
          disabled={!canSelectWorkItem(folder)}
        />
      </TableCell>
      <TableCellStyled title={workItem.workItemText ?? ""} sx={{ pr: 1, pt: 2, verticalAlign: "top" }} onClick={showWorkItemInfo}>
        <Box>
          <Typography2LinesStyled variant="body2" color={"primary.main"}>
            {workItem.workItemText}
          </Typography2LinesStyled>
          <Box sx={{ display: "flex", flexDirection: "row" }}>
            {workItem.supplements && workItem.supplements.length > 0 && (
              <Chip
                size="small"
                label={t("content.measurements.table.cellValue.supplementSmall")}
                sx={{ bgcolor: costumPalette.supplementBg, color: costumPalette.supplementColor, marginRight: 0.5 }}
              />
            )}
          </Box>
        </Box>
      </TableCellStyled>
      <TableCell
        sx={{ pl: 0, pr: 0, textAlign: "center", verticalAlign: "top" }}
        onClick={canEditWorkitem(folder) ? openEditDialog : showWorkItemInfo}
      >
        <Box sx={{ display: "flex", flexDirection: "column" }}>
          <Typography variant="body2" color={"primary.main"}>
            {formatNumberToDkNumber(workItem.workItemAmount)}
          </Typography>
          {/* KTH 14/4-23 hide until we have units <Typography variant="body2" color={"grey.50"}>
            TEST
          </Typography> */}
        </Box>
      </TableCell>
      <TableCell sx={{ p: 0, textAlign: "right", pt: 2, pr: 1, verticalAlign: "top", color: "primary.main" }} onClick={showWorkItemInfo}>
        {formatNumberToPrice(workItem.workItemTotalPaymentDkr)}
      </TableCell>
    </TableRow>
  );
};
