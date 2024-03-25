import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import {
  GetProjectResponse,
  GetProjectSpecificOperationResponse,
  usePostApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationIdMutation,
} from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";
import { ProjectSpecificOperationFormData } from "./project-specific-operation-formdata";
import { EditProjectSpecificOperationDialog } from "../edit-dialog";
import { InfoProjectSpecificOperationDialog } from "../info-dialog";

interface Props {
  project: GetProjectResponse;
  operation: GetProjectSpecificOperationResponse;
}

export function useUpdateProjectSpecificOperation(props: Props) {
  const { project, operation } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const [openEditDialog, closeEditDialog] = useDialog(EditProjectSpecificOperationDialog);
  const [openInfoDialog] = useDialog(InfoProjectSpecificOperationDialog);
  const [update] = usePostApiProjectsByProjectIdProjectspecificoperationAndProjectSpecificOperationIdMutation();
  const { canEditProjectSpecificOperation } = useDashboardRestrictions(project);

  const wrapMutation = useMutation({
    onSuccess: closeEditDialog,
    successProps: {
      description: t("dashboard.projectSpecificOperations.update.success.description"),
    },
    errorProps: {
      description: t("dashboard.projectSpecificOperations.update.error.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<ProjectSpecificOperationFormData> = (data) => {
    if (!canEditProjectSpecificOperation()) {
      toast.error(t("dashboard.projectSpecificOperations.update.restrictionError"));
      return;
    }
    wrapMutation(
      update({
        projectId: project?.id ?? "",
        projectSpecificOperationId: operation.projectSpecificOperationId ?? "",
        updateProjectSpecificOperationRequest: {
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
    if (canEditProjectSpecificOperation()) {
      openEditDialog({ operation, onSubmit: handleOnSubmit });
    } else {
      openInfoDialog({ operation });
    }
  };

  return handleClick;
}
