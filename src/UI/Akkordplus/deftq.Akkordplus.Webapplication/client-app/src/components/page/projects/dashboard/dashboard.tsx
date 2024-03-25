import { useTranslation } from "react-i18next";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import { GetProjectResponse, useGetApiProjectsByProjectIdSummationQuery } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { ProjectDashboardLink } from "./dashboard-link";
import { SummationsDashboardMobile } from "./summations/summations-dashboard-mobile";
import SummationsDashboardDesktop from "./summations/summations-dashboard-desktop";
import { useEffect, useRef } from "react";
import { useToast } from "shared/toast/hooks/use-toast";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";

interface Props {
  project: GetProjectResponse;
}

export const ProjectDashboard = (props: Props) => {
  const { project } = props;
  const { screenSize } = useScreenSize();
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const maxWidth = screenSize === ScreenSizeEnum.LargeDesktop ? "1200px" : "900px";
  const pagePadding = screenSize === ScreenSizeEnum.LargeDesktop ? 4.5 : 1;
  const spacing = 2;
  const { canViewCompensationPayment } = useDashboardRestrictions(project);
  const { data: summaryData, isError: summaryError } = useGetApiProjectsByProjectIdSummationQuery({ projectId: project.id ?? "" });

  useEffect(() => {
    if (summaryError) {
      toastRef.current.error(tRef.current("dashboard.summary.getSummaryError"));
    }
  }, [summaryError, tRef, toastRef]);

  return (
    <Grid item xs={true} overflow={"auto"} padding={pagePadding} pb={4} sx={{ width: "100%", height: "100%" }}>
      <Grid container alignItems={"center"} maxWidth={maxWidth} marginLeft={"auto"} marginRight={"auto"} rowSpacing={spacing}>
        <Grid item xs={12}>
          <Typography variant="h5" color="primary" pt={2} pb={2}>
            {project.title}
          </Typography>
        </Grid>
        {screenSize === ScreenSizeEnum.Mobile && summaryData && <SummationsDashboardMobile summaryData={summaryData} project={project} />}
        {screenSize !== ScreenSizeEnum.Mobile && summaryData && (
          <SummationsDashboardDesktop summaryData={summaryData} project={project} usePrintlayOut={false} />
        )}
        <Grid container item xs={12}>
          <Typography variant="h5" color="primary" pt={2} pb={2}>
            {t("dashboard.linkHeader")}
          </Typography>
        </Grid>
        <Grid container item xs={12} spacing={spacing}>
          <Grid item xs={6} lg={3} xl={3}>
            <ProjectDashboardLink url={`/projects/${project.id}/dashboard/project-info`} text={t("dashboard.links.projectInfo")} />
          </Grid>
          <Grid item xs={6} lg={3} xl={3}>
            <ProjectDashboardLink
              url={canViewCompensationPayment() ? `/projects/${project.id}/dashboard/compensation-payment` : undefined}
              text={t("dashboard.links.compensationPayments")}
            />
          </Grid>
          <Grid item xs={6} lg={3} xl={3}>
            <ProjectDashboardLink url={`/projects/${project.id}/dashboard/extraworkagreements`} text={t("dashboard.links.extraWorkAgreements")} />
          </Grid>
          <Grid item xs={6} lg={3} xl={3}>
            <ProjectDashboardLink url={`/projects/${project.id}/dashboard/favorites`} text={t("dashboard.links.favorits")} />
          </Grid>
        </Grid>
        <Grid container item xs={12} spacing={spacing}>
          <Grid item xs={6} lg={3} xl={3}>
            <ProjectDashboardLink url={`/projects/${project.id}/dashboard/users`} text={t("dashboard.links.users")} />
          </Grid>
          <Grid item xs={6} lg={3} xl={3}>
            <ProjectDashboardLink
              url={`/projects/${project.id}/dashboard/projectspecificoperations`}
              text={t("dashboard.links.projectSpecificOperations")}
            />
          </Grid>
          <Grid item xs={6} lg={3} xl={3}>
            <ProjectDashboardLink url={`/projects/${project.id}/dashboard/reports`} text={t("dashboard.links.reports")} />
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
};
