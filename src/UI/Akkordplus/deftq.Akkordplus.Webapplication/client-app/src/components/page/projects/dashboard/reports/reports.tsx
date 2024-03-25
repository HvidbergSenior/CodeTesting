import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import { useTranslation } from "react-i18next";
import { GetProjectResponse } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { DefaultBack2DashBoardNavigation } from "../navigation/default";
import { ProjectReport } from "./project-report/project-report";
import { LogbookReport } from "./logbook-report";
import { WorkItemsReport } from "./work-items-report";
import { StatusReport } from "./status-report";

interface Props {
  project: GetProjectResponse;
}

export const DefaultReports = (props: Props) => {
  const { project } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const maxWidth = screenSize === ScreenSizeEnum.LargeDesktop ? "1200px" : "900px";
  const spacing = 3;

  return (
    <Box sx={{ p: 0, pb: 1, position: "relative", flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", height: "100%" }}>
      <DefaultBack2DashBoardNavigation here={t("dashboard.links.reports")} />
      <Box sx={{ p: 2, overflow: "auto", height: "100%" }}>
        <Grid container alignItems={"center"} maxWidth={maxWidth} marginLeft={"auto"} marginRight={"auto"} rowSpacing={spacing} overflow={"auto"}>
          <Grid container item xs={12} spacing={spacing} pb={9}>
            <Grid container alignItems={"center"} item spacing={spacing}>
              <Grid item xs={12} lg={5} xl={5} display={"flex"} justifyContent={"center"}>
                <ProjectReport project={project} />
              </Grid>
              <Grid item xs={12} lg={5} xl={5} display={"flex"} justifyContent={"center"}>
                <LogbookReport project={project} />
              </Grid>
            </Grid>
          </Grid>
          <Grid container item xs={12} spacing={spacing} pb={9}>
            <Grid container alignItems={"center"} item spacing={spacing}>
              <Grid item xs={12} lg={5} xl={5} display={"flex"} justifyContent={"center"}>
                <WorkItemsReport project={project} />
              </Grid>
              <Grid item xs={12} lg={5} xl={5} display={"flex"} justifyContent={"center"}>
                <StatusReport project={project} />
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </Box>
    </Box>
  );
};
