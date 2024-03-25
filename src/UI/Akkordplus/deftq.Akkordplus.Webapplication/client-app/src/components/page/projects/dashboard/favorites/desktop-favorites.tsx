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
import { DesktopFavoriteRow } from "./desktop-favorites-row";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useDeleteFavorites } from "./hooks/use-delete-favorites";
import { useOrder } from "shared/sorting/use-order";

export type FavoritesSortableId = "Text" | "Unit";

export interface TableHeaderDesktopFavorites {
  id: FavoritesSortableId;
  title: string;
  sortable: boolean;
  testid: string;
}

interface Props {
  project: GetProjectResponse;
  favorites: FavoritesResponse[];
}

export const DesktopProjectFavorites = (props: Props) => {
  const { favorites, project } = props;
  const { t } = useTranslation();
  const { canSelectFavorites, canDeleteFavorites } = useDashboardRestrictions(project);
  const [selectedIds, setSelectedIds] = useState<Set<string>>(new Set<string>());
  const [checkedAll, setCheckedAll] = useState(false);
  const openDeleteFavoritesDialog = useDeleteFavorites({ project, selectedIds });
  const { direction, orderBy, getLabelProps } = useOrder<FavoritesSortableId>("Text");

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

  const sortedData = useMemo(() => {
    var sortedList = favorites;

    switch (orderBy) {
      case "Text":
        sortedList = [...favorites].sort((a, b) => sortCompareString(direction, a?.text, b?.text));
        break;
      // KTH 14/4-23 hide until we have units case "Unit":
      //   sortedList = [...favorites].sort((a, b) => sortCompareString(direction, a?.unit, b?.unit));
      //   break;
    }

    return sortedList;
  }, [favorites, orderBy, direction]);

  const headerConfig: TableHeaderDesktopFavorites[] = [
    {
      id: "Text",
      title: t("dashboard.favorits.table.text"),
      sortable: true,
      testid: "dashboard-favorites-header-text",
    },
    // KTH 14/4-23 hide until we have units {
    //   id: "Unit",
    //   title: t("dashboard.favorits.table.unit"),
    //   sortable: true,
    //   testid: "dashboard-favorites-header-unit",
    // },
  ];

  return (
    <Box sx={{ flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", width: "100%", height: "100%", pl: 4, pr: 4 }}>
      <Box sx={{ display: "flex" }}>
        <Box sx={{ display: "flex", flexDirection: "column", justifyContent: "flex-start", flexGrow: 1 }}>
          <Typography variant="body1" color="primary.main" sx={{ pt: 2, pb: 2 }}>
            {t("dashboard.favorits.description")}
          </Typography>
        </Box>

        <Box sx={{ display: "flex", justifyContent: "flex-end", p: 2 }}>
          <TextIcon translateText="common.delete">
            <IconButton data-testid="dashboard-favorites-delete-rows" disabled={!canDeleteFavorites(selectedIds)} onClick={openDeleteFavoritesDialog}>
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
                  data-testid="dashboard-favorites-table-checkbox-all"
                  onChange={(event) => onCheckAll(event.target.checked)}
                  checked={selectedIds.size === favorites.length}
                  disabled={!canSelectFavorites()}
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
                } else {
                  return (
                    <TableCell data-testid={cell.testid} key={cell.id} sx={{ color: "primary.main", backgroundColor: "primary.light" }}>
                      <TableSortLabel {...getLabelProps(cell.id)}>{cell.title}</TableSortLabel>
                    </TableCell>
                  );
                }
              })}
            </TableRow>
          </TableHead>
          <TableBody>
            {sortedData.map((favorite) => {
              return (
                <DesktopFavoriteRow
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
