import { useMemo } from "react";
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
import { CompensationResponse, GetProjectResponse } from "api/generatedApi";
import { useOrder } from "shared/sorting/use-order";
import { MobileCompensationPaymentRow } from "./mobile-compensation-payment-row";
import { sortCompareDateString, sortCompareNumber } from "utils/compares";

export type CompensationPaymentSortableId = "Period" | "Users" | "Amount";

export interface TableHeaderCompensationPayment {
  id: CompensationPaymentSortableId;
  title: string;
  testid: string;
  alignment: "right" | "left";
}

interface Props {
  project: GetProjectResponse;
  compensations: CompensationResponse[];
}

export const MobileCompensationPayments = (props: Props) => {
  const { compensations } = props;
  const { t } = useTranslation();
  const { direction, orderBy, getLabelProps } = useOrder<CompensationPaymentSortableId>("Period");

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
      testid: "dashboard-compensation-payments-header-period",
      alignment: "left",
    },
    {
      id: "Amount",
      title: t("dashboard.compensationPayments.table.amount"),
      testid: "dashboard-compensation-payments-header-amount",
      alignment: "right",
    },
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%", pl: 1, pr: 1 }}>
      <Box sx={{ display: "flex" }}>
        <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start", flexGrow: 1 }}>
          <Typography variant="body1" color="primary.main" sx={{ pt: 2, pb: 2 }}>
            {t("dashboard.compensationPayments.description")}
          </Typography>
        </Box>
      </Box>
      <TableContainer sx={{ height: "calc(100vh - 240px)", overflowY: "auto", pb: "50px" }} component={Paper}>
        <Table stickyHeader>
          <TableHead sx={{ backgroundColor: "primary.light" }}>
            <TableRow>
              {headerConfig.map((cell, index) => {
                return (
                  <TableCell
                    data-testid={cell.testid}
                    key={cell.id}
                    sx={{ color: "primary.main", backgroundColor: "primary.light" }}
                    align={cell.alignment}
                  >
                    <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                  </TableCell>
                );
              })}
            </TableRow>
          </TableHead>
          <TableBody>
            {sortedData.map((compensation, index) => {
              return <MobileCompensationPaymentRow key={index} row={compensation} />;
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
