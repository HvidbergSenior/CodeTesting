import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import styled from "@mui/system/styled";
import { costumPalette } from "theme/palette";
import { GroupedWorkItemsResponse } from "api/generatedApi";
import { useMemo } from "react";
import { useTranslation } from "react-i18next";
import { useOrder } from "shared/sorting/use-order";
import { sortCompareNumber, sortCompareString } from "utils/compares";
import { GroupingWorkitemRow } from "./grouping-workitems-row";

interface Props {
  groupedWorkitems: GroupedWorkItemsResponse[];
  scroll: boolean;
  sorting: boolean;
}

export type WorkItemsGroupSortableId = "Text" | "Amount" | "PaymentDkr";

export interface TableHeaderGrouping {
  id: WorkItemsGroupSortableId;
  title: string;
  sortable: boolean;
  testid: string;
}

export const GroupingWorkitemsTable = ({ groupedWorkitems, scroll, sorting }: Props) => {
  const { t } = useTranslation();
  const { direction, orderBy, getLabelProps } = useOrder<WorkItemsGroupSortableId>("Text");

  const headerConfig: TableHeaderGrouping[] = [
    {
      id: "Text",
      title: t("content.overview.grouping.dialog.table.title"),
      sortable: sorting && true,
      testid: "overview-grouping-dialog-header-title",
    },
    {
      id: "Amount",
      title: t("content.overview.grouping.dialog.table.quantity"),
      sortable: sorting && true,
      testid: "overview-grouping-dialog-header-amount",
    },
    {
      id: "PaymentDkr",
      title: t("content.overview.grouping.dialog.table.payment"),
      sortable: sorting && true,
      testid: "overview-grouping-dialog-header-payment",
    },
  ];

  const sortedData = useMemo(() => {
    var sortedList = [...groupedWorkitems];

    switch (orderBy) {
      case "Text":
        sortedList = [...groupedWorkitems].sort((a, b) => sortCompareString(direction, a?.text, b?.text));
        break;
      case "Amount":
        sortedList = [...groupedWorkitems].sort((a, b) => sortCompareNumber(direction, a?.amount, b?.amount));
        break;
      case "PaymentDkr":
        sortedList = [...groupedWorkitems].sort((a, b) => sortCompareNumber(direction, a?.paymentDkr, b?.paymentDkr));
        break;
    }

    return sortedList;
  }, [groupedWorkitems, orderBy, direction]);

  const TableHeaderCell = styled(TableCell)({
    color: "primary.main",
    backgroundColor: `${costumPalette.primaryLight} !important`,
    WebkitPrintColorAdjust: "exact",
  });

  return (
    <TableContainer sx={{ maxHeight: !scroll ? "auto" : "700px", overflow: scroll && sortedData.length > 8 ? "scroll" : "hidden" }} component={Paper}>
      <Table stickyHeader>
        <TableHead sx={{ backgroundColor: "primary.light" }}>
          <TableRow>
            {headerConfig.map((cell, index) => {
              return (
                <TableHeaderCell
                  data-testid={cell.testid}
                  key={cell.id}
                  sx={{
                    textAlign: index === 0 ? "left" : "right",
                    pr: cell.sortable ? 2 : 5.5,
                  }}
                >
                  {cell.sortable ? <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel> : cell.title}
                </TableHeaderCell>
              );
            })}
          </TableRow>
        </TableHead>
        <TableBody>
          {sortedData.map((groupedWorkItem) => {
            return <GroupingWorkitemRow key={groupedWorkItem.id} groupedWorkItem={groupedWorkItem} />;
          })}
        </TableBody>
      </Table>
    </TableContainer>
  );
};
