import { Box, CircularProgress } from "@mui/material";
import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { GetProjectResponse, useGetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsQuery, WorkItemResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useToast } from "shared/toast/hooks/use-toast";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { DesktopFolderWorkItems } from "./desktop/desktop-folder-work-items";
import { MobileFolderWorkItems } from "./mobile/mobile-folder-work-items";

interface Prop {
  folder: ExtendedProjectFolder;
  project: GetProjectResponse;
  dataFlatlist: ExtendedProjectFolder[];
}

export const FolderWorkitems = (props: Prop) => {
  const { folder, project, dataFlatlist } = props;
  const { screenSize } = useScreenSize();
  const { t } = useTranslation();
  const toast = useToast();
  const toastRef = useRef(toast);
  const [loading, setLoading] = useState(true);
  const { data: workItems, error: workItemsError } = useGetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsQuery(
    {
      folderId: folder.projectFolderId ?? "",
      projectId: project.id ?? "",
    },
    { refetchOnMountOrArgChange: true }
  );
  const [loadedWorkItems, setLoadedWorkItems] = useState<WorkItemResponse[] | undefined>(undefined);

  useEffect(() => {
    if (workItems) {
      setLoadedWorkItems(workItems?.workItems ?? []);
      setLoading(false);
    }
    if (workItemsError) {
      toastRef.current.error(t("content.measurements.getWorkItemsError"));
    }
  }, [workItems, workItemsError, toastRef, t]);

  return (
    <Box sx={{ width: "100%", height: "100%" }}>
      {loading && (
        <Box sx={{ width: "100%", height: "100%", display: "flex", justifyContent: "center", alignItems: "center" }}>
          <CircularProgress size={150} />
        </Box>
      )}
      {!loading && screenSize === ScreenSizeEnum.Mobile ? (
        <MobileFolderWorkItems project={project} selectedFolder={folder} workItems={loadedWorkItems} dataFlatlist={dataFlatlist} />
      ) : (
        <DesktopFolderWorkItems project={project} selectedFolder={folder} workItems={loadedWorkItems} dataFlatlist={dataFlatlist} />
      )}
    </Box>
  );
};
