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
import { GetProjectSpecificOperationResponse, GetProjectResponse } from "api/generatedApi";
import { useOrder } from "shared/sorting/use-order";
import { sortCompareNumber, sortCompareString } from "utils/compares";
import { MobileProjectSpecificOperationRow } from "./mobile-project-specific-operation-row";

export type ProjectSpecificOperationSortableId = "Name" | "Payment";

export interface TableHeaderProjectSpecificOperation {
  id: ProjectSpecificOperationSortableId;
  title: string;
  testid: string;
  alignment: "right" | "left";
}

interface Props {
  project: GetProjectResponse;
  compensations: GetProjectSpecificOperationResponse[];
}

export const MobileProjectSpecificOperations = (props: Props) => {
  const { compensations } = props;
  const { t } = useTranslation();
  const { direction, orderBy, getLabelProps } = useOrder<ProjectSpecificOperationSortableId>("Name");

  const sortedData = useMemo(() => {
    var sortedList = compensations;

    switch (orderBy) {
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
      id: "Name",
      title: t("common.name"),
      testid: "dashboard-project-specific-operations-header-name",
      alignment: "left",
    },
    {
      id: "Payment",
      title: t("common.currency"),
      testid: "dashboard-project-specific-operations-header-currency",
      alignment: "right",
    },
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%", pl: 1, pr: 1 }}>
      <Box sx={{ display: "flex" }}>
        <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start", flexGrow: 1 }}>
          <Typography variant="body1" color="primary.main" sx={{ pt: 2, pb: 2 }}>
            {t("dashboard.projectSpecificOperations.description")}
          </Typography>
        </Box>
      </Box>
      <TableContainer sx={{ height: "calc(100vh - 240px)", overflowY: "auto", pb: "50px" }} component={Paper}>
        <Table stickyHeader>
          <TableHead sx={{ backgroundColor: "primary.light" }}>
            <TableRow>
              {headerConfig.map((cell) => {
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
            {sortedData.map((operation) => {
              return <MobileProjectSpecificOperationRow key={operation.projectSpecificOperationId} row={operation} />;
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
