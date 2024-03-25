import ContentCopyOutlinedIcon from "@mui/icons-material/ContentCopyOutlined";
import CreateNewFolderOutlinedIcon from "@mui/icons-material/CreateNewFolderOutlined";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";
import DriveFileMoveOutlinedIcon from "@mui/icons-material/DriveFileMoveOutlined";
import EditOutlinedIcon from "@mui/icons-material/EditOutlined";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import LinkOutlinedIcon from "@mui/icons-material/LinkOutlined";
import LocationCityOutlinedIcon from "@mui/icons-material/LocationCityOutlined";
import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import Typography from "@mui/material/Typography";
import type { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { PUBLIC_URL } from "api/path";
import { useCopyFolder } from "components/page/folder/copy/hooks/use-copy";
import { useCreateFolder } from "components/page/folder/create-edit/hooks/create/use-create-folder";
import { useEditFolderName } from "components/page/folder/create-edit/hooks/edit-folder-name/use-edit-folder-name";
import { useDeleteFolder } from "components/page/folder/hooks/use-delete-folder";
import { FolderInfo } from "components/page/folder/info/info";
import { useMoveFolder } from "components/page/folder/move/hooks/use-move";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { Fragment } from "react";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { useToast } from "shared/toast/hooks/use-toast";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";

interface ProjectPopupProps {
  project: GetProjectResponse;
  selectedFolder: ProjectFolderResponse;
  dataFlatlist: ExtendedProjectFolder[];
  anchorEl: HTMLElement | undefined;
  onClose: () => void;
}

export function ProjectPopupMenu(props: ProjectPopupProps) {
  const { project, selectedFolder, dataFlatlist, anchorEl, onClose } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canDeleteFolder, canEditFolder, canCreateFolder, canCopyFolder, canMoveFolder } = useFolderRestrictions(project);

  const [openInfoDialog] = useDialog(FolderInfo);
  const openCreateFolderDialog = useCreateFolder({
    project,
    folder: selectedFolder,
  });
  const openEditFolderNameDialog = useEditFolderName({
    project,
    folder: selectedFolder,
  });
  const openMoveFolderDialog = useMoveFolder({ project, selectedFolder, dataFlatlist });
  const openCopyFolderDialog = useCopyFolder({ project, selectedFolder, dataFlatlist });

  const openDeleteFolderDialog = useDeleteFolder({ project, folder: selectedFolder });

  const clickHandler = (popupMenuId: string) => {
    switch (popupMenuId) {
      case "info":
        openInfoDialog({ folder: selectedFolder });
        break;
      case "edit":
        openEditFolderNameDialog();
        break;
      case "link":
        if (selectedFolder) {
          navigator.clipboard.writeText(PUBLIC_URL + "projects/" + project.id + "/folders/" + selectedFolder.projectFolderId);
          toast.success(t("folder.link.success"));
        }
        break;
      case "move":
        openMoveFolderDialog();
        break;
      case "copy":
        openCopyFolderDialog();
        break;
      case "create":
        openCreateFolderDialog();
        break;
      case "delete":
        openDeleteFolderDialog();
        break;
    }
    onClose();
  };

  return (
    <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={onClose}>
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          p: 2,
          pt: 1.5,
          minWidth: 220,
        }}
      >
        <ListItemIcon sx={{ minWidth: "36px" }}>
          {selectedFolder ? <FolderOutlinedIcon sx={{ color: "text.primary" }} /> : <LocationCityOutlinedIcon sx={{ color: "text.primary" }} />}
        </ListItemIcon>
        <Typography sx={{ fontWeight: "bold" }}>{selectedFolder ? selectedFolder.projectFolderName : project.title}</Typography>
      </Box>
      <Divider sx={{ mb: 1 }} />
      {selectedFolder && (
        <Fragment>
          <MenuItem onClick={() => clickHandler("info")}>
            <ListItemIcon>
              <InfoOutlinedIcon />
            </ListItemIcon>
            <ListItemText>{t("folder.menu.info")}</ListItemText>
          </MenuItem>
          {canEditFolder() && (
            <MenuItem onClick={() => clickHandler("edit")}>
              <ListItemIcon>
                <EditOutlinedIcon />
              </ListItemIcon>
              <ListItemText>{t("common.rename")}</ListItemText>
            </MenuItem>
          )}
          <Divider />
          <MenuItem onClick={() => clickHandler("link")}>
            <ListItemIcon>
              <LinkOutlinedIcon />
            </ListItemIcon>
            <ListItemText>{t("folder.menu.link")}</ListItemText>
          </MenuItem>
          {canMoveFolder() && (
            <MenuItem onClick={() => clickHandler("move")}>
              <ListItemIcon>
                <DriveFileMoveOutlinedIcon />
              </ListItemIcon>
              <ListItemText>{t("common.moveTo")}</ListItemText>
            </MenuItem>
          )}
          {canCopyFolder() && (
            <MenuItem onClick={() => clickHandler("copy")}>
              <ListItemIcon>
                <ContentCopyOutlinedIcon />
              </ListItemIcon>
              <ListItemText>{t("common.copyTo")}</ListItemText>
            </MenuItem>
          )}
        </Fragment>
      )}
      {canCreateFolder() && (
        <Fragment>
          <Divider />
          <MenuItem onClick={() => clickHandler("create")}>
            <ListItemIcon>
              <CreateNewFolderOutlinedIcon />
            </ListItemIcon>
            <ListItemText>{t("common.create")}</ListItemText>
          </MenuItem>
        </Fragment>
      )}
      {canDeleteFolder(selectedFolder) && (
        <MenuItem onClick={() => clickHandler("delete")}>
          <ListItemIcon>
            <DeleteOutlinedIcon />
          </ListItemIcon>
          <ListItemText>{t("common.delete")}</ListItemText>
        </MenuItem>
      )}
    </Menu>
  );
}
