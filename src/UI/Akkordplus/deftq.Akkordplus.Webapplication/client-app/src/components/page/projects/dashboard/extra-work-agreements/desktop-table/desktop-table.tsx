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
import IconButton from "@mui/material/IconButton";
import Checkbox from "@mui/material/Checkbox";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";
import { ExtraWorkAgreementResponse, GetProjectResponse } from "api/generatedApi";
import { sortCompareNumber, sortCompareString } from "utils/compares";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useOrder } from "shared/sorting/use-order";
import { DesktopExtraWorkAgreementRow } from "./desktop-row";
import { useExtraWorkAgreementFormatter } from "../hooks/format";
import { capitalizeString, formatHoursAndMinutes } from "utils/formats";
import { useDeleteExtraWorkAgreements } from "../hooks/use-delete-extra-work-agreements";

export type AgreementSortableId = "Id" | "Name" | "Type" | "Hours" | "Payment";

export interface TableHeaderDesktopAgreements {
  id: AgreementSortableId;
  title: string;
  sortable: boolean;
  align: string;
  testid: string;
}

interface Props {
  project: GetProjectResponse;
  agreements: ExtraWorkAgreementResponse[];
}

export const DesktopProjectExtraWorkAgreements = (props: Props) => {
  const { agreements, project } = props;
  const { t } = useTranslation();
  const { canSelectExtraWorkAgreements, canDeleteExtraWorkAgreements } = useDashboardRestrictions(project);
  const { getTypeByAgreementFormatted } = useExtraWorkAgreementFormatter();
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [checkedAll, setCheckedAll] = useState(false);
  const openDeleteExtraWorkAgreementsDialog = useDeleteExtraWorkAgreements({ project, selectedIds });
  const { direction, orderBy, getLabelProps } = useOrder<AgreementSortableId>("Name");

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
      agreements?.forEach((item) => ids.add(item.extraWorkAgreementId ?? ""));
    }
    setSelectedIds(ids);
    setCheckedAll(!checkedAll);
  };

  const isFavoriteChecked = (id: string | undefined): boolean => {
    return selectedIds.has(id ?? "");
  };

  const sortedData = useMemo(() => {
    var sortedList = agreements;

    switch (orderBy) {
      case "Id":
        sortedList = [...agreements].sort((a, b) => sortCompareString(direction, a?.extraWorkAgreementNumber, b?.extraWorkAgreementNumber));
        break;
      case "Name":
        sortedList = [...agreements].sort((a, b) => sortCompareString(direction, a?.name, b?.name));
        break;
      case "Type":
        sortedList = [...agreements].sort((a, b) => sortCompareString(direction, getTypeByAgreementFormatted(a), getTypeByAgreementFormatted(b)));
        break;
      case "Hours":
        sortedList = [...agreements].sort((a, b) =>
          sortCompareString(direction, formatHoursAndMinutes(a.paymentDkr), formatHoursAndMinutes(b.paymentDkr))
        );
        break;
      case "Payment":
        sortedList = [...agreements].sort((a, b) => sortCompareNumber(direction, a.paymentDkr, b.paymentDkr));
        break;
    }

    return sortedList;
  }, [agreements, orderBy, direction, getTypeByAgreementFormatted]);

  const headerConfig: TableHeaderDesktopAgreements[] = [
    {
      id: "Id",
      title: t("dashboard.extraWorkAgreements.table.id"),
      sortable: true,
      align: "left",
      testid: "dashboard-extra-work-agreements-header-id",
    },
    {
      id: "Name",
      title: t("common.name"),
      sortable: true,
      align: "left",
      testid: "dashboard-extra-work-agreements-header-name",
    },
    {
      id: "Type",
      title: t("dashboard.extraWorkAgreements.table.type"),
      sortable: true,
      align: "left",
      testid: "dashboard-extra-work-agreements-header-type",
    },
    {
      id: "Hours",
      title: t("common.time.hours"),
      sortable: true,
      align: "right",
      testid: "dashboard-extra-work-agreements-header-hours",
    },
    {
      id: "Payment",
      title: capitalizeString(t("common.currency")),
      sortable: true,
      align: "right",
      testid: "dashboard-extra-work-agreements-header-payment",
    },
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%", pl: 4, pr: 4 }}>
      <Box sx={{ display: "flex", justifyContent: "end" }}>
        <Box sx={{ display: "flex", p: 2 }}>
          <TextIcon translateText="common.delete">
            <IconButton
              data-testid="dashboard-extra-work-agreements-delete-rows"
              disabled={!canDeleteExtraWorkAgreements(selectedIds)}
              onClick={openDeleteExtraWorkAgreementsDialog}
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
                  data-testid="dashboard.extra-work-agreements-table-checkbox-all"
                  onChange={(event) => onCheckAll(event.target.checked)}
                  checked={selectedIds.size === agreements.length}
                  disabled={!canSelectExtraWorkAgreements()}
                />
              </TableCell>
              {headerConfig.map((cell) => {
                if (!cell.sortable) {
                  return (
                    <TableCell
                      data-testid={cell.testid}
                      key={cell.id}
                      sx={{ color: "primary.main", backgroundColor: "primary.light", textAlign: cell.align }}
                    >
                      {cell.title}
                    </TableCell>
                  );
                }
                return (
                  <TableCell
                    data-testid={cell.testid}
                    key={cell.id}
                    sx={{ color: "primary.main", backgroundColor: "primary.light", textAlign: cell.align }}
                  >
                    <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                  </TableCell>
                );
              })}
            </TableRow>
          </TableHead>
          <TableBody>
            {sortedData.map((agreement) => {
              return (
                <DesktopExtraWorkAgreementRow
                  key={agreement.extraWorkAgreementNumber}
                  project={project}
                  row={agreement}
                  onCheck={onCheck}
                  checked={isFavoriteChecked(agreement.extraWorkAgreementId)}
                />
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
