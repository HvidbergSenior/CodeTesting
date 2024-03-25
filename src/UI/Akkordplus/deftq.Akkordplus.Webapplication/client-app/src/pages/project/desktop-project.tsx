import { Grid } from "@mui/material";
import type { GetProjectResponse } from "api/generatedApi";
import { FolderContent } from "components/page/folder/content/content";
import { ProjectDashboard } from "components/page/projects/dashboard/dashboard";
import { DefaultProjectExtraWorkAgreements } from "components/page/projects/dashboard/extra-work-agreements/default";
import { DefaultProjectFavorites } from "components/page/projects/dashboard/favorites/default";
import { DefaultProjectParticipants } from "components/page/projects/dashboard/users/default";
import { DefaultProjectInfo } from "components/page/projects/dashboard/project-info/default";
import { DefaultReports } from "components/page/projects/dashboard/reports/reports";
import { ProjectMenu } from "components/page/projects/desktop/menu";
import { DefaultDraft } from "components/page/projects/draft/default";
import { ProjectLogbook } from "components/page/projects/logbook/logbook";
import { OfflineDialog } from "components/shared/offline-dialog/offline-dialog";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useNavigate } from "react-router-dom";
import { RouteSubPage } from "shared/route-types";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { DefaultCompensationPayments } from "components/page/projects/dashboard/compensation-payments/default";
import { DefaultProjectSpecificOperations } from "components/page/projects/dashboard/project-specific-operations/default";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder | undefined;
  dataFlatlist: ExtendedProjectFolder[];
  subPage: RouteSubPage;
  folderSelectedProps: (nodeId: string) => void;
}

export const DesktopProjectPage = (props: Props) => {
  const { project, selectedFolder, dataFlatlist, subPage } = props;
  const navigate = useNavigate();
  const isOnline = useOnlineStatus();

  let selectedSubPage = subPage;

  const menuSelected = (page: RouteSubPage) => {
    selectedSubPage = page;
    let url = `../${project.id}/${page}`;
    navigate(url);
  };

  const goToDraft = () => {
    menuSelected("draft");
  };

  return (
    <Grid container direction="row" height="100%">
      {selectedSubPage !== "draft" && <OfflineDialog isOpen={!isOnline} onClose={goToDraft} />}
      <ProjectMenu
        selectedFolder={selectedFolder}
        dataFlatlist={dataFlatlist}
        project={project}
        subPage={subPage}
        folderSelectedProps={(nodeId: string) => props.folderSelectedProps(nodeId)}
        menuSelectedProps={(page: RouteSubPage) => menuSelected(page)}
      />
      {selectedSubPage === "dashboard" && <ProjectDashboard project={project} />}
      {selectedSubPage === "dashboard-favorites" && <DefaultProjectFavorites project={project} />}
      {selectedSubPage === "dashboard-compensation-payment" && <DefaultCompensationPayments project={project} />}
      {selectedSubPage === "dashboard-extra-work-agreements" && <DefaultProjectExtraWorkAgreements project={project} />}
      {selectedSubPage === "dashboard-project-info" && selectedFolder && <DefaultProjectInfo project={project} selectedFolder={selectedFolder} />}
      {selectedSubPage === "dashboard-reports" && <DefaultReports project={project} />}
      {selectedSubPage === "dashboard-users" && <DefaultProjectParticipants project={project} />}
      {selectedSubPage === "dashboard-project-specific-operations" && <DefaultProjectSpecificOperations project={project} />}
      {selectedSubPage === "folders" && selectedFolder && <FolderContent folder={selectedFolder} project={project} dataFlatlist={dataFlatlist} />}
      {selectedSubPage === "foldercontent" && selectedFolder && (
        <FolderContent folder={selectedFolder} project={project} dataFlatlist={dataFlatlist} />
      )}
      {selectedSubPage === "logbook" && <ProjectLogbook project={project} />}
      {selectedSubPage === "draft" && <DefaultDraft project={project} foldersFlatlist={dataFlatlist} />}
    </Grid>
  );
};
