import { useState, useEffect, Fragment, useCallback, useMemo } from "react";
import { useTranslation } from "react-i18next";

import Box from "@mui/material/Box";
import Checkbox from "@mui/material/Checkbox";
import IconButton from "@mui/material/IconButton";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import Paper from "@mui/material/Paper";
import styled from "@mui/system/styled";
import ContentCopyOutlinedIcon from "@mui/icons-material/ContentCopyOutlined";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";
import DriveFileMoveOutlinedIcon from "@mui/icons-material/DriveFileMoveOutlined";

import { GetProjectResponse, WorkItemResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { DesktopWorkItemMaterialTableRow } from "./desktop-work-item-material-row";
import { sortCompareNumber, sortCompareString } from "utils/compares";
import { getHmsFromMilliSeconds } from "utils/formats";
import { DesktopWorkItemOperationTableRow } from "./desktop-work-item-operation-row";
import { FolderLockedButton } from "components/page/folder/lock-folder/folder-locked-button";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { useMoveWorkitems } from "../../move/hooks/use-move";
import { useDeleteWorkitems } from "../../hooks/use-delete-workitem";
import { useCopyWorkItems } from "../../copy/hooks/use-copy";
import { useOrder } from "shared/sorting/use-order";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder;
  workItems?: WorkItemResponse[];
  dataFlatlist: ExtendedProjectFolder[];
}

export type WorkItemSortableId =
  | "material"
  | "moutingCode"
  | "operationSupplement"
  | "amount"
  | "unit"
  | "operationTime"
  | "operationTotalTime"
  | "payment";

export interface TableHeader {
  id: WorkItemSortableId;
  title: string;
  sortable: boolean;
  testid: string;
  alignment: "right" | "left";
}

export const DesktopFolderWorkItems = (props: Props) => {
  const { workItems, project, selectedFolder, dataFlatlist } = props;
  const { t } = useTranslation();
  const { canSelectWorkItem, canDeleteWorkitems, canMoveWorkitems, canCopyWorkitems } = useWorkItemRestrictions(project);
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [sortedWorkItems, setSortedWorkItems] = useState<WorkItemResponse[]>(workItems ?? []);
  const [checkedAll, setCheckedAll] = useState(false);
  const moveWorkitemsDialog = useMoveWorkitems({ project, folder: selectedFolder, dataFlatlist, selectedWorkitemIds: selectedIds });
  const copyWorkItemsDialog = useCopyWorkItems({ project, dataFlatlist, selectedFolder, selectedIds });
  const openDeleteMeasurementDialog = useDeleteWorkitems({ project, folder: selectedFolder, selectedIds });
  const { direction, orderBy, getLabelProps } = useOrder<WorkItemSortableId>("material");

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

  useEffect(() => {
    if (workItems && workItems.length > 0) {
      setSortedWorkItems(workItems);
    } else {
      setSortedWorkItems([]);
    }
  }, [workItems]);

  const headerConfig: TableHeader[] = [
    {
      id: "material",
      title: t("content.measurements.table.workItem"),
      sortable: true,
      testid: "measurements-table-header-sorting-material",
      alignment: "left",
    },
    {
      id: "moutingCode",
      title: t("content.measurements.table.moutingCode"),
      sortable: false,
      testid: "measurements-table-header-sorting-mountingcode",
      alignment: "left",
    },
    {
      id: "operationSupplement",
      title: t("content.measurements.table.operationSupplement"),
      sortable: false,
      testid: "measurements-table-header-sorting-supplements",
      alignment: "left",
    },
    // KTH 2/1-23 hide for better calculation { id: "operationTime", title: t("content.measurements.table.operationTime"), sortable: false, testid: "measurements-table-header-sorting-operationtime" },
    {
      id: "amount",
      title: t("content.measurements.table.amount"),
      sortable: true,
      testid: "measurements-table-header-sorting-amount",
      alignment: "right",
    },
    // KTH 14/4-23 hide until we have units { id: "unit", title: t("content.measurements.table.unit"), sortable: false, testid: "measurements-table-header-sorting-unit" },
    {
      id: "operationTotalTime",
      title: t("content.measurements.table.operationTotalTime"),
      sortable: true,
      testid: "measurements-table-header-sorting-total-operationtime",
      alignment: "right",
    },
    {
      id: "payment",
      title: t("content.measurements.table.payment"),
      sortable: true,
      testid: "measurements-table-header-sorting-total-payment",
      alignment: "right",
    },
  ];

  const sortedData = useMemo(() => {
    var sortedList = sortedWorkItems;

    switch (orderBy) {
      case "material":
        sortedList = [...sortedList].sort((a, b) => sortCompareString(direction, a?.workItemText, b?.workItemText));
        break;
      case "amount":
        sortedList = [...sortedList].sort((a, b) => sortCompareNumber(direction, a?.workItemAmount, b?.workItemAmount));
        break;
      case "operationTotalTime":
        sortedList = [...sortedList].sort((a, b) =>
          sortCompareString(
            direction,
            getHmsFromMilliSeconds(a.workItemTotalOperationTimeMilliseconds),
            getHmsFromMilliSeconds(b.workItemTotalOperationTimeMilliseconds)
          )
        );
        break;
      case "payment":
        sortedList = [...sortedList].sort((a, b) => sortCompareNumber(direction, a.workItemTotalPaymentDkr, b.workItemTotalPaymentDkr));
        break;
    }

    return sortedList;
  }, [sortedWorkItems, orderBy, direction]);

  const NoWorkItems = styled(Box)`
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    fontstyle: italic;
  `;

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%" }}>
      {workItems && sortedWorkItems.length > 0 && (
        <Box>
          <Box sx={{ display: "flex", justifyContent: "flex-end", p: 2, gap: 2 }}>
            {selectedFolder && <FolderLockedButton project={project} folder={selectedFolder} showText={true} />}
            <TextIcon translateText="common.move">
              <IconButton disabled={!canMoveWorkitems(selectedFolder, selectedIds)} onClick={moveWorkitemsDialog}>
                <DriveFileMoveOutlinedIcon />
              </IconButton>
            </TextIcon>
            <TextIcon translateText="common.copyTo">
              <IconButton disabled={!canCopyWorkitems(selectedFolder, selectedIds)} onClick={copyWorkItemsDialog}>
                <ContentCopyOutlinedIcon />
              </IconButton>
            </TextIcon>
            <TextIcon translateText="common.delete">
              <IconButton
                data-testid="measurement-table-delete-row"
                disabled={!canDeleteWorkitems(selectedFolder, selectedIds)}
                onClick={openDeleteMeasurementDialog}
              >
                <DeleteOutlinedIcon />
              </IconButton>
            </TextIcon>
          </Box>
          <TableContainer sx={{ height: "calc(100vh - 220px)", overflowY: "auto", pb: 16 }} component={Paper}>
            <Table stickyHeader>
              <TableHead sx={{ backgroundColor: "primary.light" }}>
                <TableRow>
                  <TableCell sx={{ backgroundColor: "primary.light", width: 10 }}>
                    <Checkbox
                      data-testid="measurement-table-checkbox-all"
                      onChange={(event) => onCheckAll(event.target.checked)}
                      checked={selectedIds.size === workItems.length}
                      disabled={!canSelectWorkItem(selectedFolder)}
                    />
                  </TableCell>
                  {headerConfig.map((cell, index) => {
                    if (cell.sortable) {
                      return (
                        <TableCell
                          data-testid={cell.testid}
                          key={index}
                          sx={{ color: "primary.main", backgroundColor: "primary.light", textAlign: cell.alignment }}
                        >
                          <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                        </TableCell>
                      );
                    }

                    return (
                      <TableCell
                        data-testid={cell.testid}
                        key={index}
                        sx={{ color: "primary.main", backgroundColor: "primary.light", textAlign: cell.alignment }}
                      >
                        {cell.title}
                      </TableCell>
                    );
                  })}
                </TableRow>
              </TableHead>
              <TableBody>
                {sortedData.map((workItem, index) => {
                  return workItem.workItemType === "Material" ? (
                    <DesktopWorkItemMaterialTableRow
                      key={index}
                      project={project}
                      folder={selectedFolder}
                      workItem={workItem}
                      onCheck={onCheck}
                      checked={isWorkItemChecked(workItem.workItemId)}
                    />
                  ) : (
                    <DesktopWorkItemOperationTableRow
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
        </Box>
      )}
      {workItems && sortedWorkItems.length <= 0 && (
        <Fragment>
          <Box sx={{ display: "flex", justifyContent: "flex-end", mb: 0.5, gap: 2 }}>
            {selectedFolder && <FolderLockedButton project={project} folder={selectedFolder} showText={true} />}
          </Box>
          <NoWorkItems color={"grey.100"}>{t("content.measurements.noWorkItemsIsRegistred")}</NoWorkItems>
        </Fragment>
      )}
    </Box>
  );
};
