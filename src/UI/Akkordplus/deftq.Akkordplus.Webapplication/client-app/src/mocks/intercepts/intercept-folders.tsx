import { rest } from "msw";
import { GetApiProjectsByProjectIdFoldersApiResponse } from "api/generatedApi";

const InterceptFolders = rest.get("https://localhost:5001/api/projects/123/folders", (req, res, ctx) => {
  const folder: GetApiProjectsByProjectIdFoldersApiResponse = {
    projectId: "123",
    rootFolder: {
      createdBy: "Kasper",
      createdTime: new Date().toISOString(),
      documents: [],
      projectFolderDescription: "hej",
      projectFolderId: "124",
      projectFolderName: "OverMappe 1",
      subFolders: [
        { projectFolderName: "Mappe 1", projectFolderId: "1.1" },
        { projectFolderName: "Mappe 2", projectFolderId: "1.2" },
      ],
      projectFolderLocked: "Unlocked",
    },
  };
  return res(ctx.json(folder));
});
export default InterceptFolders;
