import { ProjectResponse } from "api/generatedApi";
import { Role, useRoleRestriction } from "./use-role-restriction";

export const useRateRestrictions = (project: ProjectResponse) => {
  const { hasRole } = useRoleRestriction(project);

  const canEditBaseRateAndSupplements = (): boolean => {
    if (hasRole([Role.Owner])) {
      return true;
    }
    return false;
  };

  return { canEditBaseRateAndSupplements };
};
