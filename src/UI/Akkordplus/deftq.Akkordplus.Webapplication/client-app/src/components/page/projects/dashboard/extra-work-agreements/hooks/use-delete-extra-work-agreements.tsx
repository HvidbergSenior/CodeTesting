import { useTranslation } from "react-i18next";
import { GetProjectResponse, useDeleteApiProjectsByProjectIdExtraworkagreementsMutation } from "api/generatedApi";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useToast } from "shared/toast/hooks/use-toast";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";

interface Props {
  project: GetProjectResponse;
  selectedIds: Set<string>;
}

export function useDeleteExtraWorkAgreements(props: Props) {
  const { project, selectedIds } = props;
  const { t } = useTranslation();

  const { canDeleteExtraWorkAgreements } = useDashboardRestrictions(project);
  const toast = useToast();

  const [deleteAgreements] = useDeleteApiProjectsByProjectIdExtraworkagreementsMutation();

  const handleOnSuccessClose = () => {
    selectedIds.clear();
  };

  const wrapMutation = useMutation({
    onSuccess: handleOnSuccessClose,
    successProps: {
      description: t("dashboard.extraWorkAgreements.delete.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      deleteAgreements({
        projectId: project.id ? project.id : "",
        removeExtraWorkAgreementRequest: { extraWorkAgreementIds: Array.from(selectedIds) },
      })
    );
  };

  const openConfirmDialog = useConfirm({
    title: t("dashboard.extraWorkAgreements.delete.title"),
    description: t("dashboard.extraWorkAgreements.delete.description"),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = () => {
    if (!canDeleteExtraWorkAgreements(selectedIds)) {
      toast.error(t("dashboard.extraWorkAgreements.delete.restrictionError"));
      return;
    }
    openConfirmDialog();
  };

  return handleClick;
}
