import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import { GetProjectResponse } from "api/generatedApi";
import { useTranslation } from "react-i18next";

interface Props {
  project: GetProjectResponse;
}

export const ContactInfoContent = ({ project }: Props) => {
  const { t } = useTranslation();

  return (
    <Grid>
      <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
        <Box>
          <Typography variant={"overline"} color={"primary.main"}>
            {t("dashboard.projectInfo.contactInfo.adress")}
          </Typography>
          <Typography variant={"body2"} color={"primary.main"}>
            {project.companyAddress}
          </Typography>
        </Box>
        <Box>
          <Typography variant={"overline"} color={"primary.main"}>
            {t("dashboard.projectInfo.contactInfo.name")}
          </Typography>
          <Typography variant={"body2"} color={"primary.main"}>
            {project.companyName}
          </Typography>
        </Box>
        <Box sx={{ display: "flex" }}>
          <Box sx={{ width: "50%" }}>
            <Typography variant={"overline"} color={"primary.main"}>
              {t("dashboard.projectInfo.contactInfo.cvr")}
            </Typography>
            <Typography variant={"body2"} color={"primary.main"}>
              {project.companyCvrNo}
            </Typography>
          </Box>
          <Box sx={{ width: "50%" }}>
            <Typography variant={"overline"} color={"primary.main"}>
              {t("dashboard.projectInfo.contactInfo.p")}
            </Typography>
            <Typography variant={"body2"} color={"primary.main"}>
              {project.companyPNo}
            </Typography>
          </Box>
        </Box>
      </Box>
    </Grid>
  );
};
