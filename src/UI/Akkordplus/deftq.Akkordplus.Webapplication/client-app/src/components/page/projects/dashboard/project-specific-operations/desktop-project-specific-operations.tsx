import { useState, useCallback, useMemo } from "react";
import { useTranslation } from "react-i18next";
import Table from "@mui/material/Table";
import TableContainer from "@mui/material/TableContainer";
import Paper from "@mui/material/Paper";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import Checkbox from "@mui/material/Checkbox";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";
import { GetProjectSpecificOperationResponse, GetProjectResponse } from "api/generatedApi";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useOrder } from "shared/sorting/use-order";
import { sortCompareNumber, sortCompareString } from "utils/compares";
import { DesktopProjectSpecificOperationRow } from "./desktop-project-specific-operation-row";
import { useDeleteProjectSpecificOperations } from "./hooks/use-delete-projects-specific-operations";

export type ProjectSpecificOperationSortableId = "Agreement" | "Name" | "TimeType" | "Payment";

export interface TableHeaderProjectSpecificOperation {
  id: ProjectSpecificOperationSortableId;
  title: string;
  sortable: boolean;
  testid: string;
  alignment: "right" | "left";
}

interface Props {
  project: GetProjectResponse;
  compensations: GetProjectSpecificOperationResponse[];
}

export const DesktopProjectSpecificOperations = (props: Props) => {
  const { project, compensations } = props;
  const { t } = useTranslation();
  const { canSelectProjectSpecificOperations, canDeleteProjectSpecificOperations } = useDashboardRestrictions(project);
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [checkedAll, setCheckedAll] = useState(false);
  const { direction, orderBy, getLabelProps } = useOrder<ProjectSpecificOperationSortableId>("Agreement");
  const openDeletePeorjectsSpecificOperationsDialog = useDeleteProjectSpecificOperations({ project, selectedIds });

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
      compensations?.forEach((item) => ids.add(item.projectSpecificOperationId ?? ""));
    }
    setSelectedIds(ids);
    setCheckedAll(!checkedAll);
  };

  const isCompensationPaymentChecked = (id: string | undefined): boolean => {
    return selectedIds.has(id ?? "");
  };

  const sortedData = useMemo(() => {
    var sortedList = compensations;

    switch (orderBy) {
      case "Agreement":
        sortedList = [...compensations].sort((a, b) => sortCompareString(direction, a?.extraWorkAgreementNumber, b?.extraWorkAgreementNumber));
        break;
      case "Name":
        sortedList = [...compensations].sort((a, b) => sortCompareString(direction, a?.name, b?.name));
        break;
      case "Payment":
        sortedList = [...compensations].sort((a, b) => sortCompareNumber(direction, a?.payment, b?.payment));
        break;
    }

    return sortedList;
  }, [compensations, orderBy, direction]);

  const headerConfig: TableHeaderProjectSpecificOperation[] = [
    {
      id: "Agreement",
      title: t("dashboard.projectSpecificOperations.table.extraWorkAgrrementNumber"),
      sortable: true,
      testid: "dashboard-project-specific-operations-header-extra-work-agreement-number",
      alignment: "left",
    },
    {
      id: "Name",
      title: t("common.name"),
      sortable: true,
      testid: "dashboard-project-specific-operations-header-name",
      alignment: "left",
    },
    {
      id: "TimeType",
      title: t("common.time.label"),
      sortable: false,
      testid: "dashboard-project-specific-operations-header-time",
      alignment: "left",
    },
    {
      id: "Payment",
      title: t("common.currency"),
      sortable: true,
      testid: "dashboard-project-specific-operations-header-currency",
      alignment: "right",
    },
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%", pl: 4, pr: 4 }}>
      <Box sx={{ display: "flex" }}>
        <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start", flexGrow: 1 }}>
          <Typography variant="body1" color="primary.main" sx={{ pt: 2, pb: 2 }}>
            {t("dashboard.projectSpecificOperations.description")}
          </Typography>
        </Box>

        <Box sx={{ display: "flex", justifyContent: "flex-end", p: 2 }}>
          <TextIcon translateText="common.delete">
            <IconButton
              data-testid="dashboard-project-specific-operations-delete-rows"
              disabled={!canDeleteProjectSpecificOperations(selectedIds)}
              onClick={openDeletePeorjectsSpecificOperationsDialog}
            >
              <DeleteOutlinedIcon />
            </IconButton>
          </TextIcon>
        </Box>
      </Box>
      <TableContainer sx={{ height: "calc(100vh - 240px)", overflowY: "auto", pb: "50px" }} component={Paper}>
        <Table stickyHeader>
          <TableHead sx={{ backgroundColor: "primary.light" }}>
            <TableRow>
              <TableCell sx={{ backgroundColor: "primary.light", width: 10 }}>
                <Checkbox
                  data-testid="dashboard-project-specific-operations-table-checkbox-all"
                  onChange={(event) => onCheckAll(event.target.checked)}
                  checked={selectedIds.size === compensations?.length}
                  disabled={!canSelectProjectSpecificOperations()}
                />
              </TableCell>
              {headerConfig.map((cell) => {
                if (!cell.sortable) {
                  return (
                    <TableCell
                      data-testid={cell.testid}
                      key={cell.id}
                      sx={{ color: "primary.main", backgroundColor: "primary.light" }}
                      align={cell.alignment}
                    >
                      {cell.title}
                    </TableCell>
                  );
                } else {
                  return (
                    <TableCell
                      data-testid={cell.testid}
                      key={cell.id}
                      sx={{ color: "primary.main", backgroundColor: "primary.light", pr: 5 }}
                      align={cell.alignment}
                    >
                      <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                    </TableCell>
                  );
                }
              })}
            </TableRow>
          </TableHead>
          <TableBody>
            {sortedData.map((operation) => {
              return (
                <DesktopProjectSpecificOperationRow
                  key={operation.projectSpecificOperationId}
                  project={project}
                  row={operation}
                  onCheck={onCheck}
                  checked={isCompensationPaymentChecked(operation.projectSpecificOperationId)}
                />
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
