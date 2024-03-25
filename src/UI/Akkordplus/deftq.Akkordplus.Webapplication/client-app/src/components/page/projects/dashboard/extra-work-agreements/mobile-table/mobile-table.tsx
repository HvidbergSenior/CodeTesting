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
import { MobileExtraWorkAgreementRow } from "./mobile-row";
import { capitalizeString } from "utils/formats";
import { useDeleteExtraWorkAgreements } from "../hooks/use-delete-extra-work-agreements";

export interface TableHeaderMobileAgreements {
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

export type AgreementSortableId = "Name" | "Payment";

export const MobileProjectExtraWorkAgreements = (props: Props) => {
  const { agreements, project } = props;
  const { t } = useTranslation();
  const { canSelectExtraWorkAgreements, canDeleteExtraWorkAgreements } = useDashboardRestrictions(project);
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [checkedAll, setCheckedAll] = useState(false);
  const openDeleteExtraWorkAgreementsDialog = useDeleteExtraWorkAgreements({ project, selectedIds });
  const { direction, orderBy, getLabelProps } = useOrder<AgreementSortableId>("Name");

  const sortedData = useMemo(() => {
    var sortedList = agreements;

    switch (orderBy) {
      case "Name":
        sortedList = [...agreements].sort((a, b) => sortCompareString(direction, a?.name, b?.name));
        break;
      case "Payment":
        sortedList = [...agreements].sort((a, b) => sortCompareNumber(direction, a.paymentDkr, b.paymentDkr));
        break;
    }

    return sortedList;
  }, [agreements, orderBy, direction]);

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

  const isAgreementChecked = (id: string | undefined): boolean => {
    return selectedIds.has(id ?? "");
  };

  const headerConfig: TableHeaderMobileAgreements[] = [
    {
      id: "Name",
      title: t("dashboard.favorits.table.info"),
      sortable: true,
      align: "left",
      testid: "dashboard-extra-work-agreements-header-text",
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
    <Box sx={{ flex: 1, display: "flex", flexDirection: "column", pb: 12 }}>
      <Box sx={{ display: "flex", justifyContent: "flex-end", mb: 0.5 }}>
        <TextIcon>
          <IconButton
            data-testid="dashboard-extra-work-agreements-delete-rows"
            disabled={!canDeleteExtraWorkAgreements(selectedIds)}
            onClick={openDeleteExtraWorkAgreementsDialog}
          >
            <DeleteOutlinedIcon />
          </IconButton>
        </TextIcon>
      </Box>
      <TableContainer component={Paper}>
        <Table>
          <TableHead sx={{ backgroundColor: "primary.light" }}>
            <TableRow>
              <TableCell sx={{ backgroundColor: "primary.light" }}>
                <Checkbox
                  data-testid="measurement-table-checkbox-all"
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
                      sx={{ color: "primary.main", backgroundColor: "primary.light", textAlign: "center" }}
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
                <MobileExtraWorkAgreementRow
                  key={agreement.extraWorkAgreementId}
                  project={project}
                  row={agreement}
                  onCheck={onCheck}
                  checked={isAgreementChecked(agreement.extraWorkAgreementId)}
                />
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
