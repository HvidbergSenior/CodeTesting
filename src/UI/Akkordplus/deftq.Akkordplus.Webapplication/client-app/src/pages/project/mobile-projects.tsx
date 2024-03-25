import type { GetProjectResponse } from "api/generatedApi";
import { FolderContent } from "components/page/folder/content/content";
import { ProjectDashboard } from "components/page/projects/dashboard/dashboard";
import { DefaultProjectExtraWorkAgreements } from "components/page/projects/dashboard/extra-work-agreements/default";
import { DefaultProjectFavorites } from "components/page/projects/dashboard/favorites/default";
import { DefaultProjectInfo } from "components/page/projects/dashboard/project-info/default";
import { DefaultDraft } from "components/page/projects/draft/default";
import { ProjectLogbook } from "components/page/projects/logbook/logbook";
import { OfflineDialog } from "components/shared/offline-dialog/offline-dialog";
import { ProjectMobileTree } from "components/shared/tree/mobile/tree/tree";
import { Fragment } from "react";
import { RouteSubPage } from "shared/route-types";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import type { ExtendedProjectFolder } from "./hooks/use-map-tree-to-flat";
import { useNavigate } from "react-router-dom";
import { DefaultReports } from "components/page/projects/dashboard/reports/reports";
import { DefaultProjectParticipants } from "components/page/projects/dashboard/users/default";
import { DefaultCompensationPayments } from "components/page/projects/dashboard/compensation-payments/default";
import { DefaultProjectSpecificOperations } from "components/page/projects/dashboard/project-specific-operations/default";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder | undefined;
  dataFlatlist: ExtendedProjectFolder[];
  subPage: RouteSubPage;
  folderSelectedProps: (nodeId: string) => void;
}

export const MobileProjectPage = (props: Props) => {
  const { project, subPage, selectedFolder, dataFlatlist } = props;
  const isOnline = useOnlineStatus();
  const navigate = useNavigate();

  const goToDraft = () => {
    let url = `../${project.id}/${"draft"}`;
    navigate(url);
  };

  return (
    <Fragment>
      {subPage !== "draft" && <OfflineDialog isOpen={!isOnline} onClose={goToDraft} />}
      {subPage === "folders" && (
        <ProjectMobileTree
          project={project}
          selectedFolder={selectedFolder}
          dataFlatlist={dataFlatlist}
          includeGoToContent={true}
          folderSelectedProps={(nodeId: string) => props.folderSelectedProps(nodeId)}
        />
      )}
      {subPage === "dashboard" && <ProjectDashboard project={project} />}
      {subPage === "dashboard-favorites" && <DefaultProjectFavorites project={project} />}
      {subPage === "dashboard-compensation-payment" && <DefaultCompensationPayments project={project} />}
      {subPage === "dashboard-extra-work-agreements" && <DefaultProjectExtraWorkAgreements project={project} />}
      {subPage === "dashboard-project-info" && selectedFolder && <DefaultProjectInfo project={project} selectedFolder={selectedFolder} />}
      {subPage === "dashboard-reports" && <DefaultReports project={project} />}
      {subPage === "dashboard-users" && <DefaultProjectParticipants project={project} />}
      {subPage === "dashboard-project-specific-operations" && <DefaultProjectSpecificOperations project={project} />}
      {subPage === "logbook" && <ProjectLogbook project={project} />}
      {subPage === "foldercontent" && selectedFolder && <FolderContent folder={selectedFolder} project={project} dataFlatlist={dataFlatlist} />}
      {subPage === "draft" && <DefaultDraft project={project} foldersFlatlist={dataFlatlist} />}
    </Fragment>
  );
};
