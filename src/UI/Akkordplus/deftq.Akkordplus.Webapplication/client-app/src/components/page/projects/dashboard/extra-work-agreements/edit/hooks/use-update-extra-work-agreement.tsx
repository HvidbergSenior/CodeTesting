import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import {
  ExtraWorkAgreementResponse,
  GetProjectResponse,
  usePutApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementIdMutation,
} from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { UpdateExtraWorkAgreementDialog } from "../update-dialog";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";
import { ExtraWorkAgreementFormData } from "./use-input-validation";

interface Props {
  project: GetProjectResponse;
  agreement: ExtraWorkAgreementResponse;
}

export function useUpdateExtraWorkAgreement(props: Props) {
  const { project, agreement } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canEditExtraWorkAgreement } = useDashboardRestrictions(project);
  const [openUpdateExtraWorkAgreement, closeUpdateExtraWorkAgreementDialog] = useDialog(UpdateExtraWorkAgreementDialog);
  const [updateExtraWorkAgreeemnt] = usePutApiProjectsByProjectIdExtraworkagreementsAndExtraWorkAgreementIdMutation();

  const wrapMutation = useMutation({
    onSuccess: closeUpdateExtraWorkAgreementDialog,
    successProps: {
      description: t("dashboard.extraWorkAgreements.update.success.description"),
    },
    errorProps: {
      description: t("dashboard.extraWorkAgreements.update.error.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<ExtraWorkAgreementFormData> = (data) => {
    if (!canEditExtraWorkAgreement()) {
      toast.error(t("dashboard.extraWorkAgreements.update.restrictionError"));
      return;
    }
    wrapMutation(
      updateExtraWorkAgreeemnt({
        projectId: project?.id ?? "",
        extraWorkAgreementId: agreement.extraWorkAgreementId ?? "",
        updateProjectExtraWorkAgreementsRequest: {
          description: data.description,
          extraWorkAgreementNumber: data.extraWorkAgreementNumber,
          extraWorkAgreementType: data.extraWorkAgreementType,
          name: data.name,
          paymentDkr: data.paymentDkr,
          workTime: data.workTime,
        },
      })
    );
  };

  const handleClick = () => {
    openUpdateExtraWorkAgreement({ onSubmit: handleOnSubmit, agreement, allowChanges: canEditExtraWorkAgreement() });
  };

  return handleClick;
}
