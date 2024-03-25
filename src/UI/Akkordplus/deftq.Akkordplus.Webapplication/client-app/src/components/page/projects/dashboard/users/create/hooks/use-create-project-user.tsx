import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { GetProjectResponse, usePostApiProjectsByProjectIdUsersMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { CreateProjectUserDialog } from "../create-project-user";
import { CreateProjectUserFormData } from "../inputs/project-user-form-data";

interface Props {
  project: GetProjectResponse;
}

export function useCreateProjectUser(props: Props) {
  const { project } = props;
  const { t } = useTranslation();

  const [openCreateDialog, closeCreateDialog] = useDialog(CreateProjectUserDialog);
  const [createUser] = usePostApiProjectsByProjectIdUsersMutation();

  const wrapMutation = useMutation({
    onSuccess: closeCreateDialog,
    successProps: {
      description: t("dashboard.users.create.success.description"),
    },
    errorProps: {
      description: t("dashboard.users.create.error.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<CreateProjectUserFormData> = (data) => {
    wrapMutation(
      createUser({
        projectId: project?.id ?? "",
        registerProjectUserRequest: {
          name: data.name,
          role: data.role,
          email: data.email,
          address: data.adr,
          phone: data.phone,
        },
      })
    );
  };

  const handleClick = () => {
    openCreateDialog({ onSubmit: handleOnSubmit });
  };

  return handleClick;
}
