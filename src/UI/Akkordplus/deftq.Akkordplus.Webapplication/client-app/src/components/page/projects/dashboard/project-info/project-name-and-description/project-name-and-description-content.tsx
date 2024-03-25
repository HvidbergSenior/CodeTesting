import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import { GetProjectResponse } from "api/generatedApi";
import { useTranslation } from "react-i18next";

interface Props {
  project: GetProjectResponse;
}

export const ProjectNameAndDescriptionContent = ({ project }: Props) => {
  const { t } = useTranslation();

  const hasDescription = (): boolean => {
    return (project.description ?? "").length > 0;
  };

  return (
    <Grid>
      <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
        <Box>
          <Typography variant={"overline"} color={"primary.main"}>
            {t("dashboard.projectInfo.projectNameAndDescription.project")}
          </Typography>
          <Typography variant={"body2"} color={"primary.main"}>
            {project.title}
          </Typography>
        </Box>
        <Box sx={{ display: "flex" }}>
          <Box sx={{ width: "50%" }}>
            <Typography variant={"overline"} color={"primary.main"}>
              {t("dashboard.projectInfo.projectNameAndDescription.pieceworkNumber")}
            </Typography>
            <Typography variant={"body2"} color={"primary.main"}>
              {project.pieceWorkNumber}
            </Typography>
          </Box>
          <Box sx={{ width: "50%" }}>
            <Typography variant={"overline"} color={"primary.main"}>
              {t("dashboard.projectInfo.projectNameAndDescription.orderNumber")}
            </Typography>
            <Typography variant={"body2"} color={"primary.main"}>
              {project.orderNumber}
            </Typography>
          </Box>
        </Box>
        <Box>
          <Typography variant={"overline"} color={"primary.main"}>
            {t("dashboard.projectInfo.projectNameAndDescription.description")}
          </Typography>
          {hasDescription() ? (
            <Typography variant={"body2"} color={"primary.main"} whiteSpace="pre-wrap">
              {project.description}
            </Typography>
          ) : (
            <Typography variant="body2" color="grey.100">
              Ingen beskrivelse endnu
            </Typography>
          )}
        </Box>
      </Box>
    </Grid>
  );
};
