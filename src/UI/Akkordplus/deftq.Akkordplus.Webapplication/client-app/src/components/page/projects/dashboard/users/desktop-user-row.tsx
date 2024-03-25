import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { ProjectUserResponse } from "api/generatedApi";
import { useFormatUser } from "./create/hooks/use-format-users";

interface Props {
  user: ProjectUserResponse;
}

export const DesktopUserRow = (props: Props) => {
  const { user } = props;
  const { formatRole } = useFormatUser();
  return (
    <TableRow data-testid={`dashboard-favorite-row-${user.name}`}>
      <TableCell data-testid="dashboard-users-row-name">
        <Typography variant="body2">{user.name}</Typography>
      </TableCell>
      <TableCell data-testid="dashboard-user-row-role">
        <Typography variant="body2">{formatRole(user.role)}</Typography>
      </TableCell>
      <TableCell data-testid="dashboard-user-row-status">
        <Typography variant="body2">-</Typography>
      </TableCell>
      <TableCell data-testid="dashboard-user-row-email">
        <Typography variant="body2">{user.email}</Typography>
      </TableCell>
      <TableCell data-testid="dashboard-user-row-adr">
        <Typography variant="body2">{user.address}</Typography>
      </TableCell>
      <TableCell data-testid="dashboard-user-row-phone">
        <Typography variant="body2">{user.phone}</Typography>
      </TableCell>
    </TableRow>
  );
};
