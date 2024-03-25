import { useTranslation } from "react-i18next";
import { GetProjectResponse, useDeleteApiProjectsByProjectIdCompensationsMutation } from "api/generatedApi";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useToast } from "shared/toast/hooks/use-toast";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";

interface Props {
  project: GetProjectResponse;
  selectedIds: Set<string>;
}

export function useDeleteCompensationPayments(props: Props) {
  const { project, selectedIds } = props;
  const { t } = useTranslation();

  const { canDeleteCompensationPayments } = useDashboardRestrictions(project);
  const toast = useToast();

  const [deleteCompensationPayments] = useDeleteApiProjectsByProjectIdCompensationsMutation();

  const handleOnSuccessClose = () => {
    selectedIds.clear();
  };

  const wrapMutation = useMutation({
    onSuccess: handleOnSuccessClose,
    successProps: {
      description: t("dashboard.compensationPayments.delete.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      deleteCompensationPayments({
        projectId: project.id ? project.id : "",
        removeCompensationPaymentsRequest: { compensationPaymentIds: Array.from(selectedIds) },
      })
    );
  };

  const openConfirmDialog = useConfirm({
    title: t("dashboard.compensationPayments.delete.title"),
    description: t("dashboard.compensationPayments.delete.description"),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = () => {
    if (!canDeleteCompensationPayments(selectedIds)) {
      toast.error(t("dashboard.compensationPayments.delete.restrictionError"));
      return;
    }
    openConfirmDialog();
  };

  return handleClick;
}
