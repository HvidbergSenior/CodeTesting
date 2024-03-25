import { Box, Checkbox, Chip, TableCell, TableRow, Typography } from "@mui/material";
import { Fragment } from "react";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { TableCellStyled } from "shared/styles/table-cell-styled";
import { Typography2LinesStyled } from "shared/styles/typography-styled";
import { costumPalette } from "theme/palette";
import { formatNumberToAmount } from "utils/formats";
import { DraftWorkItem } from "utils/offline-storage/use-offline-storage";
import { ShowDraftInfoDialog } from "../../info/show-draft-info-dialog";

interface Prop {
  draftWorkItem: DraftWorkItem;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export const MobileDraftRowOperation = (props: Prop) => {
  const { draftWorkItem, onCheck, checked } = props;
  const { t } = useTranslation();

  const [openInfoDialog] = useDialog(ShowDraftInfoDialog);

  const showDraftInfo = () => {
    openInfoDialog({ draft: draftWorkItem });
  };

  const borderBottomColor = draftWorkItem.note ? "transparent" : costumPalette.gray;
  const paddingBottom = draftWorkItem.note ? "4px" : "16px";

  return (
    <Fragment>
      <TableRow sx={{ cursor: "pointer" }} onClick={showDraftInfo}>
        <TableCell sx={{ p: 0, pt: 0.5, verticalAlign: "top", borderBottomColor: borderBottomColor }}>
          <Checkbox
            data-testid="mobile-draft-table-operation-row-checkbox"
            onChange={(event) => onCheck(draftWorkItem.draftId)}
            onClick={(event) => event.stopPropagation()}
            checked={checked}
          />
        </TableCell>
        <TableCellStyled
          title={draftWorkItem.operation?.operationText ?? ""}
          sx={{ pr: 1, pt: 2, pb: paddingBottom, verticalAlign: "top", borderBottomColor: borderBottomColor }}
        >
          <Box>
            <Typography2LinesStyled variant="body2" color={"primary.main"}>
              {draftWorkItem.operation?.operationText}
            </Typography2LinesStyled>
            <Box sx={{ display: "flex", flexDirection: "row" }}>
              {draftWorkItem.supplements && draftWorkItem.supplements.length > 0 && (
                <Chip
                  size="small"
                  label={t("content.measurements.table.cellValue.supplementSmall")}
                  sx={{ bgcolor: costumPalette.supplementBg, color: costumPalette.supplementColor, marginRight: 0.5 }}
                />
              )}
            </Box>
          </Box>
        </TableCellStyled>
        <TableCell sx={{ pl: 0, pr: 0, textAlign: "center", verticalAlign: "top", pb: paddingBottom, borderBottomColor: borderBottomColor }}>
          <Box sx={{ display: "flex", flexDirection: "column" }} pr={3}>
            <Typography variant="body2" color={"primary.main"} textAlign={"right"}>
              {formatNumberToAmount(draftWorkItem.workItemAmount)}
            </Typography>
            {/* KTH 14/4-23 hide until we have units <Typography variant="body2" color={"grey.50"} textAlign={"right"}>
              TEST
            </Typography> */}
          </Box>
        </TableCell>
      </TableRow>
      {draftWorkItem.note && (
        <TableRow>
          <TableCell></TableCell>
          <TableCell colSpan={2} sx={{ padding: "0px 16px 16px 16px" }}>
            {draftWorkItem.note && draftWorkItem.note.length > 0 && (
              <Chip
                size="medium"
                label={draftWorkItem.note}
                sx={{
                  padding: "8px 2px 8px 2px",
                  bgcolor: "primary.light",
                  color: costumPalette.supplementColor,
                  marginRight: 0.5,
                  height: "auto",
                  "& .MuiChip-label": {
                    display: "block",
                    whiteSpace: "normal",
                  },
                }}
              />
            )}
          </TableCell>
        </TableRow>
      )}
    </Fragment>
  );
};
