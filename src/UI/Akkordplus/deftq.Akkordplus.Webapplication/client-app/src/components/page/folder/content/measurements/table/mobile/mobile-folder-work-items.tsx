import Box from "@mui/material/Box";
import Checkbox from "@mui/material/Checkbox";
import Paper from "@mui/material/Paper";
import styled from "@mui/system/styled";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import IconButton from "@mui/material/IconButton";
import { Fragment, useCallback, useState } from "react";
import { useTranslation } from "react-i18next";
import ContentCopyOutlinedIcon from "@mui/icons-material/ContentCopyOutlined";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";
import DriveFileMoveOutlinedIcon from "@mui/icons-material/DriveFileMoveOutlined";

import { GetProjectResponse, WorkItemResponse } from "api/generatedApi";
import { MobileWorkItemMaterialTableRow } from "./mobile-work-item-material-row";
import { MobileWorkItemOperationTableRow } from "./mobile-work-item-operation-row";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { FolderLockedButton } from "components/page/folder/lock-folder/folder-locked-button";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { useDeleteWorkitems } from "../../hooks/use-delete-workitem";
import { useMoveWorkitems } from "../../move/hooks/use-move";
import { useCopyWorkItems } from "../../copy/hooks/use-copy";
import { TableCellStyled } from "./mobile-work-item-row-styles";

interface Prop {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder;
  workItems?: WorkItemResponse[];
  dataFlatlist: ExtendedProjectFolder[];
}

export const MobileFolderWorkItems = (props: Prop) => {
  const { selectedFolder, workItems, project, dataFlatlist } = props;
  const { t } = useTranslation();
  const { canSelectWorkItem, canDeleteWorkitems, canMoveWorkitems, canCopyWorkitems } = useWorkItemRestrictions(project);
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [checkedAll, setCheckedAll] = useState(false);
  const openDeleteMeasurementDialog = useDeleteWorkitems({ project, folder: selectedFolder, selectedIds });
  const moveWorkitemsDialog = useMoveWorkitems({ project, folder: selectedFolder, dataFlatlist, selectedWorkitemIds: selectedIds });
  const copyWorkItemsDialog = useCopyWorkItems({ project, dataFlatlist, selectedFolder, selectedIds });

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
      workItems?.forEach((item) => ids.add(item.workItemId ?? ""));
    }
    setSelectedIds(ids);
    setCheckedAll(!checkedAll);
  };

  const isWorkItemChecked = (id: string | undefined): boolean => {
    return selectedIds.has(id ?? "");
  };

  const deleteSelectedItems = () => {
    // TODO: call confirmation dialog
    // When confirm button is tapped in confirmation dialog:
    // when success, invalidate folder and clear selected ids
    // if failure, give information dialog to user
    openDeleteMeasurementDialog();
  };

  const NoWorkItems = styled(Box)`
    width: 100%;
    height: 65%;
    display: flex;
    justify-content: center;
    align-items: center;
    fontstyle: italic;
  `;

  return (
    <Box sx={{ flex: 1, flexDirection: "column", overflowY: "hidden", display: "flex", width: "100%", height: "100%" }}>
      {workItems && workItems.length > 0 && (
        <Fragment>
          <Box sx={{ display: "flex", justifyContent: "flex-end", mb: 0.5, gap: 0.5 }}>
            {selectedFolder && <FolderLockedButton project={project} folder={selectedFolder} showText={false} />}
            <TextIcon>
              <IconButton disabled={!canMoveWorkitems(selectedFolder, selectedIds)} onClick={moveWorkitemsDialog}>
                <DriveFileMoveOutlinedIcon />
              </IconButton>
            </TextIcon>
            <TextIcon>
              <IconButton disabled={!canCopyWorkitems(selectedFolder, selectedIds)} onClick={copyWorkItemsDialog}>
                <ContentCopyOutlinedIcon />
              </IconButton>
            </TextIcon>
            <TextIcon>
              <IconButton
                data-testid="mobile-measurement-table-delete-row"
                disabled={!canDeleteWorkitems(selectedFolder, selectedIds)}
                onClick={deleteSelectedItems}
              >
                <DeleteOutlinedIcon />
              </IconButton>
            </TextIcon>
          </Box>

          <TableContainer sx={{ height: "100%", overflowY: "auto", pb: "70px" }} component={Paper}>
            <Table sx={{ tableLayout: "fixed" }}>
              <TableHead sx={{ backgroundColor: "primary.light" }}>
                <TableRow>
                  <TableCell sx={{ width: "40px", p: 0, color: "primary.main" }}>
                    <Checkbox
                      data-testid="mobile-measurement-table-checkbox-all"
                      onChange={(event) => onCheckAll(event.target.checked)}
                      checked={selectedIds.size === workItems.length}
                      disabled={!canSelectWorkItem(selectedFolder)}
                    />
                  </TableCell>

                  <TableCellStyled sx={{ width: "auto", pr: 1, color: "primary.main" }}>
                    {t("content.measurements.table.workItemSmall")}
                  </TableCellStyled>
                  <TableCellStyled sx={{ width: "50px", pl: 0, pr: 0, textAlign: "center", color: "primary.main" }}>
                    {t("content.measurements.table.amount")}
                  </TableCellStyled>
                  <TableCellStyled sx={{ width: "65px", p: 0, textAlign: "right", pr: 2, color: "primary.main" }}>
                    {t("content.measurements.table.payment")}
                  </TableCellStyled>
                </TableRow>
              </TableHead>
              <TableBody>
                {workItems.map((workItem, index) => {
                  return workItem.workItemType === "Material" ? (
                    <MobileWorkItemMaterialTableRow
                      key={index}
                      project={project}
                      folder={selectedFolder}
                      workItem={workItem}
                      onCheck={onCheck}
                      checked={isWorkItemChecked(workItem.workItemId)}
                    />
                  ) : (
                    <MobileWorkItemOperationTableRow
                      key={index}
                      project={project}
                      folder={selectedFolder}
                      workItem={workItem}
                      onCheck={onCheck}
                      checked={isWorkItemChecked(workItem.workItemId)}
                    />
                  );
                })}
              </TableBody>
            </Table>
          </TableContainer>
        </Fragment>
      )}
      {workItems && workItems.length <= 0 && (
        <Fragment>
          <Box sx={{ display: "flex", justifyContent: "flex-end", gap: 0.5 }}>
            {selectedFolder && <FolderLockedButton project={project} folder={selectedFolder} showText={true} />}
          </Box>
          <NoWorkItems color={"grey.100"}>{t("content.measurements.noWorkItemsIsRegistred")}</NoWorkItems>
        </Fragment>
      )}
    </Box>
  );
};
