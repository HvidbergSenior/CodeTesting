import { IconButton } from "@mui/material";
import { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { useLockFolder } from "./hooks/use-lock-folder";
import { TextIcon } from "components/shared/text-icon/text-icon";
import { FolderLockIcon } from "./folder-lock-icon";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";

export interface Props {
  project: GetProjectResponse;
  folder: ProjectFolderResponse;
  showText: boolean;
}

export function FolderLockedButton(props: Props) {
  const { project, folder, showText } = props;
  const { canLockFolder } = useFolderRestrictions(project);

  const openDialog = useLockFolder({ project, folder });

  const text = showText ? (folder?.projectFolderLocked === "Locked" ? "folder.lock.locked.iconText" : "folder.lock.open.iconText") : undefined;

  const changeLockState = () => {
    if (!canLockFolder()) {
      return;
    }
    openDialog();
  };

  return (
    <TextIcon translateText={text}>
      <IconButton onClick={changeLockState} disabled={!canLockFolder()}>
        <FolderLockIcon folder={folder} disabled={!canLockFolder()} />
      </IconButton>
    </TextIcon>
  );
}
