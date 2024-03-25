import ContentPasteGoOutlinedIcon from "@mui/icons-material/ContentPasteGoOutlined";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import LocationCityOutlinedIcon from "@mui/icons-material/LocationCityOutlined";
import NotificationsNoneIcon from "@mui/icons-material/NotificationsNone";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import Typography from "@mui/material/Typography";
import type { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { FolderLockIconSubfolderBadge } from "components/page/folder/lock-folder/folder-lock-icon-subfolder-badge";
import { ProjectBreadcrumbs } from "components/shared/tree/mobile/breadcrumbs/breadcrumbs";
import type { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useNavigate } from "react-router-dom";
import { isAllRatesAndSupplementsInherited } from "utils/base-rate/detect-rate-status";

interface Props {
  project: GetProjectResponse;
  selectedFolder: ExtendedProjectFolder | undefined;
  dataFlatlist: ExtendedProjectFolder[];
  includeGoToContent: boolean;
  folderSelectedProps: (nodeId: string) => void;
}

export const ProjectMobileTree = (props: Props) => {
  const { project, selectedFolder, dataFlatlist, includeGoToContent } = props;
  const navigate = useNavigate();

  const folderSelected = (folder: ProjectFolderResponse) => {
    if (includeGoToContent && (!folder?.subFolders || folder.subFolders.length === 0)) {
      goToContent(folder);
      return;
    }
    if (folder?.projectFolderId) props.folderSelectedProps(folder.projectFolderId);
  };

  const showFolderContent = (event: React.MouseEvent<HTMLElement>, folder: ProjectFolderResponse) => {
    if (!includeGoToContent) {
      return;
    }
    event.stopPropagation();
    goToContent(folder);
  };

  const goToContent = (folder: ProjectFolderResponse) => {
    if (folder?.projectFolderId) {
      let url = `../${project.id}/folders/${folder?.projectFolderId}/foldercontent`;
      navigate(url);
    }
  };

  const isFolderExtraWork = (id?: string): boolean => {
    if (!id) {
      return false;
    }
    const found = dataFlatlist.find((f) => f.projectFolderId === id);
    return !!found && found.folderExtraWork === "ExtraWork";
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column", p: 1 }}>
      {selectedFolder?.isRoot ? (
        <Box sx={{ display: "flex", justifyContent: "space-between" }}>
          <Typography variant="h6" sx={{ display: "flex", alignItems: "center" }}>
            <LocationCityOutlinedIcon
              fontSize="inherit"
              sx={{
                color: (theme) => theme.palette.primary.main,
                marginRight: (theme) => theme.spacing(1),
              }}
            />
            {project.title}
          </Typography>
          <Box>
            {selectedFolder && (
              <FolderLockIconSubfolderBadge folder={selectedFolder} data-testid={`mobile-tree-folder-locked-${selectedFolder.projectFolderName}`} />
            )}
            {includeGoToContent && (
              <IconButton onClick={(event) => showFolderContent(event, selectedFolder)} sx={{ pt: 0, pb: 0, pl: 1.5, m: 0 }}>
                <ContentPasteGoOutlinedIcon />
              </IconButton>
            )}
          </Box>
        </Box>
      ) : (
        <Box sx={{ display: "flex", flexDirection: "row" }}>
          <Box sx={{ flex: 1 }}>
            <ProjectBreadcrumbs
              project={project}
              data={selectedFolder}
              includeGoToContent={includeGoToContent}
              folderSelectedProps={props.folderSelectedProps}
            />
          </Box>
          {selectedFolder && (
            <Box sx={{ pl: 0.5, pr: 1 }}>
              <FolderLockIconSubfolderBadge folder={selectedFolder} data-testid={`mobile-tree-folder-locked-${selectedFolder.projectFolderName}`} />
            </Box>
          )}
        </Box>
      )}

      <List sx={{ marginBottom: (theme) => theme.spacing(1) }}>
        {selectedFolder?.subFolders?.map((folder) => {
          return (
            <ListItem
              dense
              disableGutters
              divider
              key={folder.projectFolderId}
              onClick={() => folderSelected(folder)}
              data-testid={`mobile-tree-${folder.projectFolderName}`}
              sx={{ p: 0, m: 0, borderWidth: 0, height: includeGoToContent ? 45 : 35 }}
            >
              <ListItemIcon>
                <FolderOutlinedIcon color={isFolderExtraWork(folder.projectFolderId) ? "secondary" : "primary"} />
              </ListItemIcon>
              <ListItemText
                primary={
                  <Typography variant="subtitle2" sx={{ lineHeight: "40px" }}>
                    {folder.projectFolderName}
                  </Typography>
                }
              />
              {!isAllRatesAndSupplementsInherited(folder) && <NotificationsNoneIcon sx={{ color: "secondary.main", marginRight: 1 }} />}

              {selectedFolder && (
                <FolderLockIconSubfolderBadge
                  folder={selectedFolder.flatlist?.find((f) => f.projectFolderId === folder.projectFolderId)}
                  data-testid={`mobile-tree-folder-locked-${selectedFolder.flatlist?.find((f) => f.projectFolderId === folder.projectFolderId)}`}
                />
              )}
              {includeGoToContent && (
                <ListItemIcon sx={{ pl: 0.5 }}>
                  <IconButton onClick={(event) => showFolderContent(event, folder)} data-testid={`mobile-tree-content${folder.projectFolderName}`}>
                    <ContentPasteGoOutlinedIcon />
                  </IconButton>
                </ListItemIcon>
              )}
            </ListItem>
          );
        })}
      </List>
    </Box>
  );
};
