import {
  GetExtraWorkAgreementRatesQueryResponse,
  GetProjectResponse,
  usePutApiProjectsByProjectIdExtraworkagreementsRatesMutation,
} from "api/generatedApi";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { useToast } from "shared/toast/hooks/use-toast";
import { useRateRestrictions } from "shared/user-restrictions/use-rate-restrictions";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { EditHours } from "../edit-hours";

interface Props {
  data: GetExtraWorkAgreementRatesQueryResponse | undefined;
  project: GetProjectResponse;
}

export const useEditHours = ({ project, data }: Props) => {
  const { t } = useTranslation();
  const { canEditBaseRateAndSupplements } = useRateRestrictions(project);
  const [saveExtraWorkAgreementRates] = usePutApiProjectsByProjectIdExtraworkagreementsRatesMutation();
  const toast = useToast();
  const [openEditHoursDialog, closeEditHoursDialog] = useDialog(EditHours);

  const handleSuccess = () => {
    closeEditHoursDialog();
  };

  const wrapMutation = useMutation({
    onSuccess: handleSuccess,
    successProps: {
      description: t("dashboard.projectInfo.editHours.success"),
    },
    resultDialogType: ResultDialogType.Toast,
    errorProps: {
      description: t("dashboard.projectInfo.editHours.failure"),
    },
  });

  const handleOnSubmit = (customerHours: string, companyHours: string) => {
    if (!canEditBaseRateAndSupplements()) {
      toast.error(t("dashboard.projectInfo.restrictionError"));
      return;
    }

    wrapMutation(
      saveExtraWorkAgreementRates({
        projectId: project.id ?? "",
        updateExtraWorkAgreementRatesRequest: { customerRatePrHour: parseFloat(customerHours), companyRatePrHour: parseFloat(companyHours) },
      })
    );
  };

  const handleClick = () => {
    openEditHoursDialog({ onSubmit: handleOnSubmit, project: project, data: data });
  };

  return handleClick;
};
