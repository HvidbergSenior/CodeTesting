import { useEffect, useRef, useState } from "react";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import Fab from "@mui/material/Fab";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import { GetProjectResponse, useGetApiProjectsByProjectIdFavoritesQuery } from "api/generatedApi";
import { useScreenSize, ScreenSizeEnum } from "shared/use-screen-size";
import { DefaultBack2DashBoardNavigation } from "../navigation/default";
import { DesktopProjectFavorites } from "./desktop-favorites";
import { MobileProjectFavorites } from "./mobile-favorites";
import { useToast } from "shared/toast/hooks/use-toast";
import { useTranslation } from "react-i18next";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useCreateFavorite } from "./create/hooks/use-create";

interface Props {
  project: GetProjectResponse;
}

export const DefaultProjectFavorites = (props: Props) => {
  const { project } = props;
  const { screenSize } = useScreenSize();
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const { data, error } = useGetApiProjectsByProjectIdFavoritesQuery({ projectId: project?.id ?? "" });
  const [isLoading, setIsLoading] = useState(true);
  const { canCreateFavorites } = useDashboardRestrictions(project);
  const openCreateFavoriteDialog = useCreateFavorite({ project });

  useEffect(() => {
    if (data) {
      setIsLoading(false);
    }
    if (error) {
      setIsLoading(false);
      console.error(error);
      toastRef.current.error(tRef.current("dashboard.favorits.getFavorites.error"));
    }
  }, [data, error, toastRef, tRef]);

  const openCreateFavoriteItem = () => {
    openCreateFavoriteDialog();
  };

  return (
    <Grid item xs={true} overflow={"auto"} padding={1} height={"100%"}>
      <Box sx={{ height: "100%", display: "flex", flexDirection: "column", justifyContent: "start" }}>
        <DefaultBack2DashBoardNavigation here={t("dashboard.links.favorits")} />
        {isLoading && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <CircularProgress size={100} />
          </Box>
        )}
        {!isLoading && (!data?.favorites || data.favorites.length <= 0) && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <Typography variant="body1" color="grey.100" fontStyle={"italic"}>
              {t("dashboard.favorits.getFavorites.noFavorites")}
            </Typography>
          </Box>
        )}
        {!isLoading &&
          data?.favorites &&
          (data?.favorites ?? []).length > 0 &&
          (screenSize === ScreenSizeEnum.Mobile ? (
            <MobileProjectFavorites project={project} favorites={data.favorites} />
          ) : (
            <DesktopProjectFavorites project={project} favorites={data.favorites} />
          ))}
        {!isLoading && canCreateFavorites() && (
          <Fab
            sx={{
              position: "absolute",
              bottom: screenSize === ScreenSizeEnum.Mobile ? "70px" : "30px",
              right: screenSize === ScreenSizeEnum.Mobile ? "20px" : "10px",
              backgroundColor: "primary.dark",
            }}
            onClick={openCreateFavoriteItem}
          >
            <AddCircleOutlineIcon fontSize="large" sx={{ color: "grey.100", pb: "2px", pr: "2px" }} />
          </Fab>
        )}
      </Box>
    </Grid>
  );
};
