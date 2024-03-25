import { useCallback, useMemo, useState } from "react";
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
import ContentCopyOutlinedIcon from "@mui/icons-material/ContentCopyOutlined";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutline";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useOrder } from "shared/sorting/use-order";
import { sortCompareNumber, sortCompareString } from "utils/compares";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { DraftWorkItem } from "utils/offline-storage/use-offline-storage";
import { DesktopDraftRowMaterial } from "../rows/desktop/desktop-draft-row-material";
import { DesktopDraftRowOperation } from "../rows/desktop/desktop-draft-row-operation";
import { useDeleteDraftItem } from "../delete/hook/use-delete-draft-item";
import { GetProjectResponse } from "api/generatedApi";
import { useCopyDraftWorkItemToFolder } from "../move/hooks/use-copy-draft-work-items-to-folder";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";

interface Props {
  project: GetProjectResponse;
  foldersFlatlist: ExtendedProjectFolder[];
  draftWorkItems: DraftWorkItem[];
  updateTable: () => void;
}

export type DraftWorktemSortableId = "material" | "moutingCode" | "operationSupplement" | "amount" | "unit" | "note";

interface TableHeader {
  id: DraftWorktemSortableId;
  title: string;
  sortable: boolean;
  testid: string;
  alignment: "right" | "left";
}

export const DraftDesktop = ({ project, foldersFlatlist, draftWorkItems, updateTable }: Props) => {
  const { t } = useTranslation();
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const isOnline = useOnlineStatus();
  const [checkedAll, setCheckedAll] = useState(false);
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

  const headerConfig: TableHeader[] = [
    {
      id: "material",
      title: t("content.measurements.table.workItem"),
      sortable: true,
      testid: "draft-table-header-sorting-material",
      alignment: "left",
    },
    {
      id: "moutingCode",
      title: t("content.measurements.table.moutingCode"),
      sortable: false,
      testid: "draft-table-header-sorting-mountingcode",
      alignment: "left",
    },
    {
      id: "operationSupplement",
      title: t("content.measurements.table.operationSupplement"),
      sortable: false,
      testid: "draft-table-header-sorting-supplements",
      alignment: "left",
    },
    { id: "amount", title: t("content.measurements.table.amount"), sortable: true, testid: "draft-table-header-sorting-amount", alignment: "right" },
    // KTH 14/4-23 hide until we have units { id: "unit", title: t("content.measurements.table.unit"), sortable: false, testid: "draft-table-header-sorting-unit" },
    { id: "note", title: t("common.note"), sortable: false, testid: "draft-table-header-sorting-note", alignment: "left" },
  ];

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
      case "note":
        sortedList = [...sortedList].sort((a, b) => sortCompareString(direction, a?.note ?? " ", b?.note ?? " "));
        break;
    }

    return sortedList;
  }, [draftWorkItems, orderBy, direction]);

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%", pl: 2, pr: 2 }}>
      <Box sx={{ display: "flex" }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            justifyContent: "flex-start",
            flexGrow: 1,
            padding: "32px 0px 0px 0px",
          }}
        >
          <Typography variant="h5" color="primary.dark" sx={{ pt: 1 }}>
            {t("draft.title")}
          </Typography>
          <Typography variant="body1" color="grey.50" sx={{ pt: 2 }}>
            {t("draft.description")}
          </Typography>
        </Box>
      </Box>
      <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%" }}>
        {draftWorkItems.length > 0 && (
          <Box>
            <Box sx={{ display: "flex", justifyContent: "flex-end", pb: 2, gap: 2 }}>
              <TextIcon translateText="common.copyTo">
                <IconButton disabled={!isOnline || selectedIds.size === 0} onClick={copyDraftWorkItemsToFolder}>
                  <ContentCopyOutlinedIcon />
                </IconButton>
              </TextIcon>
              <TextIcon translateText="common.delete">
                <IconButton data-testid="draft-select-draft-item" disabled={selectedIds.size === 0} onClick={handleDeleteItem}>
                  <DeleteOutlinedIcon />
                </IconButton>
              </TextIcon>
            </Box>
            <TableContainer sx={{ height: "calc(100vh - 240px)", overflowY: "auto", pb: 10 }} component={Paper}>
              <Table stickyHeader>
                <TableHead sx={{ backgroundColor: "primary.light" }}>
                  <TableRow>
                    <TableCell sx={{ backgroundColor: "primary.light", width: 10 }}>
                      <Checkbox
                        data-testid="measurement-table-checkbox-all"
                        onChange={(event) => onCheckAll(event.target.checked)}
                        checked={selectedIds.size === draftWorkItems.length}
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
                      } else {
                        if (cell.id === "note") {
                          return (
                            <TableCell data-testid={cell.testid} key={index} sx={{ color: "primary.main", backgroundColor: "primary.light", pl: 10 }}>
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
                      }
                    })}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {sortedData.map((draftWorkItem, index) => {
                    return draftWorkItem.workItemType === "Material" ? (
                      <DesktopDraftRowMaterial
                        key={index}
                        draftWorkItem={draftWorkItem}
                        onCheck={onCheck}
                        checked={isWorkItemChecked(draftWorkItem.draftId)}
                      />
                    ) : (
                      <DesktopDraftRowOperation
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
        )}
        {draftWorkItems.length <= 0 && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <Typography variant="body1" color="grey.100" fontStyle={"italic"}>
              {t("content.measurements.noWorkItemsIsRegistred")}
            </Typography>
          </Box>
        )}
      </Box>
    </Box>
  );
};
