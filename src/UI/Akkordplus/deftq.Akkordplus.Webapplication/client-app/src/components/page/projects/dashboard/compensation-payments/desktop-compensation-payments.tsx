import { useState, useCallback, useMemo } from "react";
import { useTranslation } from "react-i18next";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import Box from "@mui/material/Box";
import TableContainer from "@mui/material/TableContainer";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import Checkbox from "@mui/material/Checkbox";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";
import { CompensationResponse, GetProjectResponse } from "api/generatedApi";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useOrder } from "shared/sorting/use-order";
import { DesktopCompensationPaymentRow } from "./desktop-compensation-payment-row";
import { sortCompareNumber, sortCompareDateString } from "utils/compares";
import { useDeleteCompensationPayments } from "./hooks/use-delete-compensation-payments";

export type CompensationPaymentSortableId = "Period" | "Users" | "Amount";

export interface TableHeaderCompensationPayment {
  id: CompensationPaymentSortableId;
  title: string;
  sortable: boolean;
  testid: string;
  alignment: "right" | "left";
}

interface Props {
  project: GetProjectResponse;
  compensations: CompensationResponse[];
}

export const DesktopCompensationPayments = (props: Props) => {
  const { project, compensations } = props;
  const { t } = useTranslation();
  const { canSelectCompensationPayments, canDeleteCompensationPayments } = useDashboardRestrictions(project);
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [checkedAll, setCheckedAll] = useState(false);
  const { direction, orderBy, getLabelProps } = useOrder<CompensationPaymentSortableId>("Period");
  const openDeleteCompensationPaymentsDialog = useDeleteCompensationPayments({ project, selectedIds });

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
      compensations?.forEach((item) => ids.add(item.projectCompensationId ?? ""));
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
      case "Amount":
        sortedList = [...compensations].sort((a, b) => sortCompareNumber(direction, a?.compensationPaymentDkr, b?.compensationPaymentDkr));
        break;
      case "Period":
        sortedList = [...compensations].sort((a, b) => sortCompareDateString(direction, a?.startDate, b?.startDate));
        break;
    }

    return sortedList;
  }, [compensations, orderBy, direction]);

  const headerConfig: TableHeaderCompensationPayment[] = [
    {
      id: "Period",
      title: t("dashboard.compensationPayments.table.period"),
      sortable: true,
      testid: "dashboard-compensation-payments-header-period",
      alignment: "left",
    },
    {
      id: "Users",
      title: t("dashboard.compensationPayments.table.users"),
      sortable: false,
      testid: "dashboard-compensation-payments-header-users",
      alignment: "left",
    },
    {
      id: "Amount",
      title: t("dashboard.compensationPayments.table.amount"),
      sortable: true,
      testid: "dashboard-compensation-payments-header-amount",
      alignment: "right",
    },
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%", pl: 4, pr: 4 }}>
      <Box sx={{ display: "flex" }}>
        <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start", flexGrow: 1 }}>
          <Typography variant="body1" color="primary.main" sx={{ pt: 2, pb: 2 }}>
            {t("dashboard.compensationPayments.description")}
          </Typography>
        </Box>

        <Box sx={{ display: "flex", justifyContent: "flex-end", p: 2 }}>
          <TextIcon translateText="common.delete">
            <IconButton
              data-testid="dashboard-compensation-payments-delete-rows"
              disabled={!canDeleteCompensationPayments(selectedIds)}
              onClick={openDeleteCompensationPaymentsDialog}
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
                  data-testid="dashboard-compensation-payments-table-checkbox-all"
                  onChange={(event) => onCheckAll(event.target.checked)}
                  checked={selectedIds.size === compensations?.length}
                  disabled={!canSelectCompensationPayments()}
                />
              </TableCell>
              {headerConfig.map((cell, index) => {
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
            {sortedData.map((compensation, index) => {
              return (
                <DesktopCompensationPaymentRow
                  key={index}
                  project={project}
                  row={compensation}
                  onCheck={onCheck}
                  checked={isCompensationPaymentChecked(compensation.projectCompensationId)}
                />
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
