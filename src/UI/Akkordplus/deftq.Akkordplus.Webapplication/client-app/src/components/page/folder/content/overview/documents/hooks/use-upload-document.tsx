import { GetProjectResponse } from "api/generatedApi";
import { makeFullPath } from "api/path";
import { getAccessToken } from "api/baseApi";
import { api } from "api/enhancedEndpoints";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useDispatch } from "react-redux";

export function useUploadDocument(folder: ExtendedProjectFolder | undefined, project: GetProjectResponse, maxFileSizeMb?: number) {
  const dispatch = useDispatch();

  const maxFileSizeBytes = (maxFileSizeMb ?? 10) * 1024 * 1024;

  const clickUploadDocument = async (event: any): Promise<string | undefined> => {
    event.stopPropagation();

    if (!project?.id || !folder?.projectFolderId) return undefined;

    const url = makeFullPath(`/api/projects/${project.id}/folders/${folder.projectFolderId}/documents`);
    const target = event.target as HTMLInputElement;
    const fileObj = target?.files && target.files[0];

    if (!fileObj) return undefined;

    const uploadDocumentFormData = new FormData();
    uploadDocumentFormData.append("file", fileObj);

    if (fileObj.size > maxFileSizeBytes) {
      throw new RangeError();
    }

    // reset file input
    event.target.value = null;

    const token = await getAccessToken();
    const uploadedFilename = await fetch(url, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
      },
      body: uploadDocumentFormData,
    })
      .then((response) => {
        if (response.ok) return response.json;
        throw new Error();
      })
      .then(() => {
        dispatch(api.util.invalidateTags(["Folders"]));
        return fileObj.name;
      })
      .catch((error) => {
        console.error(error);
        return undefined;
      });
    return uploadedFilename;
  };

  return clickUploadDocument;
}
