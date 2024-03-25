import { rest } from "msw";
import InterceptConfig from "./intercepts/intercept-config";
import InterceptProjects from "./intercepts/intercept-project";
import InterceptFolders from "./intercepts/intercept-folders";
import IntercepWorkItems from "./intercepts/intercept-work-items";
import InterceptCopyMeasurements from "./intercepts/intercept-copy-measurements";
import InterceptProjectsById from "./intercepts/intercept-projects-by-id";
import InterceptProjectId from "./intercepts/intercept-projectid";
import InterceptGroupedItems from "./intercepts/intercept-grouped-items";
import InterceptBaseSupplements from "./intercepts/intercept-basesupplements";
import InterceptBaseRate from "./intercepts/intercept-baserate";
import InterceptExtraWorkAgreements from "./intercepts/intercept-extrawork-agreements";

export const handlers = [
  // Handles a POST /login request
  rest.post("/login", null!),

  InterceptConfig,
  InterceptProjects,
  InterceptFolders,
  InterceptProjectId,
  InterceptProjectsById,
  IntercepWorkItems.IntercepWorkItemsProject,
  IntercepWorkItems.IntercepWorkItemsSubFolder,
  IntercepWorkItems.IntercepWorkItemsSubFolderTwo,
  InterceptCopyMeasurements.InterceptCopyMeasurementsOne,
  InterceptCopyMeasurements.InterceptCopyMeasurementsTwo,
  InterceptGroupedItems.InterceptGroupedItemsWidget,
  InterceptGroupedItems.InterceptGroupedItemsTable,
  InterceptBaseSupplements,
  InterceptBaseRate,
  InterceptExtraWorkAgreements,
];
