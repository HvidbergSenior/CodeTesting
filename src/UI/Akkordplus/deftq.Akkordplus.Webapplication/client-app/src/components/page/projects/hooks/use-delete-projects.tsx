import { useTranslation } from "react-i18next";
import { useState } from "react";
import { ProjectResponse, useDeleteApiProjectsByProjectIdMutation } from "api/generatedApi";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useToast } from "shared/toast/hooks/use-toast";
import { useProjectRestrictions } from "shared/user-restrictions/use-project-restrictions";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";

export function useDeleteProject() {
  const { t } = useTranslation();
  const toast = useToast();
  const { canDeleteProject } = useProjectRestrictions();
  const [deleteProject] = useDeleteApiProjectsByProjectIdMutation();
  const [projectId, setProjectId] = useState("");

  const wrapMutation = useMutation({
    onSuccess: () => {},
    successProps: {
      title: t("project.delete.success.title"),
      description: t("project.delete.success.description"),
    },
    resultDialogType: ResultDialogType.Dialog,
  });

  const handleOnSubmit = () => {
    if (!canDeleteProject()) {
      toast.error(t("project.create.restrictedError"));
      return;
    }
    wrapMutation(
      deleteProject({
        projectId,
      })
    );
  };

  const openConfirmDialog = useConfirm({
    title: t("project.delete.title"),
    description: t("project.delete.description"),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = (projectId: ProjectResponse["projectId"]) => {
    if (projectId) {
      setProjectId(projectId);
      openConfirmDialog();
    }
  };

  return handleClick;
}
