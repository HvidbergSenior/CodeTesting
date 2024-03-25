import { rest } from "msw";
import { GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiResponse } from "api/generatedApi";

const IntercepWorkItemsProject = rest.get("https://localhost:5001/api/projects/123/folders/124/workitems", (req, res, ctx) => {
  const folder: GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiResponse = {
    projectFolderId: "124",
    projectId: "123",
    workItems: [
      {
        workItemId: "1",
        workItemDate: undefined,
        workItemAmount: 1,
        workItemText: "Materiale",
        workItemMaterial: { workItemEanNumber: "12345", workItemMountingCode: 4, workItemMountingCodeText: "Mounting text" },
        workItemTotalPaymentDkr: 100,
        workItemType: "Material",
        workItemTotalOperationTimeMilliseconds: 3000,
      },
      {
        workItemId: "2",
        workItemDate: undefined,
        workItemAmount: 1,
        workItemText: "Operation",
        workItemOperation: { operationNumber: "12345" },
        workItemTotalPaymentDkr: 100,
        workItemType: "Operation",
        workItemTotalOperationTimeMilliseconds: 3000,
      },
    ],
  };
  return res(ctx.json(folder));
});

const IntercepWorkItemsSubFolder = rest.get("https://localhost:5001/api/projects/123/folders/1.1/workitems", (req, res, ctx) => {
  const folder: GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiResponse = {
    projectFolderId: "124",
    projectId: "123",
    workItems: [
      {
        workItemId: "1",
        workItemDate: undefined,
        workItemAmount: 1,
        workItemText: "Materiale 1",
        workItemMaterial: { workItemEanNumber: "12345", workItemMountingCode: 4, workItemMountingCodeText: "Mounting text" },
        workItemTotalPaymentDkr: 100,
        workItemType: "Material",
        workItemTotalOperationTimeMilliseconds: 3000,
      },
      {
        workItemId: "2",
        workItemDate: undefined,
        workItemAmount: 1,
        workItemText: "Operation 1",
        workItemOperation: { operationNumber: "12345" },
        workItemTotalPaymentDkr: 100,
        workItemType: "Operation",
        workItemTotalOperationTimeMilliseconds: 3000,
      },
    ],
  };
  return res(ctx.json(folder));
});

const IntercepWorkItemsSubFolderTwo = rest.get("https://localhost:5001/api/projects/123/folders/1.2/workitems", (req, res, ctx) => {
  const folder: GetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsApiResponse = {
    projectFolderId: "124",
    projectId: "123",
    workItems: [
      {
        workItemId: "1",
        workItemDate: undefined,
        workItemAmount: 1,
        workItemText: "Materiale 2",
        workItemMaterial: { workItemEanNumber: "12345", workItemMountingCode: 4, workItemMountingCodeText: "Mounting text" },
        workItemTotalPaymentDkr: 100,
        workItemType: "Material",
        workItemTotalOperationTimeMilliseconds: 3000,
      },
      {
        workItemId: "2",
        workItemDate: undefined,
        workItemAmount: 1,
        workItemText: "Operation 2",
        workItemOperation: { operationNumber: "12345" },
        workItemTotalPaymentDkr: 100,
        workItemType: "Operation",
        workItemTotalOperationTimeMilliseconds: 3000,
      },
    ],
  };
  return res(ctx.json(folder));
});

const InterceptWorkItems = { IntercepWorkItemsProject, IntercepWorkItemsSubFolder, IntercepWorkItemsSubFolderTwo };
export default InterceptWorkItems;
