import { Fragment, useEffect, useRef, useState } from "react";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import Fab from "@mui/material/Fab";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import { GetProjectResponse, useGetApiProjectsByProjectIdCompensationsQuery } from "api/generatedApi";
import { useScreenSize, ScreenSizeEnum } from "shared/use-screen-size";
import { DefaultBack2DashBoardNavigation } from "../navigation/default";
import { useToast } from "shared/toast/hooks/use-toast";
import { useTranslation } from "react-i18next";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { DesktopCompensationPayments } from "./desktop-compensation-payments";
import { MobileCompensationPayments } from "./mobile-compensation-payments";
import { useCreateCompensationPayment } from "./create/hooks/use-create-compensation-payment";

interface Props {
  project: GetProjectResponse;
}

export const DefaultCompensationPayments = (props: Props) => {
  const { project } = props;
  const { screenSize } = useScreenSize();
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const { data, error } = useGetApiProjectsByProjectIdCompensationsQuery({ projectId: project?.id ?? "" }, { refetchOnMountOrArgChange: true });
  const [isLoading, setIsLoading] = useState(true);
  const { canViewCompensationPayment, canCreateCompensationPayment } = useDashboardRestrictions(project);
  const openCreateDialog = useCreateCompensationPayment({ project });

  useEffect(() => {
    if (data) {
      setIsLoading(false);
    }
    if (error) {
      setIsLoading(false);
      console.error(error);
      toastRef.current.error(tRef.current("dashboard.compensationPayment.getCompensationPayment.error"));
    }
  }, [data, error, toastRef, tRef]);

  return (
    <Grid item xs={true} overflow={"auto"} padding={1} height={"100%"}>
      <Box sx={{ height: "100%", display: "flex", flexDirection: "column", justifyContent: "start" }}>
        <DefaultBack2DashBoardNavigation here={t("dashboard.links.compensationPayments")} />
        {canViewCompensationPayment() && (
          <Fragment>
            {isLoading && (
              <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
                <CircularProgress size={100} />
              </Box>
            )}
            {!isLoading && (!data?.compensations || data.compensations.length <= 0) && (
              <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
                <Typography variant="body1" color="grey.100" fontStyle={"italic"}>
                  {t("dashboard.compensationPayments.getCompensationPayments.noCompensationPayments")}
                </Typography>
              </Box>
            )}
            {!isLoading &&
              data?.compensations &&
              (data?.compensations ?? []).length > 0 &&
              (screenSize === ScreenSizeEnum.Mobile ? (
                <MobileCompensationPayments project={project} compensations={data.compensations} />
              ) : (
                <DesktopCompensationPayments project={project} compensations={data.compensations} />
              ))}
            {!isLoading && canCreateCompensationPayment() && (
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
          </Fragment>
        )}
        {!canViewCompensationPayment() && (
          <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "center", alignItems: "center" }}>
            <Typography variant="body2" color={"grey.100"}>
              {t("dashboard.compensationPayments.restrictionError")}
            </Typography>
          </Box>
        )}
      </Box>
    </Grid>
  );
};
