import { useState, useMemo } from "react";
import { useTranslation } from "react-i18next";
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
import { FoundOperation } from "api/generatedApi";
import { useOrder } from "shared/sorting/use-order";
import { sortCompareString } from "utils/compares";

export interface Props {
  searchResults: FoundOperation[];
  preselected: FoundOperation | undefined;
  selectedOperationProps: (operation: FoundOperation) => void;
}

export function SelectOperationTable(props: Props) {
  type OperationSortableId = "operationText" | "operationNumber";

  const { searchResults, preselected, selectedOperationProps } = props;
  const { t } = useTranslation();
  const [selectedOperation, setSelectedOperation] = useState<FoundOperation | undefined>(preselected);
  const { direction, orderBy, getLabelProps } = useOrder<OperationSortableId>("operationText");

  if (preselected) {
    selectedOperationProps(preselected);
  }

  const TableCellStyled = styled(TableCell)`
    white-space: nowrap;
    text-overflow: ellipsis;
    overflow: hidden;
  `;

  const TableCellTypographyStyled = styled(Typography)`
    white-space: break-spaces;
    text-overflow: ellipsis;
    overflow: hidden;
  `;

  const selectOperation = (event: React.MouseEvent<HTMLElement>, operation: FoundOperation) => {
    setSelectedOperation(operation);
    selectedOperationProps(operation);
  };

  const isOperationSelected = (operation: FoundOperation) => {
    return selectedOperation && selectedOperation.operationNumber === operation.operationNumber;
  };

  const sortedData = useMemo(() => {
    var sortedList = searchResults;

    switch (orderBy) {
      case "operationText":
        sortedList = [...sortedList].sort((a, b) => sortCompareString(direction, a.operationText?.trim(), b.operationText?.trim()));
        break;
    }

    return sortedList;
  }, [searchResults, orderBy, direction]);

  return (
    <TableContainer sx={{ height: "100%", overflowY: "auto" }} component={Paper}>
      <Table sx={{ tableLayout: "fixed" }}>
        <TableHead sx={{ backgroundColor: "primary.light" }}>
          <TableRow>
            <TableCellStyled sx={{ width: "auto", pr: 0, color: "primary.main" }}>
              <TableSortLabel {...getLabelProps("operationText")}>
                {t("content.measurements.create.selectOperationStep.table.operation")}
              </TableSortLabel>
            </TableCellStyled>
          </TableRow>
        </TableHead>
        <TableBody>
          {sortedData.map((operation, index) => {
            return (
              <TableRow
                data-testid={`create-workitem-operation-search-result-${operation.operationText}-line`}
                key={index}
                onClick={(event) => selectOperation(event, operation)}
                selected={isOperationSelected(operation)}
              >
                <TableCellStyled sx={{ pr: 0, color: "primary.main" }}>
                  <div>
                    <TableCellTypographyStyled
                      sx={{ color: "black" }}
                      data-testid={`create-workitem-operation-search-result-${operation.operationText}-name`}
                      variant="body2"
                      style={{ wordWrap: "break-word" }}
                    >
                      {operation.operationText}
                    </TableCellTypographyStyled>
                  </div>

                  <Typography data-testid={`create-workitem-operation-search-reesult-${operation.operationText}-number`} variant="caption">
                    {operation.operationNumber}
                  </Typography>
                </TableCellStyled>
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
