import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import Fab from "@mui/material/Fab";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import { GetProjectResponse, useGetApiProjectsByProjectIdExtraworkgreementsQuery } from "api/generatedApi";
import { useScreenSize, ScreenSizeEnum } from "shared/use-screen-size";
import { DefaultBack2DashBoardNavigation } from "../navigation/default";
import { useToast } from "shared/toast/hooks/use-toast";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { MobileProjectExtraWorkAgreements } from "./mobile-table/mobile-table";
import { DesktopProjectExtraWorkAgreements } from "./desktop-table/desktop-table";
import { ValueCard } from "components/shared/card/value-card";
import { useCreateExtraWorkAgreement } from "./edit/hooks/use-create-extra-work-agreement";

interface Props {
  project: GetProjectResponse;
}

export const DefaultProjectExtraWorkAgreements = (props: Props) => {
  const { project } = props;
  const { screenSize } = useScreenSize();
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const { data, error } = useGetApiProjectsByProjectIdExtraworkgreementsQuery({ projectId: project?.id ?? "" });
  const [isLoading, setIsLoading] = useState(true);
  const { canCreateFavorites } = useDashboardRestrictions(project);
  const openCreateExtraWorkAgreementDialog = useCreateExtraWorkAgreement({ project });

  useEffect(() => {
    if (data) {
      setIsLoading(false);
    }
    if (error) {
      setIsLoading(false);
      console.error(error);
      toastRef.current.error(tRef.current("dashboard.extraWorkAgreements.getExtraWorkAgreements.error"));
    }
  }, [data, error, toastRef, tRef]);

  const openCreateAgreement = () => {
    openCreateExtraWorkAgreementDialog();
  };

  return (
    <Grid item xs={true} overflow={"auto"} padding={1} height={"100%"}>
      <Box sx={{ height: "100%", display: "flex", flexDirection: "column", justifyContent: "start" }}>
        <DefaultBack2DashBoardNavigation here={t("dashboard.links.extraWorkAgreements")} />
        {isLoading && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <CircularProgress size={100} />
          </Box>
        )}
        {!isLoading && (!data?.extraWorkAgreements || data.extraWorkAgreements.length <= 0) && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <Typography variant="body1" color="grey.100" fontStyle={"italic"}>
              {t("dashboard.extraWorkAgreements.getExtraWorkAgreements.noExtraWorkAgreements")}
            </Typography>
          </Box>
        )}
        {!isLoading && data?.extraWorkAgreements && data.extraWorkAgreements.length > 0 && (
          <Box sx={{ pt: 5, pl: screenSize === ScreenSizeEnum.Mobile ? 0 : 4, pr: screenSize === ScreenSizeEnum.Mobile ? 0 : 4 }}>
            <ValueCard
              titleNamespace="dashboard.links.extraWorkAgreements"
              value={data?.totalPaymentDkr}
              unitNamespace={"common.currency"}
              asCurrency={true}
              showBackground={false}
              fontSize={"large"}
            />
          </Box>
        )}
        {!isLoading &&
          data?.extraWorkAgreements &&
          data.extraWorkAgreements.length > 0 &&
          (screenSize === ScreenSizeEnum.Mobile ? (
            <MobileProjectExtraWorkAgreements project={project} agreements={data.extraWorkAgreements} />
          ) : (
            <DesktopProjectExtraWorkAgreements project={project} agreements={data.extraWorkAgreements} />
          ))}
        {!isLoading && canCreateFavorites() && (
          <Fab
            sx={{
              position: "absolute",
              bottom: screenSize === ScreenSizeEnum.Mobile ? "70px" : "30px",
              right: screenSize === ScreenSizeEnum.Mobile ? "20px" : "10px",
              backgroundColor: "primary.dark",
            }}
            onClick={openCreateAgreement}
          >
            <AddCircleOutlineIcon fontSize="large" sx={{ color: "grey.100", pb: "2px", pr: "2px" }} />
          </Fab>
        )}
      </Box>
    </Grid>
  );
};
