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
import { MobileUserRow } from "./mobile-user-row";
import { useFormatUser } from "./create/hooks/use-format-users";

export type UsersSortableId = "Name" | "Role";

export interface TableHeaderDesktopUser {
  id: UsersSortableId;
  title: string;
  testid: string;
}

interface Props {
  users: ProjectUserResponse[];
}

export const MobileProjectUsers = (props: Props) => {
  const { users } = props;
  const { t } = useTranslation();
  const { formatRole } = useFormatUser();
  const { direction, orderBy, getLabelProps } = useOrder<UsersSortableId>("Name");

  const sortedData = useMemo(() => {
    var sortedList = users;

    switch (orderBy) {
      case "Name":
        sortedList = [...users].sort((a, b) => sortCompareString(direction, a?.name, b?.name));
        break;
      case "Role":
        sortedList = [...users].sort((a, b) => sortCompareString(direction, formatRole(a.role), formatRole(b.role)));
        break;
    }

    return sortedList;
  }, [users, orderBy, direction, formatRole]);

  const headerConfig: TableHeaderDesktopUser[] = [
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
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%" }}>
      <Typography variant="h5" color="primary.dark" sx={{ pt: 1, pl: 1 }}>
        {t("dashboard.links.users")}
      </Typography>
      <Typography variant="body1" color="primary.main" sx={{ pt: 2, pb: 2, pl: 1 }}>
        {t("dashboard.users.description")}
      </Typography>
      <TableContainer sx={{ height: "calc(100vh - 240px)", overflowY: "auto", pb: "90px" }} component={Paper}>
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
              return <MobileUserRow key={user.name} user={user} />;
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
