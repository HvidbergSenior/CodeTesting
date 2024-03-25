import { GetProjectResponse, RegisterWorkItemMaterialRequest, RegisterWorkItemOperationRequest, WorkItemType } from "api/generatedApi";
import { DraftWorkItem } from "utils/offline-storage/use-offline-storage";

export interface DraftWorkItemRequest {
  draftId: string;
  text: string;
  workItemType: WorkItemType;
  request: RegisterWorkItemMaterialRequest | RegisterWorkItemOperationRequest;
  requestSuccess: boolean;
  requestFailed: boolean;
}
export function UseMapDrafts() {
  const mapToRequests = (project: GetProjectResponse, drafts: DraftWorkItem[]): DraftWorkItemRequest[] => {
    const list: DraftWorkItemRequest[] = [];
    if (!project.id) {
      return [];
    }

    drafts.forEach((draft) => {
      const workItemAmount = draft.workItemAmount && draft.workItemAmount > 0 ? draft.workItemAmount : undefined;
      const supplements = draft.supplements;
      const request = {
        draftId: draft.draftId,
        workItemType: draft.workItemType,
        requestFailed: false,
        requestSuccess: false,
      } as DraftWorkItemRequest;
      if (draft.workItemType === "Material") {
        request.text = draft.material?.name ?? "";
        request.request = {
          materialId: draft.material?.id,
          supplementOperations: draft.supplementOperations,
          supplements,
          workItemAmount,
          workItemMountingCode: draft.workItemMountingCode,
        } as RegisterWorkItemMaterialRequest;
      } else {
        request.text = draft.operation?.operationText ?? "";
        request.request = {
          operationId: draft.operation?.operationId,
          supplements,
          workItemAmount,
        } as RegisterWorkItemOperationRequest;
      }
      list.push(request);
    });
    return list;
  };

  return { mapToRequests };
}
