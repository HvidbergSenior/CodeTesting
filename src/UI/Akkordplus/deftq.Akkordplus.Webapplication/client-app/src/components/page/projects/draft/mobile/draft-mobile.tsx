import { Fragment, useCallback, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Checkbox from "@mui/material/Checkbox";
import IconButton from "@mui/material/IconButton";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import Typography from "@mui/material/Typography";
import styled from "@mui/system/styled";
import ContentCopyOutlinedIcon from "@mui/icons-material/ContentCopyOutlined";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutline";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useOrder } from "shared/sorting/use-order";
import { TableCellStyled } from "shared/styles/table-cell-styled";
import { sortCompareNumber, sortCompareString } from "utils/compares";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { DraftWorkItem } from "utils/offline-storage/use-offline-storage";
import { MobileDraftRowMaterial } from "../rows/mobile/mobile-draft-row-material";
import { MobileDraftRowOperation } from "../rows/mobile/mobile-draft-row-operation";
import { GetProjectResponse } from "api/generatedApi";
import { useDeleteDraftItem } from "../delete/hook/use-delete-draft-item";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useCopyDraftWorkItemToFolder } from "../move/hooks/use-copy-draft-work-items-to-folder";

interface Prop {
  project: GetProjectResponse;
  foldersFlatlist: ExtendedProjectFolder[];
  draftWorkItems: DraftWorkItem[];
  updateTable: () => void;
}

export type DraftWorktemSortableId = "material" | "moutingCode" | "operationSupplement" | "amount" | "unit" | "note";

export const DraftMobile = (props: Prop) => {
  const { project, foldersFlatlist, draftWorkItems, updateTable } = props;
  const { t } = useTranslation();
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [checkedAll, setCheckedAll] = useState(false);
  const isOnline = useOnlineStatus();
  const { direction, orderBy, getLabelProps } = useOrder<DraftWorktemSortableId>("material");
  const openDeleteItemConfirmation = useDeleteDraftItem({ project: project, selectedIds: selectedIds, updateTable: updateTable });
  const copyDraftWorkItemsToFolder = useCopyDraftWorkItemToFolder({
    project,
    foldersFlatlist,
    selectedDraftIds: Array.from(selectedIds),
  });

  const onCheck = useCallback(
    (id: string | undefined) => {
      if (id === undefined) return;
      let ids = new Set(selectedIds);
      if (ids.has(id)) {
        ids.delete(id);
      } else {
        ids.add(id);
      }
      setSelectedIds(ids);
    },
    [selectedIds]
  );

  const onCheckAll = (checked: boolean) => {
    let ids = new Set<string>();
    if (checked) {
      draftWorkItems?.forEach((item) => ids.add(item.draftId ?? ""));
    }
    setSelectedIds(ids);
    setCheckedAll(!checkedAll);
  };

  const isWorkItemChecked = (id: string | undefined): boolean => {
    return selectedIds.has(id ?? "");
  };

  const handleDeleteItem = () => {
    openDeleteItemConfirmation();
  };

  const NoWorkItems = styled(Box)`
    width: 100%;
    height: 65%;
    display: flex;
    justify-content: center;
    align-items: center;
    fontstyle: italic;
  `;

  const sortedData = useMemo(() => {
    var sortedList = draftWorkItems;

    switch (orderBy) {
      case "material":
        sortedList = [...sortedList].sort((a, b) =>
          sortCompareString(
            direction,
            a.workItemType === "Material" ? a.material?.name : a?.operation?.operationText,
            b.workItemType === "Material" ? b?.material?.name : b.operation?.operationText
          )
        );
        break;
      case "amount":
        sortedList = [...sortedList].sort((a, b) => sortCompareNumber(direction, a?.workItemAmount, b?.workItemAmount));
        break;
    }

    return sortedList;
  }, [draftWorkItems, orderBy, direction]);

  return (
    <Box sx={{ flex: 1, flexDirection: "column", overflowY: "scroll", display: "flex", width: "100%", height: "100%" }}>
      <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start", flexGrow: 1, padding: "20px 10px 0px 20px" }}>
        <Typography variant="h5" color="primary.dark">
          {t("draft.title")}
        </Typography>
        <Typography variant="body1" color="grey.50" sx={{ pt: 2 }}>
          {t("draft.description")}
        </Typography>
      </Box>
      {draftWorkItems && draftWorkItems.length > 0 && (
        <Fragment>
          <Box sx={{ display: "flex", justifyContent: "flex-end", mb: 0.5, gap: 0.5 }}>
            <TextIcon>
              <IconButton disabled={!isOnline || selectedIds.size === 0} onClick={copyDraftWorkItemsToFolder}>
                <ContentCopyOutlinedIcon />
              </IconButton>
            </TextIcon>
            <TextIcon>
              <IconButton data-testid="mobile-measurement-table-delete-row" disabled={selectedIds.size === 0} onClick={handleDeleteItem}>
                <DeleteOutlinedIcon />
              </IconButton>
            </TextIcon>
          </Box>
          <Box sx={{ height: "100%", width: "100%", padding: "0px 10px 20px 20px" }}>
            <TableContainer sx={{ overflowY: "auto", pb: "70px" }} component={Paper}>
              <Table sx={{ tableLayout: "fixed" }}>
                <TableHead sx={{ backgroundColor: "primary.light" }}>
                  <TableRow>
                    <TableCell sx={{ width: "40px", p: 0, color: "primary.main" }}>
                      <Checkbox
                        data-testid="mobile-measurement-table-checkbox-all"
                        onChange={(event) => onCheckAll(event.target.checked)}
                        checked={selectedIds.size === draftWorkItems.length}
                      />
                    </TableCell>

                    <TableCellStyled sx={{ width: "auto", pr: 1, color: "primary.main" }}>
                      <TableSortLabel {...getLabelProps("material")}>{t("content.measurements.table.workItemSmall")}</TableSortLabel>
                    </TableCellStyled>
                    <TableCellStyled sx={{ width: "100px", pl: 5, textAlign: "center", color: "primary.main" }}>
                      <TableSortLabel {...getLabelProps("amount")}> {t("content.measurements.table.amount")}</TableSortLabel>
                    </TableCellStyled>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {sortedData.map((draftWorkItem, index) => {
                    return draftWorkItem.workItemType === "Material" ? (
                      <MobileDraftRowMaterial
                        key={index}
                        draftWorkItem={draftWorkItem}
                        onCheck={onCheck}
                        checked={isWorkItemChecked(draftWorkItem.draftId)}
                      />
                    ) : (
                      <MobileDraftRowOperation
                        key={index}
                        draftWorkItem={draftWorkItem}
                        onCheck={onCheck}
                        checked={isWorkItemChecked(draftWorkItem.draftId)}
                      />
                    );
                  })}
                </TableBody>
              </Table>
            </TableContainer>
          </Box>
        </Fragment>
      )}
      {draftWorkItems && draftWorkItems.length <= 0 && (
        <NoWorkItems sx={{ paddingBottom: 20 }} color={"grey.100"}>
          {t("content.measurements.noWorkItemsIsRegistred")}
        </NoWorkItems>
      )}
    </Box>
  );
};
