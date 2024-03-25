import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { GetProjectResponse, usePostApiProjectsByProjectIdSetupProjectcompanyMutation } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { useToast } from "shared/toast/hooks/use-toast";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { ProjectSetupFormData } from "../../project-setup-form-data";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { EditProjectCompanyAndWorkplaceDialog } from "../edit-project-company-and-workplace-dialog";

interface Props {
  project: GetProjectResponse;
}
export function useEditProjectCompanyAndWorkplace(props: Props) {
  const { project } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canEditProjectSetupProjectCompany } = useDashboardRestrictions(project);

  const [openEditDialog, closeEditDialog] = useDialog(EditProjectCompanyAndWorkplaceDialog);
  const [updateProject] = usePostApiProjectsByProjectIdSetupProjectcompanyMutation();

  const wrapMutation = useMutation({
    onSuccess: closeEditDialog,
    successProps: {
      title: t("project.editProjectCompany.success.title"),
    },
    errorProps: {
      title: t("project.editProjectCompany.error.title"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<ProjectSetupFormData> = (data) => {
    if (!canEditProjectSetupProjectCompany()) {
      toast.error(t("project.editProjectCompany.restrictedError"));
      return;
    }
    wrapMutation(
      updateProject({
        projectId: project.id ?? "",
        updateProjectCompanyRequest: {
          company: data.companyName,
          cvrNumber: data.cvrNumber,
          pNumber: data.pNumber,
          workplaceAdr: data.workplaceAdr,
        },
      })
    );
  };

  const handleClick = () => {
    openEditDialog({ onSubmit: handleOnSubmit, project });
  };

  return handleClick;
}
