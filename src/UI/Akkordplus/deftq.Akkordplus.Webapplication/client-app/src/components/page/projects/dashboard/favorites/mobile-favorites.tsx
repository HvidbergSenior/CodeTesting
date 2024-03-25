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
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import Checkbox from "@mui/material/Checkbox";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";
import { FavoritesResponse, GetProjectResponse } from "api/generatedApi";
import { sortCompareString } from "utils/compares";
import { MobileFavoriteRow } from "./mobile-favorites-row";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useDeleteFavorites } from "./hooks/use-delete-favorites";
import { useOrder } from "shared/sorting/use-order";

export interface TableHeaderMobileFavorites {
  id: string;
  title: string;
  sortable: boolean;
  testid: string;
}
interface Props {
  project: GetProjectResponse;
  favorites: FavoritesResponse[];
}

type FavoriteSortableId = "favorite";

export const MobileProjectFavorites = (props: Props) => {
  const { favorites, project } = props;
  const { t } = useTranslation();
  const { canSelectFavorites, canDeleteFavorites } = useDashboardRestrictions(project);
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [checkedAll, setCheckedAll] = useState(false);
  const openDeleteFavoritesDialog = useDeleteFavorites({ project, selectedIds });
  const { direction, getLabelProps } = useOrder<FavoriteSortableId>("favorite");

  const sortedData = useMemo(() => {
    var sortedList = favorites;

    sortedList = [...favorites].sort((a, b) => sortCompareString(direction, a?.text, b?.text));

    return sortedList;
  }, [favorites, direction]);

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
      favorites?.forEach((item) => ids.add(item.favoriteItemId ?? ""));
    }
    setSelectedIds(ids);
    setCheckedAll(!checkedAll);
  };

  const isFavoriteChecked = (id: string | undefined): boolean => {
    return selectedIds.has(id ?? "");
  };

  const headerConfig: TableHeaderMobileFavorites[] = [
    {
      id: "text",
      title: t("dashboard.favorits.table.text"),
      sortable: true,
      testid: "dashboard-favorites-header-text",
    },
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%" }}>
      <Typography variant="h5" color="primary.dark" sx={{ pt: 1, pl: 1 }}>
        {t("dashboard.favorits.title")}
      </Typography>
      <Typography variant="body1" color="primary.main" sx={{ pt: 2, pb: 0, pl: 1 }}>
        {t("dashboard.favorits.description")}
      </Typography>
      <Box sx={{ display: "flex", justifyContent: "flex-end", mb: 0.5 }}>
        <TextIcon>
          <IconButton data-testid="dashboard-favorites-delete-rows" disabled={!canDeleteFavorites(selectedIds)} onClick={openDeleteFavoritesDialog}>
            <DeleteOutlinedIcon />
          </IconButton>
        </TextIcon>
      </Box>
      <TableContainer sx={{ height: "calc(100vh - 240px)", overflowY: "auto", pb: "90px" }} component={Paper}>
        <Table stickyHeader>
          <TableHead sx={{ backgroundColor: "primary.light" }}>
            <TableRow>
              <TableCell sx={{ backgroundColor: "primary.light" }}>
                <Checkbox
                  data-testid="dashboard-favorites-table-checkbox-all"
                  onChange={(event) => onCheckAll(event.target.checked)}
                  checked={selectedIds.size === favorites.length}
                  disabled={!canSelectFavorites()}
                />
              </TableCell>
              {headerConfig.map((cell, index) => {
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
                if (index === 0) {
                  return (
                    <TableCell data-testid={cell.testid} key={cell.id} sx={{ color: "primary.main", backgroundColor: "primary.light" }}>
                      <TableSortLabel {...getLabelProps("favorite")}>{cell.title}</TableSortLabel>
                    </TableCell>
                  );
                } else {
                  return (
                    <TableCell data-testid={cell.testid} key={cell.id} sx={{ color: "primary.main", backgroundColor: "primary.light" }}>
                      <TableSortLabel {...getLabelProps("favorite")}>{cell.title}</TableSortLabel>
                    </TableCell>
                  );
                }
              })}
            </TableRow>
          </TableHead>
          <TableBody>
            {sortedData.map((favorite) => {
              return (
                <MobileFavoriteRow
                  key={favorite.favoriteItemId}
                  project={project}
                  row={favorite}
                  onCheck={onCheck}
                  checked={isFavoriteChecked(favorite.favoriteItemId)}
                />
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
