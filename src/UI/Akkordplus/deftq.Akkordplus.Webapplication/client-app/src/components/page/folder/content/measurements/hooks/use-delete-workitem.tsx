import { useTranslation } from "react-i18next";
import { GetProjectResponse, useDeleteApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMutation } from "api/generatedApi";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useToast } from "shared/toast/hooks/use-toast";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";

interface Props {
  project: GetProjectResponse;
  folder?: ExtendedProjectFolder;
  selectedIds: Set<string>;
}

export function useDeleteWorkitems(props: Props) {
  const { project, folder, selectedIds } = props;
  const { t } = useTranslation();

  const { canDeleteWorkitems } = useWorkItemRestrictions(project);
  const toast = useToast();
  const [deleteWorkitem] = useDeleteApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMutation();

  const handleOnSuccessClose = () => {
    selectedIds.clear();
  };
  const wrapMutation = useMutation({
    onSuccess: handleOnSuccessClose,
    successProps: {
      description: t("content.measurements.delete.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      deleteWorkitem({
        projectId: project.id ? project.id : "",
        folderId: folder?.projectFolderId ? folder.projectFolderId : "",
        removeWorkItemRequest: { workItemIds: Array.from(selectedIds) },
      })
    );
  };

  const openConfirmDialog = useConfirm({
    title: t("content.measurements.delete.title"),
    description: t("content.measurements.delete.description"),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = () => {
    if (!canDeleteWorkitems(folder, selectedIds)) {
      toast.error(t("content.measurements.delete.restrictionError"));
      return;
    }
    openConfirmDialog();
  };

  return handleClick;
}
