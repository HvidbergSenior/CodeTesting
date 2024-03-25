import { BaseRateUpdate, GetProjectResponse, usePutApiProjectsByProjectIdFoldersAndFolderIdBaserateMutation } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { SubmitState } from "shared/enums";
import { useToast } from "shared/toast/hooks/use-toast";
import { useRateRestrictions } from "shared/user-restrictions/use-rate-restrictions";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { EditPaymentFactor } from "../edit-factor";

interface Props {
  folder: ExtendedProjectFolder;
  project: GetProjectResponse;
}

export const useEditPaymentFactor = ({ folder, project }: Props) => {
  const { t } = useTranslation();
  const { canEditBaseRateAndSupplements } = useRateRestrictions(project);
  const [saveBaseSupplements] = usePutApiProjectsByProjectIdFoldersAndFolderIdBaserateMutation();
  const toast = useToast();
  const [openEditPaymentFactorDialog, closeEditPaymentFactorDialog] = useDialog(EditPaymentFactor);

  const handleSuccess = () => {
    closeEditPaymentFactorDialog();
  };

  const wrapMutation = useMutation({
    onSuccess: handleSuccess,
    successProps: {
      description: t("content.calculation.baseRateAndSupplements.edit.successBaseRate"),
    },
    resultDialogType: ResultDialogType.Toast,
    errorProps: {
      description: t("content.calculation.baseRateAndSupplements.edit.failureBaseRate"),
    },
  });

  const handleOnSubmit = (state: SubmitState, baseRateUpdate: BaseRateUpdate) => {
    if (state === SubmitState.Close) {
      closeEditPaymentFactorDialog();
    }

    if (!canEditBaseRateAndSupplements()) {
      toast.error(t("content.calculation.baseRateAndSupplements.edit.restrictionError"));
      return;
    }

    if (state === SubmitState.Save) {
      wrapMutation(
        saveBaseSupplements({
          projectId: project.id ?? "",
          folderId: folder?.projectFolderId ?? "",
          updateBaseRateRequest: { baseRateRegulationPercentage: baseRateUpdate },
        })
      );
    }
  };

  const handleClick = () => {
    openEditPaymentFactorDialog({ onSubmit: handleOnSubmit, folder: folder, project: project });
  };

  return handleClick;
};
