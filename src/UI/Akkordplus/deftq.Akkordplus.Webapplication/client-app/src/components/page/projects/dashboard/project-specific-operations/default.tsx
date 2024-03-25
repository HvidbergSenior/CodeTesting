import { useEffect, useRef, useState } from "react";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import Fab from "@mui/material/Fab";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import { GetProjectResponse, useGetApiProjectsByProjectIdProjectspecificoperationQuery } from "api/generatedApi";
import { useScreenSize, ScreenSizeEnum } from "shared/use-screen-size";
import { DefaultBack2DashBoardNavigation } from "../navigation/default";
import { useToast } from "shared/toast/hooks/use-toast";
import { useTranslation } from "react-i18next";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { MobileProjectSpecificOperations } from "./mobile-project-specific-operations";
import { DesktopProjectSpecificOperations } from "./desktop-project-specific-operations";
import { useCreateProjectSpecificOperation } from "./edit/hooks/use-create-projects-specific-operation";

interface Props {
  project: GetProjectResponse;
}

export const DefaultProjectSpecificOperations = (props: Props) => {
  const { project } = props;
  const { screenSize } = useScreenSize();
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const { data, error } = useGetApiProjectsByProjectIdProjectspecificoperationQuery({ projectId: project?.id ?? "" });
  const [isLoading, setIsLoading] = useState(true);
  const openCreateDialog = useCreateProjectSpecificOperation({ project });
  const { canCreateProjectSpecificOperation } = useDashboardRestrictions(project);

  useEffect(() => {
    if (data) {
      setIsLoading(false);
    }
    if (error) {
      setIsLoading(false);
      console.error(error);
      toastRef.current.error(tRef.current("dashboard.projectSpecificOperations.getProjectSpecificOperations.error"));
    }
  }, [data, error, toastRef, tRef]);

  return (
    <Grid item xs={true} overflow={"auto"} padding={1} height={"100%"}>
      <Box sx={{ height: "100%", display: "flex", flexDirection: "column", justifyContent: "start" }}>
        <DefaultBack2DashBoardNavigation here={t("dashboard.links.projectSpecificOperations")} />

        {isLoading && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <CircularProgress size={100} />
          </Box>
        )}
        {!isLoading && (!data?.projectSpecificOperations || data.projectSpecificOperations.length <= 0) && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <Typography variant="body1" color="grey.100" fontStyle={"italic"}>
              {t("dashboard.projectSpecificOperations.getProjectSpecificOperations.noProjectSpecificOperations")}
            </Typography>
          </Box>
        )}
        {!isLoading &&
          data?.projectSpecificOperations &&
          (data?.projectSpecificOperations ?? []).length > 0 &&
          (screenSize === ScreenSizeEnum.Mobile ? (
            <MobileProjectSpecificOperations project={project} compensations={data.projectSpecificOperations} />
          ) : (
            <DesktopProjectSpecificOperations project={project} compensations={data.projectSpecificOperations} />
          ))}
        {!isLoading && canCreateProjectSpecificOperation() && (
          <Fab
            sx={{
              position: "absolute",
              bottom: screenSize === ScreenSizeEnum.Mobile ? "70px" : "30px",
              right: screenSize === ScreenSizeEnum.Mobile ? "20px" : "10px",
              backgroundColor: "primary.dark",
            }}
            onClick={openCreateDialog}
          >
            <AddCircleOutlineIcon fontSize="large" sx={{ color: "grey.100", pb: "2px", pr: "2px" }} />
          </Fab>
        )}
      </Box>
    </Grid>
  );
};
