import { usePutApiProjectsByProjectIdFoldersAndFolderIdDescriptionMutation } from "api/generatedApi";
import type { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { FolderEditDescription, CreateProjectFormData } from "../../edit-description/edit-folder-description";

interface Props {
  project: GetProjectResponse;
  folder: ProjectFolderResponse;
}

export function useEditFolderDescription(props: Props) {
  const { project, folder } = props;
  const { t } = useTranslation();
  const [openEditFolderDescriptionDialog, closeEditFolderDescriptionDialog] = useDialog(FolderEditDescription);
  const [editFolderName] = usePutApiProjectsByProjectIdFoldersAndFolderIdDescriptionMutation();

  const wrapMutation = useMutation({
    onSuccess: closeEditFolderDescriptionDialog,
    successProps: {
      description: t("folder.editDescription.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleSubmit: SubmitHandler<CreateProjectFormData> = (data) => {
    wrapMutation(
      editFolderName({
        projectId: project.id ? project.id : "",
        folderId: folder.projectFolderId ? folder.projectFolderId : "",
        updateProjectFolderDescriptionRequest: {
          projectFolderDescription: data.folderDescription,
        },
      })
    );
  };

  const handleClick = () => {
    openEditFolderDescriptionDialog({
      folder: folder,
      onSubmit: handleSubmit,
    });
  };

  return handleClick;
}
