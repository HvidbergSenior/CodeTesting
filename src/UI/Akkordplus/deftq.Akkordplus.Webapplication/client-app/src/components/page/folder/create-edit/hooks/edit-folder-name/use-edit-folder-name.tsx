import { usePutApiProjectsByProjectIdFoldersAndFolderIdNameMutation } from "api/generatedApi";
import type { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { FolderEditName, EditFolderNameFormData } from "../../edit-name/edit-folder-name";

interface Props {
  project: GetProjectResponse;
  folder: ProjectFolderResponse | undefined;
}

export function useEditFolderName(props: Props) {
  const { project, folder } = props;
  const { t } = useTranslation();
  const [openEditFolderNameDialog, closeEditFolderNameDialog] = useDialog(FolderEditName);
  const [editFolderName] = usePutApiProjectsByProjectIdFoldersAndFolderIdNameMutation();

  const wrapMutation = useMutation({
    onSuccess: closeEditFolderNameDialog,
    successProps: {
      description: t("folder.editName.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleSubmit: SubmitHandler<EditFolderNameFormData> = (data) => {
    wrapMutation(
      editFolderName({
        projectId: project.id ? project.id : "",
        folderId: folder?.projectFolderId ? folder?.projectFolderId : "",
        updateProjectFolderNameRequest: {
          projectFolderName: data.folderName,
        },
      })
    );
  };

  const handleClick = () => {
    openEditFolderNameDialog({
      folder: folder,
      onSubmit: handleSubmit,
    });
  };

  return handleClick;
}
