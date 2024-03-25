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
import { ProjectUserResponse } from "api/generatedApi";
import { sortCompareString } from "utils/compares";
import { useOrder } from "shared/sorting/use-order";
import { DesktopUserRow } from "./desktop-user-row";
import { useFormatUser } from "./create/hooks/use-format-users";

export type UserSortableId = "Name" | "Role" | "Status" | "Email" | "Adr" | "Phone";

export interface TableHeaderDesktopUsers {
  id: UserSortableId;
  title: string;
  testid: string;
}

interface Props {
  users: ProjectUserResponse[];
}

export const DesktopProjectUsers = (props: Props) => {
  const { users } = props;
  const { t } = useTranslation();
  const { formatRole } = useFormatUser();
  const { direction, orderBy, getLabelProps } = useOrder<UserSortableId>("Name");

  const sortedData = useMemo(() => {
    var sortedList = users;

    switch (orderBy) {
      case "Name":
        sortedList = [...users].sort((a, b) => sortCompareString(direction, a?.name, b?.name));
        break;
      case "Role":
        sortedList = [...users].sort((a, b) => sortCompareString(direction, formatRole(a.role), formatRole(b.role)));
        break;
      case "Status":
        sortedList = [...users];
        break;
      case "Email":
        sortedList = [...users].sort((a, b) => sortCompareString(direction, a?.email, b?.email));
        break;
      case "Adr":
        sortedList = [...users].sort((a, b) => sortCompareString(direction, a?.address, b?.address));
        break;
      case "Phone":
        sortedList = [...users].sort((a, b) => sortCompareString(direction, a?.phone, b?.phone));
        break;
    }

    return sortedList;
  }, [users, orderBy, direction, formatRole]);

  const headerConfig: TableHeaderDesktopUsers[] = [
    {
      id: "Name",
      title: t("common.name"),
      testid: "dashboard-users-header-name",
    },
    {
      id: "Role",
      title: t("dashboard.users.table.role"),
      testid: "dashboard-users-header-role",
    },
    {
      id: "Status",
      title: t("dashboard.users.table.status"),
      testid: "dashboard-users-header-status",
    },
    {
      id: "Email",
      title: t("dashboard.users.table.email"),
      testid: "dashboard-users-header-email",
    },
    {
      id: "Adr",
      title: t("dashboard.users.table.adr"),
      testid: "dashboard-users-header-adr",
    },
    {
      id: "Phone",
      title: t("dashboard.users.table.phone"),
      testid: "dashboard-users-header-phone",
    },
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%", pl: 4, pr: 4 }}>
      <Typography variant="body1" color="primary.main" sx={{ pt: 2, pb: 2 }}>
        {t("dashboard.users.description")}
      </Typography>
      <TableContainer sx={{ height: "calc(100vh - 240px)", overflowY: "auto", pb: "50px" }} component={Paper}>
        <Table stickyHeader>
          <TableHead sx={{ backgroundColor: "primary.light" }}>
            <TableRow>
              {headerConfig.map((cell, index) => {
                return (
                  <TableCell data-testid={cell.testid} key={cell.id} sx={{ color: "primary.main", backgroundColor: "primary.light" }}>
                    <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                  </TableCell>
                );
              })}
            </TableRow>
          </TableHead>
          <TableBody>
            {sortedData?.map((user) => {
              return <DesktopUserRow key={user.name} user={user} />;
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
