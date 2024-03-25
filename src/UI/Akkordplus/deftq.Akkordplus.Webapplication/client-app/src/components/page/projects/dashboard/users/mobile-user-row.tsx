import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { ProjectUserResponse } from "api/generatedApi";
import { useFormatUser } from "./create/hooks/use-format-users";

interface Props {
  user: ProjectUserResponse;
}

export const MobileUserRow = (props: Props) => {
  const { user } = props;
  const { formatRole } = useFormatUser();
  return (
    <TableRow data-testid={`mobile-user-row-${user.name}`}>
      <TableCell data-testid="mobile-user-row-name" sx={{ display: "flex", flexDirection: "column" }}>
        <Typography variant="body2">{user.name}</Typography>
        <Typography variant="caption" color={"primary.main"}>
          {user.email}
        </Typography>
        {user.address && (
          <Typography variant="caption" color={"primary.main"}>
            {user.address}
          </Typography>
        )}
        {user.phone && (
          <Typography variant="caption" color={"primary.main"}>
            {user.phone}
          </Typography>
        )}
      </TableCell>
      <TableCell data-testid="mobile-user-row-role">
        <Typography variant="body2">{formatRole(user.role)}</Typography>
      </TableCell>
    </TableRow>
  );
};
