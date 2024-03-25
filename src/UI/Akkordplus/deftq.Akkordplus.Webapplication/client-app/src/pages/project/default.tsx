import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useRef } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";

import { useGetApiProjectsByProjectIdQuery, useGetApiProjectsByProjectIdFoldersQuery } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { DesktopProjectPage } from "./desktop-project";
import { MobileProjectPage } from "./mobile-projects";
import { ExtendedProjectFolder, useMapTreeToFlat } from "./hooks/use-map-tree-to-flat";
import { useToast } from "shared/toast/hooks/use-toast";
import { RouteSubPage } from "shared/route-types";
import { useUpdateOfflineData } from "./hooks/use-update-offline-data";
interface Props {
  subPage: RouteSubPage;
}

export const DefaultProjectPage = (props: Props) => {
  const { subPage } = props;
  const { folderId } = useParams<"folderId">();
  const { projectId } = useParams<"projectId">();

  const { mapTreeToFlat, mapTreeNode } = useMapTreeToFlat();
  const navigate = useNavigate();
  const { t } = useTranslation();
  useUpdateOfflineData();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);

  const { data: project, isError: getProjectHasError } = useGetApiProjectsByProjectIdQuery({
    projectId: projectId ?? "",
  });

  const { data: rootFolder, isError: getFolderHasError } = useGetApiProjectsByProjectIdFoldersQuery({
    projectId: projectId ?? "",
  });

  let dataFlatlist:
    | {
        folders: ExtendedProjectFolder[] | undefined;
        flatlist: ExtendedProjectFolder[] | undefined;
      }
    | undefined = undefined;
  if (rootFolder && rootFolder.rootFolder) {
    const root = mapTreeNode(rootFolder.rootFolder, undefined, true);
    root.projectFolderName = project?.title;
    dataFlatlist = !root?.subFolders ? undefined : mapTreeToFlat(root.subFolders, root);
    dataFlatlist?.flatlist?.push(root);
    root.flatlist = dataFlatlist?.flatlist;
  }

  useEffect(() => {
    if (getProjectHasError) {
      toastRef.current.error(tRef.current("project.getProjects.error"));
    }
    if (getFolderHasError) {
      toastRef.current.error(tRef.current("project.getProject.error"));
    }
  }, [getProjectHasError, getFolderHasError, toastRef, tRef]);

  const folderSelected = (nodeId: string) => {
    if (project) {
      let url = `../${project.id}/folders/${nodeId}`;
      navigate(url);
    }
  };

  const getSelectedFolder = (): ExtendedProjectFolder | undefined => {
    if (!folderId) {
      return dataFlatlist?.flatlist?.find((folder) => folder.isRoot);
    }
    return dataFlatlist?.flatlist?.find((folder) => folder.projectFolderId === folderId);
  };

  const { screenSize } = useScreenSize();
  if (project && dataFlatlist?.flatlist) {
    return screenSize === ScreenSizeEnum.Mobile ? (
      <MobileProjectPage
        project={project}
        selectedFolder={getSelectedFolder()}
        dataFlatlist={dataFlatlist.flatlist}
        subPage={subPage}
        folderSelectedProps={(nodeId: string) => folderSelected(nodeId)}
      />
    ) : (
      <DesktopProjectPage
        project={project}
        selectedFolder={getSelectedFolder()}
        dataFlatlist={dataFlatlist.flatlist}
        subPage={subPage}
        folderSelectedProps={(nodeId: string) => folderSelected(nodeId)}
      />
    );
  } else {
    return (
      <Box sx={{ width: "100%", height: "100%", display: "flex", justifyContent: "center", alignItems: "center" }}>
        <CircularProgress size={150} />
      </Box>
    );
  }
};
