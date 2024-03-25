import { GetProjectResponse, usePostApiProjectsByProjectIdFoldersAndFolderIdSupplementsMutation } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { useToast } from "shared/toast/hooks/use-toast";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { UpdateFolderSupplements } from "../update-folder-supplements";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";

interface Props {
  folder: ExtendedProjectFolder;
  project: GetProjectResponse;
}

export const useUpdateFolderSupplements = ({ folder, project }: Props) => {
  const { t } = useTranslation();
  const { canUpdateSupplementsOnFolder } = useFolderRestrictions(project);
  const [updateSupplements] = usePostApiProjectsByProjectIdFoldersAndFolderIdSupplementsMutation();
  const toast = useToast();
  const [openDialog, closeDialog] = useDialog(UpdateFolderSupplements);

  const handleSuccess = () => {
    closeDialog();
  };

  const wrapMutation = useMutation({
    onSuccess: handleSuccess,
    successProps: {
      description: t("content.calculation.folderSupplements.dialog.success"),
    },
    resultDialogType: ResultDialogType.Toast,
    errorProps: {
      description: t("content.calculation.folderSupplements.dialog.error"),
    },
  });

  const handleOnSubmit = (data: string[]) => {
    if (!canUpdateSupplementsOnFolder()) {
      toast.error(t("content.calculation.baseRateAndSupplements.dialog.restrictionError"));
      return;
    }
    wrapMutation(
      updateSupplements({
        projectId: project.id ?? "",
        folderId: folder?.projectFolderId ?? "",
        updateFolderSupplementsRequest: {
          folderSupplements: data,
        },
      })
    );
  };

  const handleClick = () => {
    openDialog({ onSubmit: handleOnSubmit, folder: folder, project: project });
  };

  return handleClick;
};
