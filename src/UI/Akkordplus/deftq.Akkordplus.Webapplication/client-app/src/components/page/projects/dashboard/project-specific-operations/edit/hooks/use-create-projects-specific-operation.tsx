import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { GetProjectResponse, usePostApiProjectsByProjectIdProjectspecificoperationMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";
import { CreateProjectSpecificOperationDialog } from "../create-dialog";
import { ProjectSpecificOperationFormData } from "./project-specific-operation-formdata";

interface Props {
  project: GetProjectResponse;
}

export function useCreateProjectSpecificOperation(props: Props) {
  const { project } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const [openDialog, closeDialog] = useDialog(CreateProjectSpecificOperationDialog);
  const [createProjectSpecificOperation] = usePostApiProjectsByProjectIdProjectspecificoperationMutation();
  const { canCreateProjectSpecificOperation } = useDashboardRestrictions(project);

  const wrapMutation = useMutation({
    onSuccess: closeDialog,
    successProps: {
      description: t("dashboard.projectSpecificOperations.create.success.description"),
    },
    errorProps: {
      description: t("dashboard.projectSpecificOperations.create.error.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<ProjectSpecificOperationFormData> = (data) => {
    wrapMutation(
      createProjectSpecificOperation({
        projectId: project?.id ?? "",
        registerProjectSpecificOperationRequest: {
          extraWorkAgreementNumber: data.newExtraWorkAgreementNumber,
          name: data.newName,
          description: data.newDescription,
          operationTimeMs: data.timeType === "operationTime" ? data.newOperationTimeMs : undefined,
          workingTimeMs: data.timeType === "workingTime" ? data.newWorkingTimeMs : undefined,
        },
      })
    );
  };

  const handleClick = () => {
    if (!canCreateProjectSpecificOperation()) {
      toast.error(t("dashboard.projectSpecificOperations.create.restrictionError"));
      return;
    }
    openDialog({ projectId: project.id ?? "", onSubmit: handleOnSubmit });
  };

  return handleClick;
}
