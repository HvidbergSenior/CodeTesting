import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutlineOutlined";
import Box from "@mui/material/Box";
import Fab from "@mui/material/Fab";
import { GetProjectResponse } from "api/generatedApi";
import { useEffect, useState } from "react";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { DraftWorkItem, useOfflineStorage } from "utils/offline-storage/use-offline-storage";
import { DraftDesktop } from "./desktop/draft-desktop";
import { DraftMobile } from "./mobile/draft-mobile";
import { useCreateDraftWorkItem } from "./create/hooks/use-create-draft-work-item";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";

interface Prop {
  project: GetProjectResponse;
  foldersFlatlist: ExtendedProjectFolder[];
}

export const DefaultDraft = (props: Prop) => {
  const { project, foldersFlatlist } = props;
  const { screenSize } = useScreenSize();
  const isOnline = useOnlineStatus();
  const { getOfflineWorkitemDrafts } = useOfflineStorage();
  const [draftWorkItems, setDraftWorkItems] = useState<DraftWorkItem[]>(getOfflineWorkitemDrafts(project.id ?? ""));

  useEffect(() => {
    function preventBackFn() {
      window.history.pushState(null, document.title, window.location.href);
    }
    if (!isOnline) {
      window.history.pushState(null, document.title, window.location.href);
      window.addEventListener("popstate", preventBackFn);
    } else {
      window.removeEventListener("popstate", preventBackFn);
    }
    return () => {
      window.removeEventListener("popstate", preventBackFn);
    };
  }, [isOnline]);

  const updateTable = () => {
    setDraftWorkItems(getOfflineWorkitemDrafts(project.id ?? ""));
  };
  const showCreateDraftWorkItemDialog = useCreateDraftWorkItem({ project, updateTable });

  return (
    <Box
      sx={{
        p: 0,
        pb: 1,
        position: "relative",
        flex: 1,
        overflowY: "hidden",
        display: "flex",
        flexDirection: "column",
        height: "100%",
      }}
    >
      {screenSize === ScreenSizeEnum.Mobile ? (
        <DraftMobile draftWorkItems={draftWorkItems} project={project} updateTable={updateTable} foldersFlatlist={foldersFlatlist} />
      ) : (
        <DraftDesktop draftWorkItems={draftWorkItems} project={project} updateTable={updateTable} foldersFlatlist={foldersFlatlist} />
      )}
      <Fab
        sx={{
          position: "absolute",
          bottom: screenSize === ScreenSizeEnum.Mobile ? "30px" : "30px",
          right: screenSize === ScreenSizeEnum.Mobile ? "10px" : "30px",
          backgroundColor: "primary.dark",
        }}
        onClick={showCreateDraftWorkItemDialog}
      >
        <AddCircleOutlineIcon fontSize="large" sx={{ color: "grey.100", pb: "2px", pr: "2px" }} />
      </Fab>
    </Box>
  );
};
