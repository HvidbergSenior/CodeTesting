import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { GetProjectResponse, usePostApiProjectsByProjectIdSetupProjecttypeMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { useToast } from "shared/toast/hooks/use-toast";
import { EditProjectTypeAndPeriodDialog } from "../edit-project-type-and-period-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { ProjectSetupFormData } from "../../project-setup-form-data";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { formatDateToRequestShortDate } from "utils/formats";

interface Props {
  project: GetProjectResponse;
}
export function useEditProjectTypeAndPeriod(props: Props) {
  const { project } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canEditProjectSetupProjectType } = useDashboardRestrictions(project);

  const [openEditDialog, closeEditDialog] = useDialog(EditProjectTypeAndPeriodDialog);
  const [updateProject] = usePostApiProjectsByProjectIdSetupProjecttypeMutation();

  const wrapMutation = useMutation({
    onSuccess: closeEditDialog,
    successProps: {
      title: t("project.editProjectType.success.title"),
    },
    errorProps: {
      title: t("project.editProjectType.error.title"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<ProjectSetupFormData> = (data) => {
    if (!canEditProjectSetupProjectType()) {
      toast.error(t("project.editProjectType.restrictedError"));
      return;
    }
    wrapMutation(
      updateProject({
        projectId: project.id ?? "",
        updateProjectTypeRequest: {
          pieceworkType: data.pieceworkType,
          pieceWorkSum: data.pieceworkType === "TwelveTwo" ? data.lumpSum : undefined,
          startDate: formatDateToRequestShortDate(data.projectDateStart),
          endDate: formatDateToRequestShortDate(data.projectDateEnd),
        },
      })
    );
  };

  const handleClick = () => {
    openEditDialog({ onSubmit: handleOnSubmit, project });
  };

  return handleClick;
}
