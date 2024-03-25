import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { GetProjectResponse, usePostApiProjectsByProjectIdFavoritesMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { CreateFavoriteDialog, CreateFavoriteFormData } from "../create";

interface Props {
  project: GetProjectResponse;
}

export function useCreateFavorite(props: Props) {
  const { project } = props;
  const { t } = useTranslation();

  const [openCreateFavorite, closeCreateFavoriteDialog] = useDialog(CreateFavoriteDialog);
  const [createFavorite] = usePostApiProjectsByProjectIdFavoritesMutation();

  const wrapMutation = useMutation({
    onSuccess: closeCreateFavoriteDialog,
    successProps: {
      description: t("dashboard.favorits.create.success.description"),
    },
    errorProps: {
      description: t("dashboard.favorits.create.error.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<CreateFavoriteFormData> = (data) => {
    const favId = data.workitemType === "Material" ? data.material.id : data.operation.operationId;
    wrapMutation(
      createFavorite({
        projectId: project?.id ?? "",
        registerProjectCatalogFavoriteRequest: {
          catalogId: favId ?? "",
          catalogType: data.workitemType,
        },
      })
    );
  };

  const handleClick = () => {
    openCreateFavorite({ onSubmit: handleOnSubmit });
  };

  return handleClick;
}
