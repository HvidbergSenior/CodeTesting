import { useTranslation } from "react-i18next";
import { GetProjectResponse, useDeleteApiProjectsByProjectIdProjectspecificoperationMutation } from "api/generatedApi";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useToast } from "shared/toast/hooks/use-toast";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";

interface Props {
  project: GetProjectResponse;
  selectedIds: Set<string>;
}

export function useDeleteProjectSpecificOperations(props: Props) {
  const { project, selectedIds } = props;
  const { t } = useTranslation();

  const { canDeleteProjectSpecificOperations } = useDashboardRestrictions(project);
  const toast = useToast();

  const [deleteProjectSpecificOperations] = useDeleteApiProjectsByProjectIdProjectspecificoperationMutation();

  const handleOnSuccessClose = () => {
    selectedIds.clear();
  };

  const wrapMutation = useMutation({
    onSuccess: handleOnSuccessClose,
    successProps: {
      description: t("dashboard.projectSpecificOperations.delete.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      deleteProjectSpecificOperations({
        projectId: project.id ? project.id : "",
        removeProjectSpecificOperationsRequest: { projectSpecificOperationIds: Array.from(selectedIds) },
      })
    );
  };

  const openConfirmDialog = useConfirm({
    title: t("dashboard.projectSpecificOperations.delete.title"),
    description: t("dashboard.projectSpecificOperations.delete.description"),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = () => {
    if (!canDeleteProjectSpecificOperations(selectedIds)) {
      toast.error(t("dashboard.projectSpecificOperations.delete.restrictionError"));
      return;
    }
    openConfirmDialog();
  };

  return handleClick;
}
