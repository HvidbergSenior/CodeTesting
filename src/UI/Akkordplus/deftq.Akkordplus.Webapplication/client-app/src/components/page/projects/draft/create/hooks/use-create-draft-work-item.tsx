import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import { GetProjectResponse } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { CreateDraftWorkItemDialog } from "../create-draft-work-item-dialog";
import { DraftWorkItem, useOfflineStorage } from "utils/offline-storage/use-offline-storage";
import { useToast } from "shared/toast/hooks/use-toast";
import { UseMapWorkItem } from "components/page/folder/content/measurements/create/hooks/use-map-work-item";
import { FormDataWorkItemDraft } from "components/page/folder/content/measurements/create/create-work-item-dialog/create-work-item-form-data";

interface Prop {
  project: GetProjectResponse;
  updateTable: () => void;
}

export function useCreateDraftWorkItem(props: Prop) {
  const { project, updateTable } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { addOfflineDraftWorkItem } = useOfflineStorage();
  const { mapExtendedSupplementOperationsRequests, mapMountingCode, mapSupplementRequests, mapSupplementTexts } = UseMapWorkItem();
  const [openCreateDraftWorkItemDialog, closeCreateDraftWorkItemDialog] = useDialog(CreateDraftWorkItemDialog);

  const handleOnSubmit: SubmitHandler<FormDataWorkItemDraft> = (data) => {
    const projectId = project?.id ?? "";
    try {
      const draftId = new Date().getTime().toString();
      const supplements = mapSupplementRequests(data.supplements);
      const supplementTexts = mapSupplementTexts(data.supplements);
      const workItemAmount = data.amount ?? 0;
      const workItemType = data.workitemType;
      const material = workItemType === "Material" ? data.material : undefined;
      const operation = workItemType === "Operation" ? data.operation : undefined;
      const workItemMountingCode = workItemType === "Material" ? mapMountingCode(data.mountingCode) : undefined;
      const workItemMountingText = workItemType === "Material" ? data.mountingCode?.text : undefined;
      const supplementOperations = workItemType === "Material" ? mapExtendedSupplementOperationsRequests(data.supplementOperations) : undefined;
      const note = data.note ?? "";
      const item = {
        note,
        draftId,
        supplements,
        supplementTexts,
        workItemAmount,
        material,
        operation,
        workItemType,
        supplementOperations,
        workItemMountingCode,
        workItemMountingText,
      } as DraftWorkItem;
      addOfflineDraftWorkItem(projectId, item);
      updateTable();
      toast.success(t("project.offline.create.success.description"));
    } catch (error) {
      console.error(error);
      toast.error(t("project.offline.create.error.description"));
    } finally {
      closeCreateDraftWorkItemDialog();
    }
  };

  const handleClick = () => {
    openCreateDraftWorkItemDialog({ onSubmit: handleOnSubmit });
  };

  return handleClick;
}
