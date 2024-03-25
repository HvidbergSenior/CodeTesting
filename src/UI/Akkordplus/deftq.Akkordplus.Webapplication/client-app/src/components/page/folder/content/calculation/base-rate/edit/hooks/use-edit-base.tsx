import { BaseSupplementUpdate, GetProjectResponse, usePutApiProjectsByProjectIdFoldersAndFolderIdBasesupplementsMutation } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { SubmitState } from "shared/enums";
import { useToast } from "shared/toast/hooks/use-toast";
import { useRateRestrictions } from "shared/user-restrictions/use-rate-restrictions";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { EditBaseRateAndSupplements } from "../edit-base";

interface Props {
  folder: ExtendedProjectFolder | undefined;
  project: GetProjectResponse;
}

export const useEditBaseRateAndSupplements = ({ folder, project }: Props) => {
  const { t } = useTranslation();
  const { canEditBaseRateAndSupplements } = useRateRestrictions(project);
  const [saveBaseSupplements] = usePutApiProjectsByProjectIdFoldersAndFolderIdBasesupplementsMutation();
  const toast = useToast();
  const [openEditBaseRateAndSupplementsDialog, closeEditBaseRateAndSupplementsDialog] = useDialog(EditBaseRateAndSupplements);

  const handleSuccess = () => {
    closeEditBaseRateAndSupplementsDialog();
  };

  const wrapMutation = useMutation({
    onSuccess: handleSuccess,
    successProps: {
      description: t("content.calculation.baseRateAndSupplements.edit.successBaseSupplements"),
    },
    errorProps: {
      description: t("content.calculation.baseRateAndSupplements.edit.failureBaseSupplements"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = (state: SubmitState, indirectTime: BaseSupplementUpdate, siteSpecificTime: BaseSupplementUpdate) => {
    if (state === SubmitState.Close) {
      closeEditBaseRateAndSupplementsDialog();
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
          updateBaseSupplementsRequest: { indirectTimePercentage: indirectTime, siteSpecificTimePercentage: siteSpecificTime },
        })
      );
    }
  };

  const handleClick = () => {
    openEditBaseRateAndSupplementsDialog({ onSubmit: handleOnSubmit, folder: folder, project: project });
  };

  return handleClick;
};
