import { usePostApiProjectsByProjectIdFoldersMutation } from "api/generatedApi";
import type { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { CreateFolderFormData, FolderCreateEdit } from "components/page/folder/create-edit/create-edit";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";

interface Props {
  project: GetProjectResponse;
  folder: ProjectFolderResponse | undefined;
}

export function useCreateFolder(props: Props) {
  const { project, folder } = props;
  const { t } = useTranslation();
  const [openCreateEditFolderDialog, closeCreateEditFolderDialog] = useDialog(FolderCreateEdit);
  const [createFolder] = usePostApiProjectsByProjectIdFoldersMutation();

  const wrapMutation = useMutation({
    onSuccess: closeCreateEditFolderDialog,
    successProps: {
      description: t("folder.createEdit.success.createDescription"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleSubmit: SubmitHandler<CreateFolderFormData> = (data) => {
    wrapMutation(
      createFolder({
        projectId: project.id ? project.id : "",
        createProjectFolderRequest: {
          folderName: data.folderName,
          folderDescription: data.folderDescription,
          parentFolderId: folder?.projectFolderId,
        },
      })
    );
  };

  const handleClick = () => {
    openCreateEditFolderDialog({
      onSubmit: handleSubmit,
    });
  };

  return handleClick;
}
