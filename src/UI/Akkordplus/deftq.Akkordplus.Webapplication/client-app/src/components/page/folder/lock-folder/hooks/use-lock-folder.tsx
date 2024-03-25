import { useTranslation } from "react-i18next";

import { GetProjectResponse, ProjectFolderResponse, usePostApiProjectsByProjectIdFoldersAndFolderIdLockMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { FolderLock, FolderLockFormData } from "../folder-lock";
import { SubmitHandler } from "react-hook-form";

interface Props {
  project: GetProjectResponse;
  folder: ProjectFolderResponse;
}

export function useLockFolder(props: Props) {
  const { project, folder } = props;
  const { t } = useTranslation();
  const [openLockFolderDialog, closeLockFolderDialog] = useDialog(FolderLock);
  const [lockFolder] = usePostApiProjectsByProjectIdFoldersAndFolderIdLockMutation();

  const wrapMutation = useMutation({
    onSuccess: closeLockFolderDialog,
    successProps: {
      description: t(folder?.projectFolderLocked === "Locked" ? "folder.lock.successOpened.description" : "folder.lock.successLocked.description"),
    },
    errorProps: {
      description: t(folder?.projectFolderLocked === "Locked" ? "folder.lock.errorOpened.description" : "folder.lock.errorLocked.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<FolderLockFormData> = (data) => {
    if (!project || !folder) {
      return;
    }
    const projectId = project.id ?? "";
    const folderId = folder.projectFolderId ?? "";
    const folderLock = folder?.projectFolderLocked === "Locked" ? "Unlocked" : "Locked";
    const recursive = data.recursive ?? false;

    wrapMutation(
      lockFolder({
        projectId,
        folderId,
        updateLockProjectFolderRequest: {
          folderLock,
          recursive,
        },
      })
    );
  };

  const handleClick = () => {
    openLockFolderDialog({ onSubmit: handleOnSubmit, folder });
  };

  return handleClick;
}
