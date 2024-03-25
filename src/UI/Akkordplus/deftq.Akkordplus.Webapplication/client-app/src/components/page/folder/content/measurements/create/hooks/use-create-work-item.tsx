import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";

import {
  FavoritesResponse,
  GetProjectResponse,
  ProjectFolderResponse,
  usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialMutation,
  usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationMutation,
} from "api/generatedApi";
import { CreateWorkItemDialog } from "components/page/folder/content/measurements/create/create-work-item-dialog/create-work-item";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { UseMapWorkItem } from "./use-map-work-item";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { useToast } from "shared/toast/hooks/use-toast";
import { FormDataWorkItem } from "../create-work-item-dialog/create-work-item-form-data";

interface Prop {
  project: GetProjectResponse;
  folder: ProjectFolderResponse | undefined;
  favorites: FavoritesResponse[];
}

export function useCreateWorkItem(props: Prop) {
  const { project, folder, favorites } = props;
  const { t } = useTranslation();
  const { canCreateWorkitem } = useWorkItemRestrictions(project);
  const toast = useToast();
  const [openCreateWorkItemDialog, closeCreateWorkItemDialog] = useDialog(CreateWorkItemDialog);
  const [createMaterialItem] = usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialMutation();
  const [createOperationItem] = usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationMutation();
  const { mapMountingCode, mapSupplementOperationsRequests, mapSupplementRequests } = UseMapWorkItem();

  const wrapMutation = useMutation({
    onSuccess: closeCreateWorkItemDialog,
    successProps: {
      description: t("content.measurements.create.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit: SubmitHandler<FormDataWorkItem> = (data) => {
    const projectId = project?.id ?? "";
    const folderId = folder?.projectFolderId ?? "";
    const workItemAmount = data.amount && data.amount > 0 ? data.amount : undefined;
    const supplements = mapSupplementRequests(data.supplements);

    const create =
      data.workitemType === "Material"
        ? createMaterialItem({
            projectId,
            folderId,
            registerWorkItemMaterialRequest: {
              materialId: data.material.id,
              workItemAmount,
              workItemMountingCode: mapMountingCode(data.mountingCode),
              supplementOperations: mapSupplementOperationsRequests(data.supplementOperations),
              supplements,
            },
          })
        : createOperationItem({
            projectId,
            folderId,
            registerWorkItemOperationRequest: {
              operationId: data.operation.operationId,
              workItemAmount,
              supplements,
            },
          });

    wrapMutation(create);
  };

  const handleClick = () => {
    if (!canCreateWorkitem(folder)) {
      toast.error(t("content.measurements.create.restrictionError"));
      return;
    }
    openCreateWorkItemDialog({ onSubmit: handleOnSubmit, folder, project, favorites });
  };

  return handleClick;
}
