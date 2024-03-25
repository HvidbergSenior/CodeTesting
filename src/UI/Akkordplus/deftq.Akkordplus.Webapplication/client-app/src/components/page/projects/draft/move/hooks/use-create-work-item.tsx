import { api } from "api/enhancedEndpoints";
import { GetProjectResponse } from "api/generatedApi";
import { DraftWorkItemRequest } from "./use-map-drafts";

export function UseDraftsCreateWorkItems() {
  const [createMaterial] = api.usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsMaterialMutation();
  const [createOperation] = api.usePostApiProjectsByProjectIdFoldersAndFolderIdWorkitemsOperationMutation();

  const createWorkItems = async (project: GetProjectResponse, folderId: string, requests: DraftWorkItemRequest[]) => {
    if (!project?.id) {
      return;
    }
    for (const request of requests) {
      const create =
        request.workItemType === "Material"
          ? createMaterial({ projectId: project.id, folderId: folderId, registerWorkItemMaterialRequest: request.request })
          : createOperation({ projectId: project.id, folderId: folderId, registerWorkItemOperationRequest: request.request });
      try {
        await create
          .unwrap()
          .then(() => {
            request.requestSuccess = true;
          })
          .catch((error) => {
            console.error(error);
            request.requestFailed = true;
          });
      } catch (error) {
        console.error(error);
        request.requestFailed = true;
      }
    }
  };

  return { createWorkItems };
}
