import { useTranslation } from "react-i18next";
import { GetProjectResponse, usePostApiProjectsByProjectIdFoldersAndFolderIdExtraworkMutation } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useDialog } from "shared/dialog/use-dialog";
import { useToast } from "shared/toast/hooks/use-toast";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { FolderExtraWork } from "../extra-work";

interface Props {
  project: GetProjectResponse;
  folder: ExtendedProjectFolder;
}

export function useExtraWorkFolder(props: Props) {
  const { project, folder } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canMarkExtraWork } = useFolderRestrictions(project);
  const [openExtraWorkFolderDialog, closeExtraWorkFolderDialog] = useDialog(FolderExtraWork);
  const [extraWorkFolder] = usePostApiProjectsByProjectIdFoldersAndFolderIdExtraworkMutation();

  const wrapMutation = useMutation({
    onSuccess: closeExtraWorkFolderDialog,
    successProps: {
      description: t(
        folder?.folderExtraWork === "ExtraWork" ? "folder.extraWork.normal.success.description" : "folder.extraWork.extraWork.success.description"
      ),
    },
    errorProps: {
      description: t(
        folder?.folderExtraWork === "ExtraWork" ? "folder.extraWork.normal.error.description" : "folder.extrawork.extraWork.error.description"
      ),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    if (!project || !folder) {
      return;
    }
    const projectId = project.id ?? "";
    const folderId = folder.projectFolderId ?? "";

    wrapMutation(
      extraWorkFolder({
        projectId,
        folderId,
        updateProjectFolderExtraWorkRequest: {
          folderExtraWorkUpdate: folder?.folderExtraWork === "ExtraWork" ? "NormalWork" : "ExtraWork",
        },
      })
    );
  };

  const handleClick = () => {
    if (!canMarkExtraWork(folder)) {
      toast.error(
        t(folder?.folderExtraWork === "ExtraWork" ? "folder.extraWork.normal.restrictionError" : "folder.extraWork.extraWork.restrictionError")
      );
      return;
    }
    openExtraWorkFolderDialog({ onSubmit: handleOnSubmit, folder });
  };

  return handleClick;
}
