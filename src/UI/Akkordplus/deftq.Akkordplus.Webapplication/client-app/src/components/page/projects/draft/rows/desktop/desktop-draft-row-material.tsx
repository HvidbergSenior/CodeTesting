import Checkbox from "@mui/material/Checkbox";
import Chip from "@mui/material/Chip";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { costumPalette } from "theme/palette";
import { formatNumberToDkNumber } from "utils/formats";
import { DraftWorkItem } from "utils/offline-storage/use-offline-storage";
import { ShowDraftInfoDialog } from "../../info/show-draft-info-dialog";

interface Prop {
  draftWorkItem: DraftWorkItem;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export const DesktopDraftRowMaterial = (props: Prop) => {
  const { draftWorkItem, onCheck, checked } = props;
  const { t } = useTranslation();
  const [openInfoDialog] = useDialog(ShowDraftInfoDialog);

  const showDraftInfo = () => {
    openInfoDialog({ draft: draftWorkItem });
  };

  return (
    <TableRow data-testid={`draft-material-table-row-${draftWorkItem.material?.name}`} onClick={showDraftInfo}>
      <TableCell>
        <Checkbox
          data-testid="draft-table-row-material-checkbox"
          onChange={(event) => onCheck(draftWorkItem.draftId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
        />
      </TableCell>
      <TableCell sx={{ color: "primary.main" }} data-testid="draft-table-cell-material" title={draftWorkItem.material?.name ?? ""}>
        {draftWorkItem.material?.name}
      </TableCell>
      <TableCell sx={{ color: "primary.main" }} data-testid="draft-table-cell-moutingcode">
        {draftWorkItem?.workItemMountingCode && draftWorkItem.workItemMountingCode > 0 ? draftWorkItem.workItemMountingText : ""}
      </TableCell>
      <TableCell sx={{ color: "primary.main" }} data-testid="draft-table-cell-supplements">
        {draftWorkItem.supplementOperations && draftWorkItem.supplementOperations.length > 0 && (
          <Chip
            data-testid="draft-table-cell-supplements-chip-operation"
            sx={{ mb: "5px", mr: "5px", bgcolor: costumPalette.operationSupplementBg, color: costumPalette.operationSupplementColor }}
            label={t("content.measurements.table.cellValue.operationSupplement", {
              value: draftWorkItem.supplementOperations.length,
            })}
          />
        )}
        {draftWorkItem.supplements && draftWorkItem.supplements.length > 0 && (
          <Chip
            data-testid="draft-table-cell-supplements-chip-supplement"
            sx={{ mb: "5px", bgcolor: costumPalette.supplementBg, color: costumPalette.supplementColor }}
            label={t("content.measurements.table.cellValue.supplement")}
          />
        )}
      </TableCell>

      <TableCell sx={{ color: "primary.main", textAlign: "right", pr: 5 }} data-testid="draft-table-cell-amount">
        {formatNumberToDkNumber(draftWorkItem.workItemAmount)}
      </TableCell>
      {/* KTH 14/4-23 hide until we have units <TableCell sx={{ color: "primary.main", textAlign: "right" }} data-testid="draft-table-cell-unit">
        TEST
      </TableCell> */}
      <TableCell sx={{ color: "primary.main", textAlign: "left", pl: 10 }} data-testid="draft-table-cell-note">
        {draftWorkItem.note && draftWorkItem.note.length > 0 && draftWorkItem.note}
      </TableCell>
    </TableRow>
  );
};
