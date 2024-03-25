import { SyntheticEvent, useState } from "react";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import TreeView from "@mui/lab/TreeView";
import TreeItem from "@mui/lab/TreeItem";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ExpandLessIcon from "@mui/icons-material/ExpandLess";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import MoreHorizIcon from "@mui/icons-material/MoreHoriz";
import NotificationsNoneIcon from "@mui/icons-material/NotificationsNone";
import LocationCityOutlinedIcon from "@mui/icons-material/LocationCityOutlined";

import type { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { ProjectPopupMenu } from "components/shared/tree/desktop/popup-menu/popup-menu";
import type { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { FolderLockIconSubfolderBadge } from "components/page/folder/lock-folder/folder-lock-icon-subfolder-badge";
import { isAllRatesAndSupplementsInherited } from "utils/base-rate/detect-rate-status";
import Tooltip from "@mui/material/Tooltip";
import { useTranslation } from "react-i18next";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder | undefined;
  dataFlatlist: ExtendedProjectFolder[];
  includeMenu: boolean;
  folderSelectedProps: (nodeId: string) => void;
}

export const ProjectTree = (props: Props) => {
  const { project, dataFlatlist, selectedFolder, includeMenu } = props;
  const [selectedNodeId, setSelectedNodeId] = useState<string | undefined>(selectedFolder?.projectFolderId);
  const { t } = useTranslation();
  const [anchorEl, setAnchorEl] = useState<HTMLElement | undefined>(undefined);
  const [activeFolder, setActiveFolder] = useState<ProjectFolderResponse | undefined>(undefined);
  const rootFolder = dataFlatlist?.find((f) => f.isRoot);

  const folderSelected = (event: SyntheticEvent, nodeIds: Array<string> | string) => {
    const nodeId = Array.isArray(nodeIds) ? nodeIds[0] : nodeIds;
    if (selectedNodeId === nodeId) {
      return;
    }
    setSelectedNodeId(nodeId);
    props.folderSelectedProps(nodeId);
  };

  const getExpandedFolders = (): string[] => {
    const folders: string[] = [];
    if (rootFolder?.projectFolderId) {
      folders.push(rootFolder?.projectFolderId);
    }
    if (selectedFolder) {
      folders.push(selectedFolder.projectFolderId ?? "");
      let parent = selectedFolder.parent;
      while (parent) {
        folders.push(parent?.projectFolderId ?? "");
        parent = parent?.parent;
      }
    }
    return folders;
  };

  const actionMenuClicked = (e: React.MouseEvent<HTMLButtonElement>, folder?: ProjectFolderResponse) => {
    if (!includeMenu) {
      return;
    }
    e.stopPropagation();
    setActiveFolder(folder);
    setAnchorEl(e.currentTarget);
  };

  const isFolderExtraWork = (id?: string): boolean => {
    if (!id) {
      return false;
    }
    const found = dataFlatlist.find((f) => f.projectFolderId === id);
    return !!found && found.folderExtraWork === "ExtraWork";
  };

  const renderTree = (folder: ProjectFolderResponse) => (
    <TreeItem
      data-testid={`tree-${folder.projectFolderName}`}
      key={folder.projectFolderId}
      nodeId={folder.projectFolderId ? folder.projectFolderId : ""}
      label={
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
          }}
        >
          <FolderOutlinedIcon color={isFolderExtraWork(folder.projectFolderId) ? "secondary" : "primary"} />
          <Box sx={{ paddingLeft: "10px", flexGrow: 1, minWidth: 0 }}>
            <Typography
              variant="body2"
              color="primary.main"
              sx={{ lineHeight: "14px", paddingTop: "3px", overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}
            >
              {folder.projectFolderName}
            </Typography>
          </Box>
          {!isAllRatesAndSupplementsInherited(folder) && (
            <Tooltip title={t("folder.baseRate.notification")}>
              <NotificationsNoneIcon sx={{ color: "secondary.main", marginRight: 1 }} />
            </Tooltip>
          )}
          {folder && (
            <FolderLockIconSubfolderBadge
              folder={dataFlatlist?.find((fl) => fl.projectFolderId === folder.projectFolderId)}
              data-testid={`tree-folder-locked-${folder.projectFolderName}`}
            />
          )}
          {includeMenu && (
            <IconButton onClick={(e) => actionMenuClicked(e, folder)} data-testid={`tree-action-menu-${folder.projectFolderName}`}>
              <MoreHorizIcon fontSize="small" color="primary" />
            </IconButton>
          )}
        </Box>
      }
    >
      {folder.subFolders && folder.subFolders.length > 0 ? folder.subFolders.map((node) => renderTree(node)) : null}
    </TreeItem>
  );

  return (
    <TreeView
      defaultCollapseIcon={<ExpandMoreIcon />}
      defaultExpandIcon={<ChevronRightIcon />}
      expanded={getExpandedFolders()}
      selected={selectedNodeId}
      multiSelect={false}
      onNodeSelect={folderSelected}
      color="primary.dark"
    >
      <TreeItem
        nodeId={rootFolder?.projectFolderId ? rootFolder?.projectFolderId : "unknown"}
        expandIcon={<ExpandLessIcon color="primary" />}
        sx={{
          "&:first-of-type ": {
            "> .MuiTreeItem-content": {
              padding: "5px 8px 5px 18px",

              "> .MuiTreeItem-iconContainer > svg": {
                fontSize: "24px",
                color: "rgba(0, 0, 0, 0.54)",
              },
            },
          },
        }}
        label={
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              paddingY: "2px",
            }}
            data-testid={`tree-${project.title}`}
          >
            <LocationCityOutlinedIcon color="primary" />
            <Box sx={{ paddingLeft: "10px", flexGrow: 1 }}>
              <Typography variant="h6" color="primary.main">
                {project.title}
              </Typography>
            </Box>
            <FolderLockIconSubfolderBadge folder={rootFolder} data-testid={`tree-folder-locked-${project.title}`} />
            {includeMenu && (
              <IconButton onClick={(e) => actionMenuClicked(e, rootFolder)} data-testid={`tree-action-menu-${project.title}`}>
                <MoreHorizIcon fontSize="small" color="primary" />
              </IconButton>
            )}
          </Box>
        }
      >
        {rootFolder?.subFolders ? rootFolder.subFolders.map((node) => renderTree(node)) : null}
      </TreeItem>
      {rootFolder && activeFolder && (
        <ProjectPopupMenu
          project={project}
          dataFlatlist={dataFlatlist}
          selectedFolder={activeFolder}
          anchorEl={anchorEl}
          onClose={() => setAnchorEl(undefined)}
        />
      )}
    </TreeView>
  );
};
