import { useTranslation } from "react-i18next";
import styled from "@mui/system/styled";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { costumPalette } from "theme/palette";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { formatNumberToAmount } from "utils/formats";
interface Props {
  folder: ExtendedProjectFolder;
}

export const PaymentFactorContent = ({ folder }: Props) => {
  const { t } = useTranslation();

  const GridBoxTopStyled = styled(Box)`
    height: 100%;
    width: 100%;
  `;

  const GridBoxBottomStyled = styled(Box)`
    height: 100%;
    width: 100%;
    border-top: 1px solid ${costumPalette.gray};
  `;

  return (
    <Box
      sx={{
        display: "grid",
        gridTemplate: "1fr 1fr",
        flexGrow: 1,
      }}
    >
      <GridBoxTopStyled>
        <Box sx={{ pl: "40px", display: "flex", flexDirection: "column", height: "100%", alignItems: "left", justifyContent: "center" }}>
          <Typography sx={{ pb: 2 }} variant="overline">
            {t("content.calculation.paymentFactor.currentPeriod")}
          </Typography>
          <Typography variant="h4" sx={{ color: costumPalette.green }}>
            {formatNumberToAmount(folder?.baseRateAndSupplements?.baseRatePerMinDkr, 3) +
              ` ` +
              t("content.calculation.paymentFactor.edit.paymentPrMinute")}
          </Typography>
        </Box>
      </GridBoxTopStyled>

      <GridBoxBottomStyled>
        <Box sx={{ pl: "40px", display: "flex", flexDirection: "column", height: "100%", alignItems: "left", justifyContent: "center" }}>
          <Typography sx={{ pb: 2 }} variant="overline">
            {t("content.calculation.paymentFactor.userDefinedRegulation")}
          </Typography>
          <Typography variant="h4" sx={{ color: costumPalette.green }}>
            {formatNumberToAmount(folder?.baseRateAndSupplements?.baseRateRegulationPercentage?.value) + ` %`}
          </Typography>
        </Box>
      </GridBoxBottomStyled>
    </Box>
  );
};
