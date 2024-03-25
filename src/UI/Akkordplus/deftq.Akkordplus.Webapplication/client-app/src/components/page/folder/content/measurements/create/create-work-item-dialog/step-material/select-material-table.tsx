import { useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import styled from "@mui/system/styled";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableSortLabel from "@mui/material/TableSortLabel";
import Typography from "@mui/material/Typography";
import { FoundMaterial } from "api/generatedApi";
import { useOrder } from "shared/sorting/use-order";
import { sortCompareString } from "utils/compares";

export interface Props {
  searchResults: FoundMaterial[];
  preselected: FoundMaterial | undefined;
  selectedMaterialProps: (material: FoundMaterial) => void;
}

export function SelectMaterialTable(props: Props) {
  type MaterialSortableId = "material" | "unit";

  const { searchResults, preselected, selectedMaterialProps } = props;
  const { t } = useTranslation();
  const [selectedMaterial, setSelectedMaterial] = useState<FoundMaterial | undefined>(preselected);
  const { direction, orderBy, getLabelProps } = useOrder<MaterialSortableId>("material");

  if (preselected) {
    selectedMaterialProps(preselected);
  }

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

  const selectMaterial = (event: React.MouseEvent<HTMLElement>, material: FoundMaterial) => {
    setSelectedMaterial(material);
    selectedMaterialProps(material);
  };

  const isMaterialSelected = (material: FoundMaterial) => {
    return selectedMaterial && selectedMaterial.eanNumber === material.eanNumber;
  };

  const sortedData = useMemo(() => {
    var sortedList = searchResults;

    switch (orderBy) {
      case "material":
        sortedList = [...sortedList].sort((a, b) => sortCompareString(direction, a.name?.trim(), b.name?.trim()));
        break;
    }

    return sortedList;
  }, [searchResults, orderBy, direction]);

  return (
    <TableContainer sx={{ height: "100%", overflowY: "auto" }} component={Paper}>
      <Table sx={{ tableLayout: "fixed" }}>
        <TableHead sx={{ backgroundColor: "primary.light" }}>
          <TableRow>
            <TableCellStyled sx={{ width: "auto", pr: 0, color: "primary.main" }}>
              <TableSortLabel {...getLabelProps("material")}>{t("content.measurements.create.selectMaterialStep.table.material")}</TableSortLabel>
            </TableCellStyled>
            {/* KTH 14/4-23 hide until we have units <TableCellStyled sx={{ width: "78px", pl: 1, color: "primary.main" }}>
              <TableSortLabel {...getLabelProps("unit")}>{t("content.measurements.create.selectMaterialStep.table.unit")}</TableSortLabel>
            </TableCellStyled> */}
          </TableRow>
        </TableHead>
        <TableBody>
          {sortedData.map((mat, index) => {
            return (
              <TableRow
                data-testid={`create-workitem-material-search-result-${mat.name}-line`}
                key={index}
                onClick={(event) => selectMaterial(event, mat)}
                selected={isMaterialSelected(mat)}
              >
                <TableCellStyled sx={{ pr: 0, color: "primary.main" }}>
                  <TableCellTypographyStyled
                    sx={{ color: "black" }}
                    data-testid={`create-workitem-material-search-result-${mat.name}-name`}
                    variant="body2"
                  >
                    {mat.name}
                  </TableCellTypographyStyled>
                  <Typography data-testid={`create-workitem-material-search-reesult-${mat.name}-ean`} variant="caption">
                    {mat.eanNumber}
                  </Typography>
                </TableCellStyled>
                {/* KTH 14/4-23 hide until we have units <TableCellStyled
                  data-testid={`create-workitem-material-search-result-${mat.name}-unit`}
                  align="center"
                  sx={{ pl: 1, color: "primary.main" }}
                >
                  Test
                </TableCellStyled> */}
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
