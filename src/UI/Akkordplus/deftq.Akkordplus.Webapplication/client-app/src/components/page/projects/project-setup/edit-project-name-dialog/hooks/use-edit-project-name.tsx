import { GetProjectResponse, usePostApiProjectsByProjectIdProjectinformationMutation } from "api/generatedApi";
import { ProjectSetupFormData } from "../../project-setup-form-data";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { useToast } from "shared/toast/hooks/use-toast";
import { EditProjectNameDialog } from "../edit-project-name-dialog";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";

interface Props {
  project: GetProjectResponse;
}
export function useEditProjectName(props: Props) {
  const { project } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canEditProjectSetupProjectName } = useDashboardRestrictions(project);

  const [openEditDialog, closeEditDialog] = useDialog(EditProjectNameDialog);
  const [updateProject] = usePostApiProjectsByProjectIdProjectinformationMutation();

  const wrapMutation = useMutation({
    onSuccess: closeEditDialog,
    successProps: {
      title: t("project.editProjectName.success.title"),
    },
    errorProps: {
      title: t("project.editProjectName.error.title"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<ProjectSetupFormData> = (data) => {
    if (!canEditProjectSetupProjectName()) {
      toast.error(t("project.editProjectName.restrictedError"));
      return;
    }
    wrapMutation(
      updateProject({
        projectId: project.id ?? "",
        updateProjectInformationRequest: {
          name: data.title,
          description: data.description,
          orderNumber: data.orderNumber,
          pieceworkNumber: data.pieceworkNumber,
        },
      })
    );
  };

  const handleClick = () => {
    openEditDialog({ onSubmit: handleOnSubmit, project });
  };

  return handleClick;
}
