import { useCallback, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import TableContainer from "@mui/material/TableContainer";
import Paper from "@mui/material/Paper";
import Checkbox from "@mui/material/Checkbox";
import { GetCompensationPaymentParticipantResponse } from "api/generatedApi";
import { useOrder } from "shared/sorting/use-order";
import { sortCompareNumber, sortCompareString } from "utils/compares";
import { CompensationPaymentParticipantSelectorStepTableRow } from "./participants-table-row";

export type CompensationPaymentUsersSortableId = "Name" | "Amount";

export interface TableHeaderCompensationPaymentUsers {
  id: CompensationPaymentUsersSortableId;
  title: string;
  testid: string;
  alignment: "left" | "right";
}
interface Props {
  participants: GetCompensationPaymentParticipantResponse[];
  onSelectedIds: (ids: string[]) => void;
}

export function CompensationPaymentParticipantsTable(props: Props) {
  const { participants, onSelectedIds } = props;
  const { t } = useTranslation();
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const { direction, orderBy, getLabelProps } = useOrder<CompensationPaymentUsersSortableId>("Name");

  const headerConfig: TableHeaderCompensationPaymentUsers[] = [
    {
      id: "Name",
      title: t("common.name"),
      testid: "dashboard-compensation-payments-participant-table-header-name",
      alignment: "left",
    },
    {
      id: "Amount",
      title: t("dashboard.compensationPayments.participants.table.hoursAndAmount"),
      testid: "dashboard-compensation-payments-participant-table-header-hours-and-amount",
      alignment: "right",
    },
  ];

  const sortedData = useMemo(() => {
    var sortedList: GetCompensationPaymentParticipantResponse[] = [...participants];

    switch (orderBy) {
      case "Amount":
        sortedList = [...participants].sort((a, b) => sortCompareNumber(direction, a.payment, b.payment));
        break;
      case "Name":
        sortedList = [...participants].sort((a, b) => sortCompareString(direction, a.name, b.name));
        break;
    }
    return sortedList;
  }, [orderBy, participants, direction]);

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
      onSelectedIds(Array.from(ids));
    },
    [onSelectedIds, selectedIds]
  );

  const onCheckAll = (checked: boolean) => {
    let ids = new Set<string>();
    if (checked) {
      participants?.forEach((participant) => ids.add(participant.projectParticipantId ?? ""));
    }
    setSelectedIds(ids);
    onSelectedIds(Array.from(ids));
  };

  const isParticipantChecked = (id: string | undefined): boolean => {
    return selectedIds.has(id ?? "");
  };

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead sx={{ backgroundColor: "primary.light" }}>
          <TableRow>
            <TableCell sx={{ backgroundColor: "primary.light" }}>
              <Checkbox
                data-testid="dashboard-compensation-payments-table-checkbox-all"
                onChange={(event) => onCheckAll(event.target.checked)}
                checked={selectedIds.size === participants.length}
              />
            </TableCell>
            {headerConfig.map((cell, index) => {
              if (index === 0) {
                return (
                  <TableCell data-testid={cell.testid} key={cell.id} sx={{ color: "primary.main", backgroundColor: "primary.light" }}>
                    <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                  </TableCell>
                );
              } else {
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
              }
            })}
          </TableRow>
        </TableHead>
        <TableBody>
          {sortedData.map((participant, index) => {
            return (
              <CompensationPaymentParticipantSelectorStepTableRow
                key={index}
                row={participant}
                onCheck={onCheck}
                checked={isParticipantChecked(participant.projectParticipantId)}
              />
            );
          })}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
