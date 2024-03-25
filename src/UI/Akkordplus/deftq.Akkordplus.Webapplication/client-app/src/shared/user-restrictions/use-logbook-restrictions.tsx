import { GetProjectLogBookWeekQueryResponse, GetProjectResponse } from "api/generatedApi";
import { Role, useRoleRestriction } from "./use-role-restriction";
import { getPCA } from "index";

export const useLogbookRestrictions = (project: GetProjectResponse) => {
  const { hasRole } = useRoleRestriction(project);
  const activeAccount = getPCA().getActiveAccount()?.localAccountId;

  const canSelectLogbookParticipant = (): boolean => {
    return hasRole([Role.Participant, Role.Owner, Role.Manager]);
  };

  const canViewLogbook = (): boolean => {
    return hasRole([Role.Participant, Role.Owner, Role.Manager]);
  };

  const canEditLogbook = (logbook?: GetProjectLogBookWeekQueryResponse, selectedUserId?: string): boolean => {
    if (!activeAccount || !selectedUserId || !logbook) {
      return false;
    }
    if (hasRole([Role.Owner])) {
      return true;
    }
    if (hasRole([Role.Manager])) {
      return false;
    }
    if (logbook.closed) {
      return false;
    }
    if (selectedUserId === activeAccount) {
      return true;
    }
    return false;
  };

  const canCloseLogbookPeriod = (logbook?: GetProjectLogBookWeekQueryResponse, selectedUserId?: string): boolean => {
    if (!activeAccount || !selectedUserId || !logbook) {
      return false;
    }
    if (hasRole([Role.Owner])) {
      return true;
    }
    if (hasRole([Role.Manager])) {
      return false;
    }
    if (logbook.closed) {
      return false;
    }
    if (selectedUserId === activeAccount) {
      return true;
    }
    return false;
  };

  const canOpenLogbookPeriod = (logbook?: GetProjectLogBookWeekQueryResponse): boolean => {
    if (!logbook?.closed) {
      return false;
    }
    return hasRole([Role.Owner]);
  };

  const canViewSalaryAdvance = (selectedUserId?: string) => {
    if (hasRole([Role.Owner])) {
      return true;
    }
    if (hasRole([Role.Manager])) {
      return false;
    }
    if (!activeAccount || !selectedUserId) {
      return false;
    }
    if (selectedUserId === activeAccount) {
      return true;
    }

    return false;
  };

  const canEditSalaryAdvance = (): boolean => {
    if (hasRole([Role.Owner])) {
      return true;
    }
    return false;
  };

  return {
    canSelectLogbookParticipant,
    canViewLogbook,
    canEditLogbook,
    canCloseLogbookPeriod,
    canOpenLogbookPeriod,
    canViewSalaryAdvance,
    canEditSalaryAdvance,
  };
};
