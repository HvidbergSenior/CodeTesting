import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { ProjectFolderResponse } from "api/generatedApi";

export const isAllRatesAndSupplementsInherited = (folder: ExtendedProjectFolder | ProjectFolderResponse | undefined) => {
  if (!folder) {
    return true;
  }
  const indirectTimeIsOverwritten = folder.baseRateAndSupplements?.indirectTimeSupplementPercentage?.valueStatus === "Inherit";
  const siteSpecificTimeIsOverwritten = folder.baseRateAndSupplements?.siteSpecificTimeSupplementPercentage?.valueStatus === "Inherit";
  const baseRateRegulationIsOverwritten = folder.baseRateAndSupplements?.baseRateRegulationPercentage?.valueStatus === "Inherit";
  return indirectTimeIsOverwritten && siteSpecificTimeIsOverwritten && baseRateRegulationIsOverwritten;
};
