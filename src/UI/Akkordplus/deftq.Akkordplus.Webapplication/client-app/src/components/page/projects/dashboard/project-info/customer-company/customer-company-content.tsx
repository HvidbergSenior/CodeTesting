import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import Typography from "@mui/material/Typography";
import { GetExtraWorkAgreementRatesQueryResponse } from "api/generatedApi";
import { useTranslation } from "react-i18next";
import { costumPalette } from "theme/palette";
import { formatNumberToAmount } from "utils/formats";

interface Props {
  data: GetExtraWorkAgreementRatesQueryResponse;
}

export const ExtraWorkAgreementContent = ({ data }: Props) => {
  const { t } = useTranslation();

  const GridBoxLeftStyled = styled(Box)`
    height: 100%;
    width: 100%;
    border-right: 1px solid;
    border-top: 1px solid;
    border-color: ${costumPalette.gray};
  `;

  const GridBoxRightStyled = styled(Box)`
    height: 100%;
    width: 100%;
    border-top: 1px solid;
    border-color: ${costumPalette.gray};
  `;

  return (
    <Box
      sx={{
        display: "grid",
        gridTemplateColumns: "1fr 1fr",
        flexGrow: 1,
      }}
      pt={2}
    >
      <GridBoxLeftStyled>
        <Box sx={{ pl: "40px", display: "flex", flexDirection: "column", height: "100%", alignItems: "left", justifyContent: "center" }}>
          <Typography sx={{ pb: 2 }} variant="overline">
            {t("dashboard.projectInfo.customerCompany.customerHours")}
          </Typography>
          <Typography variant="h4" sx={{ color: costumPalette.green }}>
            {formatNumberToAmount(data?.customerRatePerHourDkr, 2) + " " + t("common.currency")}
          </Typography>
        </Box>
      </GridBoxLeftStyled>

      <GridBoxRightStyled sx={{}}>
        <Box sx={{ pl: "40px", display: "flex", flexDirection: "column", height: "100%", alignItems: "left", justifyContent: "center" }}>
          <Typography sx={{ pb: 2 }} variant="overline">
            {t("dashboard.projectInfo.customerCompany.companyHours")}
          </Typography>
          <Typography variant="h4" sx={{ color: costumPalette.green }}>
            {formatNumberToAmount(data?.companyRatePerHourDkr, 2) + " " + t("common.currency")}
          </Typography>
        </Box>
      </GridBoxRightStyled>
    </Box>
  );
};
