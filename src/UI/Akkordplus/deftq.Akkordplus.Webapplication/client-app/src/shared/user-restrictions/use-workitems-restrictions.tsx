import { ProjectFolderResponse, ProjectResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { Role, useRoleRestriction } from "./use-role-restriction";

export const useWorkItemRestrictions = (project: ProjectResponse) => {
  const { hasRole } = useRoleRestriction(project);

  const canCreateWorkitem = (folder?: ProjectFolderResponse): boolean => {
    if (!folder) {
      return false;
    }
    if (hasRole([Role.Owner])) {
      return true;
    }
    return hasRole([Role.Participant]) && folder?.projectFolderLocked === "Unlocked";
  };

  const canEditWorkitem = (folder?: ProjectFolderResponse): boolean => {
    if (!folder) {
      return false;
    }
    if (hasRole([Role.Owner])) {
      return true;
    }
    return hasRole([Role.Participant]) && folder?.projectFolderLocked === "Unlocked";
  };

  const canSelectWorkItem = (folder?: ExtendedProjectFolder): boolean => {
    if (!folder) {
      return false;
    }
    if (hasRole([Role.Owner])) {
      return true;
    }
    return hasRole([Role.Participant]) && folder?.projectFolderLocked === "Unlocked";
  };

  const canDeleteWorkitems = (folder?: ExtendedProjectFolder, selectedWorkItems?: Set<string>): boolean => {
    if (!selectedWorkItems || selectedWorkItems.size <= 0) {
      return false;
    }
    if (!folder) {
      return false;
    }
    if (hasRole([Role.Owner])) {
      return true;
    }
    return hasRole([Role.Participant]) && folder?.projectFolderLocked === "Unlocked";
  };

  const canMoveWorkitems = (folder?: ExtendedProjectFolder, selectedWorkItems?: Set<string>): boolean => {
    if (!selectedWorkItems || selectedWorkItems.size <= 0) {
      return false;
    }
    if (!folder) {
      return false;
    }
    if (hasRole([Role.Owner])) {
      return true;
    }
    return hasRole([Role.Participant]) && folder?.projectFolderLocked === "Unlocked";
  };

  const canCopyWorkitems = (folder?: ExtendedProjectFolder, selectedWorkItems?: Set<string>): boolean => {
    if (!selectedWorkItems || selectedWorkItems.size <= 0) {
      return false;
    }
    if (!folder) {
      return false;
    }
    if (hasRole([Role.Owner])) {
      return true;
    }
    return hasRole([Role.Participant]) && folder?.projectFolderLocked === "Unlocked";
  };

  return { canCreateWorkitem, canEditWorkitem, canSelectWorkItem, canDeleteWorkitems, canMoveWorkitems, canCopyWorkitems };
};
