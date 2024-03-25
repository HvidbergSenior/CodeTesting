import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";

import {
  GetProjectResponse,
  ProjectFolderResponse,
  WorkItemResponse,
  usePutApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemIdMutation,
} from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";
import { FormDataWorkItem } from "../../create/create-work-item-dialog/create-work-item-form-data";
import { EditWorkItemDialog } from "../edit-work-item";

interface Prop {
  project: GetProjectResponse;
  folder: ProjectFolderResponse;
  workItem: WorkItemResponse;
}

export function useEditWorkItem(props: Prop) {
  const { project, folder, workItem } = props;
  const { t } = useTranslation();
  const { canEditWorkitem } = useWorkItemRestrictions(project);
  const toast = useToast();
  const [openEditWorkItemDialog, closeEditWorkItemDialog] = useDialog(EditWorkItemDialog);
  const [updateWorkItem] = usePutApiProjectsByProjectIdFoldersAndFolderIdWorkItemsWorkItemIdMutation();

  const wrapMutation = useMutation({
    onSuccess: closeEditWorkItemDialog,
    successProps: {
      description: t("content.measurements.edit.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<FormDataWorkItem> = (data) => {
    const projectId = project?.id ?? "";
    const folderId = folder?.projectFolderId ?? "";
    const workItemId = workItem.workItemId ?? "";
    const workItemAmount = data.amount && data.amount > 0 ? data.amount : undefined;

    const create = updateWorkItem({
      projectId,
      folderId,
      workItemId,
      updateWorkItemRequest: {
        workItemAmount,
      },
    });

    wrapMutation(create);
  };

  const handleClick = () => {
    if (!canEditWorkitem(folder)) {
      toast.error(t("content.measurements.edit.restrictionError"));
      return;
    }
    openEditWorkItemDialog({ onSubmit: handleOnSubmit, workItem });
  };

  return handleClick;
}
