import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { GetProjectResponse, usePostApiProjectsByProjectIdExtraworkagreementsMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { CreateExtraWorkAgreementDialog } from "../create-dialog";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";
import { ExtraWorkAgreementFormData } from "./use-input-validation";

interface Props {
  project: GetProjectResponse;
}

export function useCreateExtraWorkAgreement(props: Props) {
  const { project } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const [openCreateExtraWorkAgreement, closeCreateExtraWorkAgreementDialog] = useDialog(CreateExtraWorkAgreementDialog);
  const [createExtraWorkAgreement] = usePostApiProjectsByProjectIdExtraworkagreementsMutation();
  const { canCreateExtraWorkAgreements } = useDashboardRestrictions(project);

  const wrapMutation = useMutation({
    onSuccess: closeCreateExtraWorkAgreementDialog,
    successProps: {
      description: t("dashboard.extraWorkAgreements.create.success.description"),
    },
    errorProps: {
      description: t("dashboard.extraWorkAgreements.create.error.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<ExtraWorkAgreementFormData> = (data) => {
    wrapMutation(
      createExtraWorkAgreement({
        projectId: project?.id ?? "",
        createExtraWorkAgreementRequest: {
          description: data.description,
          extraWorkAgreementNumber: data.extraWorkAgreementNumber,
          extraWorkAgreementType: data.extraWorkAgreementType,
          name: data.name,
          paymentDkr: data.extraWorkAgreementType === "AgreedPayment" ? data.paymentDkr : undefined,
          workTime: data.extraWorkAgreementType === "CompanyHours" || data.extraWorkAgreementType === "CustomerHours" ? data.workTime : undefined,
        },
      })
    );
  };

  const handleClick = () => {
    if (!canCreateExtraWorkAgreements()) {
      toast.error(t("dashboard.extraWorkAgreements.create.restrictionError"));
      return;
    }
    openCreateExtraWorkAgreement({ onSubmit: handleOnSubmit });
  };

  return handleClick;
}
