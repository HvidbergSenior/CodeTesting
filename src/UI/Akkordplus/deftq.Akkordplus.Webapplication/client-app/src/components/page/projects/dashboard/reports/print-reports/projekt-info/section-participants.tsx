import { useTranslation } from "react-i18next";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import styled from "@mui/system/styled";
import { ProjectUserResponse } from "api/generatedApi";
import { costumPalette } from "theme/palette";

interface Props {
  users: ProjectUserResponse[];
}

export const PrintSectionProjectParticipants = ({ users }: Props) => {
  const { t } = useTranslation();

  const TableHeaderCell = styled(TableCell)({
    backgroundColor: `${costumPalette.primaryLight} !important`,
    WebkitPrintColorAdjust: "exact",
  });

  const PrintContainer = styled(Box)({
    "@media print": {
      breakInside: "avoid",
    },
  });

  return (
    <PrintContainer sx={{ display: "flex", flexDirection: "column", gap: 2, backgroundColor: "white", p: 5, borderRadius: 2 }}>
      <Typography variant="h6" sx={{ pb: 2 }}>
        {t("dashboard.reports.printReports.projectUser.title")}
      </Typography>
      <TableContainer component={Paper}>
        <Table stickyHeader>
          <TableHead sx={{ backgroundColor: "primary.light" }}>
            <TableRow>
              <TableHeaderCell>{t("common.name")}</TableHeaderCell>
              <TableHeaderCell>{t("dashboard.reports.printReports.projectUser.table.role")}</TableHeaderCell>
              <TableHeaderCell>{t("dashboard.reports.printReports.projectUser.table.email")}</TableHeaderCell>
              <TableHeaderCell>{t("dashboard.reports.printReports.projectUser.table.adr")}</TableHeaderCell>
              <TableHeaderCell>{t("dashboard.reports.printReports.projectUser.table.phone")}</TableHeaderCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {users.map((user, index) => {
              return (
                <TableRow key={index}>
                  <TableCell>{user.name}</TableCell>
                  <TableCell>{t(`dashboard.reports.printReports.projectUser.roles.${user.role}`)}</TableCell>
                  <TableCell>{user.email}</TableCell>
                  <TableCell>{user.address}</TableCell>
                  <TableCell>{user.phone}</TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </PrintContainer>
  );
};
