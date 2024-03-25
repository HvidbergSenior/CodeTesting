import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import styled from "@mui/system/styled";
import { GetProjectResponse } from "api/generatedApi";
import { costumPalette } from "theme/palette";

import { formatDate, formatNumberToAmount, formatTimestamp, getDateFromDateOnly } from "utils/formats";

interface Props {
  project: GetProjectResponse;
  usePrintlayOut: boolean;
}

export const PieceworkTypeAndPeriodContent = ({ project, usePrintlayOut }: Props) => {
  const { t } = useTranslation();

  const GridBoxTopStyled = styled(Box)`
    height: ${usePrintlayOut ? "auto" : "90%"};
    width: 100%;
  `;

  const GridBoxBottomStyled = styled(Box)`
    height: ${usePrintlayOut ? "auto" : "10%"};
    width: 100%;
    border-top: 1px solid ${usePrintlayOut ? "transparent" : costumPalette.gray};
  `;

  return (
    <Grid>
      <Box
        sx={{
          display: "grid",
          gridTemplate: "1fr 1fr",
          flexGrow: 1,
        }}
      ></Box>
      <GridBoxTopStyled>
        <Box sx={{ pl: usePrintlayOut ? 0 : "40px", display: "flex", flexDirection: "column", gap: 2, height: "100%" }}>
          <Box>
            <Typography variant={"overline"} color={"primary.main"}>
              {t("dashboard.projectInfo.pieceworkTypeAndPeriod.type")}
            </Typography>
            <Typography variant={"body2"} color={"primary.main"}>
              {t("project.pieceworkTypes." + project.pieceworkType + ".short")}
            </Typography>
          </Box>
          {project.pieceworkType === "TwelveTwo" && (
            <Box>
              <Typography variant={"overline"} color={"primary.main"}>
                {t("dashboard.projectInfo.pieceworkTypeAndPeriod.sum")}
              </Typography>
              <Typography variant={"body2"} color={"primary.main"}>
                {`${formatNumberToAmount(project.lumpSumPaymentDkr ?? 0, 2)} ${t("common.currency")}`}
              </Typography>
            </Box>
          )}
          <Box>
            <Typography variant={"overline"} color={"primary.main"}>
              {t("dashboard.projectInfo.pieceworkTypeAndPeriod.period")}
            </Typography>
            <Typography variant={"body2"} color={"primary.main"}>
              {formatDate(getDateFromDateOnly(project.startDate ?? undefined))} - {formatDate(getDateFromDateOnly(project.endDate ?? undefined))}
            </Typography>
          </Box>
        </Box>
      </GridBoxTopStyled>
      <GridBoxBottomStyled>
        <Box sx={{ pl: usePrintlayOut ? 0 : "40px", pt: 2, display: "flex", flexDirection: "row", gap: 2, height: "35%" }}>
          <Box sx={{ width: "35%" }}>
            <Typography variant={"overline"} color={"primary.main"}>
              {t("dashboard.projectInfo.pieceworkTypeAndPeriod.projectNumber")}
            </Typography>
            <Typography variant={"body2"} color={"primary.main"}>
              {project.projectNumber}
            </Typography>
          </Box>
          <Box sx={{ width: "50%" }}>
            <Typography variant={"overline"} color={"primary.main"}>
              {t("dashboard.projectInfo.pieceworkTypeAndPeriod.projectDate")}
            </Typography>
            <Typography variant={"body2"} color={"primary.main"}>
              {formatTimestamp(project.projectCreatedTime, true)}
            </Typography>
          </Box>
        </Box>
      </GridBoxBottomStyled>
    </Grid>
  );
};
