import { ProjectFolderResponse, GetProjectResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { Role, useRoleRestriction } from "./use-role-restriction";

export const useFolderRestrictions = (project: GetProjectResponse) => {
  const { hasRole } = useRoleRestriction(project);

  const canCreateFolder = (): boolean => {
    return hasRole([Role.Owner]);
  };

  const canDeleteFolder = (hasSelectedFolder: ProjectFolderResponse): boolean => {
    if (!hasSelectedFolder) {
      return false;
    }
    return hasRole([Role.Owner]);
  };

  const canEditFolder = (): boolean => {
    return hasRole([Role.Owner]);
  };

  const canMoveFolder = (): boolean => {
    return hasRole([Role.Owner]);
  };

  const canCopyFolder = (): boolean => {
    return hasRole([Role.Owner]);
  };

  const canLockFolder = (): boolean => {
    return hasRole([Role.Owner]);
  };

  const canSeeFolderMenu = (): boolean => {
    return hasRole([Role.Owner, Role.Participant]);
  };

  const canUploadeDocument = (): boolean => {
    return hasRole([Role.Owner]);
  };

  const canDeleteDocument = (): boolean => {
    return hasRole([Role.Owner]);
  };

  const canDownloadDocument = (): boolean => {
    return hasRole([Role.Owner, Role.Participant, Role.Manager]);
  };

  const canMarkExtraWork = (folder: ExtendedProjectFolder): boolean => {
    if (!folder.parent || folder.isRoot) {
      return false;
    }
    if (folder.parent.folderExtraWork === "ExtraWork") {
      return false;
    }
    return hasRole([Role.Owner]);
  };

  const canMoveDraftIntoFolder = (folder?: ExtendedProjectFolder): boolean => {
    if (hasRole([Role.Owner])) {
      return true;
    }
    return folder?.projectFolderLocked === "Unlocked";
  };

  const canUpdateSupplementsOnFolder = (): boolean => {
    return hasRole([Role.Owner]);
  };

  return {
    canCreateFolder,
    canEditFolder,
    canDeleteFolder,
    canMoveFolder,
    canCopyFolder,
    canLockFolder,
    canSeeFolderMenu,
    canUploadeDocument,
    canDeleteDocument,
    canDownloadDocument,
    canMarkExtraWork,
    canMoveDraftIntoFolder,
    canUpdateSupplementsOnFolder,
  };
};
