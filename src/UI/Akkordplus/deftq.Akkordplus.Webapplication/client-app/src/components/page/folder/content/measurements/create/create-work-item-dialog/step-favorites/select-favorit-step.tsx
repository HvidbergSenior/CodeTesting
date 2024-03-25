import { useState, useMemo } from "react";
import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Grid from "@mui/material/Grid";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import Typography from "@mui/material/Typography";
import styled from "@mui/system/styled";
import { sortCompareString } from "utils/compares";
import { FavoritesResponse } from "api/generatedApi";
import { useOrder } from "shared/sorting/use-order";
import { FormDataWorkItem } from "../create-work-item-form-data";

export interface Props {
  setValue: UseFormSetValue<FormDataWorkItem>;
  getValue: UseFormGetValues<FormDataWorkItem>;
  validateError?: string;
}

export function SelectFavoritStep(props: Props) {
  const { setValue, getValue, validateError } = props;
  const { t } = useTranslation();
  type FavoriteSortableId = "favorite" | "unit";
  const [selectedFavorite, setSelectedFavorite] = useState<FavoritesResponse | undefined>(undefined);
  const favorites = getValue("favorites");
  const { direction, orderBy, getLabelProps } = useOrder<FavoriteSortableId>("favorite");

  const sortedData = useMemo(() => {
    var sortedList = favorites;

    switch (orderBy) {
      case "favorite":
        sortedList = [...favorites].sort((a, b) => sortCompareString(direction, a.text?.trim(), b.text?.trim()));
        break;
      // KTH 14/4-23 hide until we have unitscase "unit":
      //   sortedList = [...favorites].sort((a, b) => sortCompareString(direction, a.unit?.trim(), b.unit?.trim()));
      //   break;
    }

    return sortedList;
  }, [favorites, orderBy, direction]);

  const selectFavorite = (favorite: FavoritesResponse) => {
    setSelectedFavorite(favorite);
    if (favorite.catalogType === "Material") {
      setValue("material", { id: favorite.catalogId, eanNumber: favorite.number, name: favorite.text, unit: favorite.unit });
      setValue("workitemType", "Material");
    }
    if (favorite.catalogType === "Operation") {
      setValue("operation", { operationId: favorite.catalogId, operationNumber: favorite.number, operationText: favorite.text });
      setValue("workitemType", "Operation");
    }
  };

  const isFavoriteSelected = (favorite: FavoritesResponse) => {
    return selectedFavorite?.favoriteItemId === favorite.favoriteItemId;
  };

  const TableCellStyled = styled(TableCell)`
    white-space: nowrap;
    text-overflow: ellipsis;
    overflow: hidden;
  `;

  const TableCellTypographyStyled = styled(Typography)`
    white-space: break-spaces;
    text-overflow: ellipsis;
    overflow: hidden;
  `;

  const NothingToShowTypographyStyled = styled(Typography)`
    height: 90%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-style: italic;
    text-align: center;
  `;

  return (
    <Grid container>
      <Grid item xs={12} minHeight={220}>
        {validateError && (
          <Grid item xs={12} color={"error.main"} textAlign={"center"}>
            {validateError}
          </Grid>
        )}
        {
          // is used for drafts and offline
          favorites?.length <= 0 && (
            <NothingToShowTypographyStyled variant="body1" color="grey.100">
              {t("content.measurements.create.selectFavoriteStep.noFavorites")}
            </NothingToShowTypographyStyled>
          )
        }
        {favorites?.length > 0 && (
          <TableContainer sx={{ height: "100%", overflowY: "auto" }} component={Paper}>
            <Table sx={{ tableLayout: "fixed" }}>
              <TableHead sx={{ backgroundColor: "primary.light" }}>
                <TableRow>
                  <TableCellStyled sx={{ width: "auto", pr: 0, backgroundColor: "primary.light", color: "primary.main" }}>
                    <TableSortLabel {...getLabelProps("favorite")}>
                      {t("content.measurements.create.selectFavoriteStep.table.favorite")}
                    </TableSortLabel>
                  </TableCellStyled>
                  {/* KTH 14/4-23 hide until we have units <TableCellStyled sx={{ width: "78px", pl: 1, backgroundColor: "primary.light", color: "primary.main" }}>
                    <TableSortLabel {...getLabelProps("unit")}>{t("content.measurements.create.selectFavoriteStep.table.unit")}</TableSortLabel>
                  </TableCellStyled> */}
                </TableRow>
              </TableHead>
              <TableBody>
                {sortedData.map((fav, index) => {
                  return (
                    <TableRow
                      data-testid={`create-workitem-material-search-result-${fav.text}-line`}
                      key={index}
                      onClick={() => selectFavorite(fav)}
                      selected={isFavoriteSelected(fav)}
                    >
                      <TableCellStyled sx={{ pr: 0, color: "primary.main" }}>
                        <TableCellTypographyStyled
                          sx={{ color: "black" }}
                          data-testid={`create-workitem-material-search-result-${fav.text}-name`}
                          variant="body2"
                        >
                          {fav.text}
                        </TableCellTypographyStyled>
                        <Typography data-testid={`create-workitem-material-search-reesult-${fav.text}-number`} color={"primary.main"}>
                          {fav.number}
                        </Typography>
                      </TableCellStyled>
                      {/* KTH 14/4-23 hide until we have units <TableCellStyled
                        data-testid={`create-workitem-material-search-result-${fav.text}-unit`}
                        align="right"
                        sx={{ pl: 0, pr: 3.5, color: "primary.main" }}
                      >
                        {fav.unit}
                      </TableCellStyled> */}
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </Grid>
    </Grid>
  );
}
