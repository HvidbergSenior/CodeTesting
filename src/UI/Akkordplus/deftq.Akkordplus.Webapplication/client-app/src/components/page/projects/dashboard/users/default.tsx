import { useTranslation } from "react-i18next";
import { useEffect, useRef, useState } from "react";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
// import Typography from "@mui/material/Typography";
import Fab from "@mui/material/Fab";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import { GetProjectResponse, useGetApiProjectsByProjectIdUsersQuery } from "api/generatedApi";
import { useScreenSize, ScreenSizeEnum } from "shared/use-screen-size";
import { DefaultBack2DashBoardNavigation } from "../navigation/default";
import { useToast } from "shared/toast/hooks/use-toast";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { DesktopProjectUsers } from "./desktop-users";
import { MobileProjectUsers } from "./mobile-users";
import { useCreateProjectUser } from "./create/hooks/use-create-project-user";

interface Props {
  project: GetProjectResponse;
}

export const DefaultProjectParticipants = (props: Props) => {
  const { project } = props;
  const { screenSize } = useScreenSize();
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);

  const [isLoading, setIsLoading] = useState(true);
  const openCreateUser = useCreateProjectUser({ project });
  const { canCreateProjectUsers } = useDashboardRestrictions(project);
  const { data, error } = useGetApiProjectsByProjectIdUsersQuery({ projectId: project?.id ?? "" });

  useEffect(() => {
    if (data) {
      setIsLoading(false);
    }
    if (error) {
      setIsLoading(false);
      console.error(error);
      toastRef.current.error(tRef.current("dashboard.users.getUsers.error"));
    }
  }, [data, error, toastRef, tRef]);

  return (
    <Grid item xs={true} overflow={"auto"} padding={1} height={"100%"}>
      <Box sx={{ height: "100%", display: "flex", flexDirection: "column", justifyContent: "start" }}>
        <DefaultBack2DashBoardNavigation here={t("dashboard.links.users")} />
        {isLoading && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <CircularProgress size={100} />
          </Box>
        )}
        {!isLoading &&
          data?.users &&
          (data?.users ?? []).length > 0 &&
          (screenSize === ScreenSizeEnum.Mobile ? <MobileProjectUsers users={data.users} /> : <DesktopProjectUsers users={data.users} />)}
        {!isLoading && canCreateProjectUsers() && (
          <Fab
            sx={{
              position: "absolute",
              bottom: screenSize === ScreenSizeEnum.Mobile ? "70px" : "30px",
              right: screenSize === ScreenSizeEnum.Mobile ? "20px" : "10px",
              backgroundColor: "primary.dark",
            }}
            onClick={openCreateUser}
          >
            <AddCircleOutlineIcon fontSize="large" sx={{ color: "grey.100", pb: "2px", pr: "2px" }} />
          </Fab>
        )}
      </Box>
    </Grid>
  );
};
