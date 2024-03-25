import { usePostApiProjectsMutation } from "api/generatedApi";
import { CreateProjectDialog } from "components/page/projects/project-setup/create-project-dialog/create-project-dialog";
import { ProjectSetupFormData } from "../../project-setup-form-data";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { useProjectRestrictions } from "shared/user-restrictions/use-project-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";
import { useNavigate } from "react-router-dom";

export function useCreateProject() {
  const { t } = useTranslation();
  const toast = useToast();
  const navigate = useNavigate();
  const { canCreateProject } = useProjectRestrictions();

  const [openCreateProjectDialog, closeCreateProjectDialog] = useDialog(CreateProjectDialog);
  const [createProject] = usePostApiProjectsMutation();

  const onSuccess = (response: any) => {
    closeCreateProjectDialog();
    navigate(`/projects/${response}`);
  };

  const wrapMutation = useMutation({
    onSuccessReponse: onSuccess,
    successProps: {
      title: t("project.create.success.title"),
      description: t("project.create.success.description"),
    },
    errorProps: {
      title: t("project.create.error.title"),
      description: t("project.create.error.description"),
    },
    resultDialogType: ResultDialogType.Dialog,
  });

  const handleOnSubmit: SubmitHandler<ProjectSetupFormData> = (data) => {
    if (!canCreateProject()) {
      toast.error(t("project.create.restrictedError"));
      return;
    }
    wrapMutation(
      createProject({
        createProjectRequest: {
          title: data.title,
          description: data.description,
          pieceworkType: data.pieceworkType,
          pieceworkSum: data.lumpSum,
        },
      })
    );
  };

  const handleClick = () => {
    openCreateProjectDialog({ onSubmit: handleOnSubmit });
  };

  return handleClick;
}
