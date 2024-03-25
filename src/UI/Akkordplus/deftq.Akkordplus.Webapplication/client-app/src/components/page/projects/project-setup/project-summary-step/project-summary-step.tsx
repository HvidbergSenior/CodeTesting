import { Box, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { ProjectSetupFormData } from "../project-setup-form-data";
import { formatNumberToAmount } from "utils/formats";

interface Props {
  data: ProjectSetupFormData;
}

export function ProjectSummaryStep(props: Props) {
  const { data } = props;
  const { t } = useTranslation();

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
      <Box>
        <Typography variant="caption" color={"primary.main"}>
          {t("project.projectSetup.summary.name")}
        </Typography>
        <Typography data-testid="create-project-summary-title">{data.title}</Typography>
      </Box>
      <Box>
        <Typography variant="caption" color={"primary.main"}>
          {t("project.projectSetup.summary.description")}
        </Typography>
        <Typography data-testid="create-project-summary-description">{data.description}</Typography>
      </Box>
      <Box>
        <Typography variant="caption" color={"primary.main"}>
          {t("project.projectSetup.summary.pieceworkType")}
        </Typography>
        <Typography data-testid="create-project-summary-piecework-header" color="secondary">
          {t("project.pieceworkTypes." + data.pieceworkType + ".header")}
        </Typography>
        <Typography data-testid="create-project-summary-piecework-description">
          {t("project.pieceworkTypes." + data.pieceworkType + ".description")}
        </Typography>
      </Box>
      {data.pieceworkType === "TwelveTwo" && (
        <Box>
          <Typography variant="caption" color={"primary.main"}>
            {t("project.projectSetup.summary.lumpSum")}
          </Typography>
          <Typography data-testid="create-project-summary-piecework-lump-sum">
            {`${formatNumberToAmount(data?.lumpSum, 2)} ${t("common.currency")}`}
          </Typography>
        </Box>
      )}
    </Box>
  );
}
