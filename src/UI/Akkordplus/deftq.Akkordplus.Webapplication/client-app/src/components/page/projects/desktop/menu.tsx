import { Box, Divider, Grid, Typography } from "@mui/material";
import { ProjectTree } from "components/shared/tree/desktop/tree";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";

import CalendarMonthOutlinedIcon from "@mui/icons-material/CalendarMonthOutlined";
import DashboardOutlinedIcon from "@mui/icons-material/DashboardOutlined";
import TextSnippetOutlinedIcon from "@mui/icons-material/TextSnippetOutlined";
import type { GetProjectResponse } from "api/generatedApi";
import { RouteSubPage } from "shared/route-types";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";
import { OverlaySideMenu } from "./overlay/disable-overlay";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder | undefined;
  dataFlatlist: ExtendedProjectFolder[];
  subPage: RouteSubPage;
  folderSelectedProps: (nodeId: string) => void;
  menuSelectedProps: (page: RouteSubPage) => void;
}

export const ProjectMenu = (props: Props) => {
  const { project, selectedFolder, dataFlatlist, subPage } = props;
  const { t } = useTranslation();
  const { canSeeFolderMenu } = useFolderRestrictions(project);
  const menuItemClicked = (page: RouteSubPage) => {
    props.menuSelectedProps(page);
  };

  const menuSelectedBackground = "rgba(61, 85, 124, 0.1)";
  const menuNotSelectedBackground = "transparent";

  const dashBoardSelected = (): string => {
    if (
      subPage === "dashboard" ||
      subPage === "dashboard-compensation-payment" ||
      subPage === "dashboard-extra-work-agreements" ||
      subPage === "dashboard-favorites" ||
      subPage === "dashboard-project-info" ||
      subPage === "dashboard-reports" ||
      subPage === "dashboard-users"
    ) {
      return menuSelectedBackground;
    }
    return menuNotSelectedBackground;
  };

  const logBookSelected = (): string => {
    if (subPage === "logbook") {
      return menuSelectedBackground;
    }
    return menuNotSelectedBackground;
  };

  const draftSelected = (): string => {
    if (subPage === "draft") {
      return menuSelectedBackground;
    }
    return menuNotSelectedBackground;
  };

  const activeFolder = (): ExtendedProjectFolder | undefined => {
    if (subPage === "folders" || subPage === "foldercontent") {
      return selectedFolder;
    }
    return undefined;
  };

  return (
    <Grid item sm={4} lg={3} xl={2.5} sx={{ height: "100%", backgroundColor: "primary.light", overflowY: "auto" }}>
      <Box
        onClick={() => menuItemClicked("dashboard")}
        sx={{
          display: "flex",
          alignItems: "center",
          p: 1,
          pl: "15px",
          mt: "30px",
          cursor: "pointer",
          background: dashBoardSelected(),
        }}
      >
        <DashboardOutlinedIcon color="primary" />
        <Typography variant="h6" color="primary.main" sx={{ paddingLeft: "10px" }}>
          {t("dashboard.title")}
        </Typography>
      </Box>
      <Box
        onClick={() => menuItemClicked("logbook")}
        sx={{
          display: "flex",
          alignItems: "center",
          p: 1,
          pl: "15px",
          cursor: "pointer",
          background: logBookSelected(),
        }}
      >
        <CalendarMonthOutlinedIcon color="primary" />
        <Typography variant="h6" color="primary.main" sx={{ paddingLeft: "10px" }}>
          {t("logbook.title")}
        </Typography>
      </Box>
      <Box
        onClick={() => menuItemClicked("draft")}
        sx={{
          display: "flex",
          alignItems: "center",
          p: 1,
          pl: "15px",
          mb: "5px",
          cursor: "pointer",
          background: draftSelected,
        }}
      >
        <TextSnippetOutlinedIcon color="primary" />
        <Typography variant="h6" color="primary.main" sx={{ paddingLeft: "10px" }}>
          {t("draft.title")}
        </Typography>
      </Box>

      <Divider />
      <ProjectTree
        project={project}
        dataFlatlist={dataFlatlist}
        selectedFolder={activeFolder()}
        includeMenu={canSeeFolderMenu()}
        folderSelectedProps={(nodeId: string) => props.folderSelectedProps(nodeId)}
      />
      <OverlaySideMenu selectDraft={menuItemClicked} />
    </Grid>
  );
};
