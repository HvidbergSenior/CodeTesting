import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { GetProjectResponse, usePostApiProjectsByProjectIdCompensationsMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { CreateCompensationPayment } from "../create-compensation-payment";
import { CompensationPaymentFormData } from "../compensation-payment-form-data";
import { formatDateToRequestShortDate } from "utils/formats";

interface Props {
  project: GetProjectResponse;
}

export function useCreateCompensationPayment(props: Props) {
  const { project } = props;
  const { t } = useTranslation();

  const [openCreateDialog, closeCreateDialog] = useDialog(CreateCompensationPayment);
  const [create] = usePostApiProjectsByProjectIdCompensationsMutation();

  const wrapMutation = useMutation({
    onSuccess: closeCreateDialog,
    successProps: {
      description: t("dashboard.compensationPayments.create.success.description"),
    },
    errorProps: {
      description: t("dashboard.compensationPayments.create.error.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<CompensationPaymentFormData> = (data) => {
    wrapMutation(
      create({
        projectId: project?.id ?? "",
        registerCompensationRequest: {
          compensationPayment: data.amount,
          compensationStartDate: formatDateToRequestShortDate(data.periodDateStart),
          compensationEndDate: formatDateToRequestShortDate(data.periodDateEnd),
          compensationParticipantIds: data.selectedUserIds,
        },
      })
    );
  };

  const handleClick = () => {
    openCreateDialog({ projectId: project.id ?? "", onSubmit: handleOnSubmit });
  };

  return handleClick;
}
