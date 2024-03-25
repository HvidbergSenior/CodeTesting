import styled from "@mui/system/styled";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";
import { costumPalette } from "theme/palette";
import { ProjectResponse } from "api/generatedApi";
import { formatNumberToAmount } from "utils/formats";

interface Props {
  folder: ExtendedProjectFolder | undefined;
  project: ProjectResponse;
}

export const BaseRateAndSupplementsContent = ({ folder, project }: Props) => {
  const { t } = useTranslation();

  const GridBoxRightStyled = styled(Box)`
    height: 100%;
    width: 100%;
    border-top: 1px solid;
    border-color: #e0e0e0;
  `;

  const GridBoxLeftStyled = styled(Box)`
    height: 100%;
    width: 100%;
    border-top: 1px solid;
    border-left: 1px solid;
    border-color: #e0e0e0;
  `;

  return (
    <Box
      sx={{
        pt: 2,
        display: "grid",
        gridTemplate: "1fr 1fr / 1fr 1fr",
        flexGrow: 1,
      }}
    >
      <GridBoxRightStyled>
        <Box sx={{ display: "flex", flexDirection: "column", height: "100%", alignItems: "center", justifyContent: "center" }}>
          <Typography sx={{ pb: 2 }} variant="overline">
            {t("content.calculation.baseRateAndSupplements.indirectTime")}
          </Typography>
          <Typography variant="h4" sx={{ color: costumPalette.green }}>
            {formatNumberToAmount(folder?.baseRateAndSupplements?.indirectTimeSupplementPercentage?.value) + ` %`}
          </Typography>
        </Box>
      </GridBoxRightStyled>

      <GridBoxLeftStyled sx={{}}>
        <Box sx={{ display: "flex", flexDirection: "column", height: "100%", alignItems: "center", justifyContent: "center" }}>
          <Typography sx={{ pb: 2 }} variant="overline">
            {t("content.calculation.baseRateAndSupplements.personalTime")}
          </Typography>
          <Typography variant="h4" sx={{ color: costumPalette.green }}>
            {formatNumberToAmount(folder?.baseRateAndSupplements?.personalTimeSupplementPercentage) + ` %`}
          </Typography>
        </Box>
      </GridBoxLeftStyled>

      <GridBoxRightStyled>
        <Box sx={{ display: "flex", flexDirection: "column", height: "100%", alignItems: "center", justifyContent: "center" }}>
          <Typography sx={{ pb: 2 }} variant="overline">
            {t("content.calculation.baseRateAndSupplements.siteSpecificTime")}
          </Typography>
          <Typography variant="h4" sx={{ color: costumPalette.green }}>
            {formatNumberToAmount(folder?.baseRateAndSupplements?.siteSpecificTimeSupplementPercentage?.value) + ` %`}
          </Typography>
        </Box>
      </GridBoxRightStyled>

      <GridBoxLeftStyled>
        <Box sx={{ display: "flex", flexDirection: "column", height: "100%", alignItems: "center", justifyContent: "center" }}>
          <Typography sx={{ pb: 2 }} variant="overline">
            {t("content.calculation.baseRateAndSupplements.totalSupplement")}
          </Typography>
          <Typography variant="h4" sx={{ color: "secondary.main" }}>
            {formatNumberToAmount(folder?.baseRateAndSupplements?.combinedSupplementPercentage, 2) + ` %`}
          </Typography>
        </Box>
      </GridBoxLeftStyled>
    </Box>
  );
};
