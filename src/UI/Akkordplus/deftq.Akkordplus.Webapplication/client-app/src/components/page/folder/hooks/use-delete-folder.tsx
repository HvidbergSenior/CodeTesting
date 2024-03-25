import { useTranslation } from "react-i18next";
import { useDeleteApiProjectsByProjectIdFoldersAndFolderIdMutation } from "api/generatedApi";
import { GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";

interface Props {
  project: GetProjectResponse;
  folder: ProjectFolderResponse | undefined;
}

export function useDeleteFolder(props: Props) {
  const { project, folder } = props;
  const { t } = useTranslation();

  const [deleteFolder] = useDeleteApiProjectsByProjectIdFoldersAndFolderIdMutation();

  const wrapMutation = useMutation({
    successProps: {
      description: t("folder.delete.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      deleteFolder({
        projectId: project.id ? project.id : "",
        folderId: folder?.projectFolderId ? folder.projectFolderId : "",
      })
    );
  };

  const openConfirmDialog = useConfirm({
    title: t("folder.delete.title"),
    description: t("folder.delete.description"),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = () => {
    openConfirmDialog();
  };

  return handleClick;
}
