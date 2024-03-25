import Checkbox from "@mui/material/Checkbox";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { FavoritesResponse, GetProjectResponse } from "api/generatedApi";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";

interface Props {
  project: GetProjectResponse;
  row: FavoritesResponse;
  onCheck: (id: string | undefined) => void;
  checked: boolean;
}

export const MobileFavoriteRow = (props: Props) => {
  const { project, row, onCheck, checked } = props;
  const { canSelectFavorites } = useDashboardRestrictions(project);
  return (
    <TableRow data-testid={`dashboard-favorite-row-${row.text}`}>
      <TableCell>
        <Checkbox
          data-testid="dashboard-favorite-row-checkbox"
          onChange={() => onCheck(row.favoriteItemId)}
          onClick={(event) => event.stopPropagation()}
          checked={checked}
          disabled={!canSelectFavorites()}
        />
      </TableCell>
      <TableCell data-testid="dashboard-favorite-row-text" title={row.text ?? ""}>
        <Typography variant="body2">{row.text}</Typography>
        <Typography variant="caption" color={"primary.main"}>
          {row.catalogId}
        </Typography>
      </TableCell>
    </TableRow>
  );
};
