import { GetProjectResponse } from "api/generatedApi";
import { Role, useRoleRestriction } from "./use-role-restriction";

export const useDashboardRestrictions = (project: GetProjectResponse) => {
  const { hasRole } = useRoleRestriction(project);

  // Favorites
  const canCreateFavorites = (): boolean => {
    return hasRole([Role.Manager, Role.Owner]);
  };
  const canSelectFavorites = (): boolean => {
    return hasRole([Role.Manager, Role.Owner]);
  };
  const canDeleteFavorites = (selectedItems: Set<string>): boolean => {
    if (!selectedItems || selectedItems.size <= 0) {
      return false;
    }
    return hasRole([Role.Manager, Role.Owner]);
  };

  // Extra Agreements
  const canEditExtraAgreementsRates = (): boolean => {
    return hasRole([Role.Owner]);
  };
  const canCreateExtraWorkAgreements = (): boolean => {
    return hasRole([Role.Manager, Role.Owner]);
  };
  const canSelectExtraWorkAgreements = (): boolean => {
    return true;
  };
  const canEditExtraWorkAgreement = (): boolean => {
    return hasRole([Role.Manager, Role.Owner]);
  };
  const canDeleteExtraWorkAgreements = (selectedItems: Set<string>): boolean => {
    if (!selectedItems || selectedItems.size <= 0) {
      return false;
    }
    return hasRole([Role.Manager, Role.Owner]);
  };

  // Agreed Lumpsum
  const canEditAgreedLumpsum = (): boolean => {
    return hasRole([Role.Owner]);
  };

  // Project setup
  const canEditProjectSetupProjectType = (): boolean => {
    return hasRole([Role.Owner, Role.Manager]);
  };
  const canEditProjectSetupProjectName = (): boolean => {
    return hasRole([Role.Owner, Role.Manager]);
  };
  const canEditProjectSetupProjectCompany = (): boolean => {
    return hasRole([Role.Owner, Role.Manager]);
  };

  const canCreateProjectUsers = (): boolean => {
    return hasRole([Role.Owner]);
  };

  // Compensation payment
  const canViewCompensationPayment = (): boolean => {
    return hasRole([Role.Owner, Role.Manager]);
  };
  const canCreateCompensationPayment = (): boolean => {
    return hasRole([Role.Owner]);
  };
  const canSelectCompensationPayments = (): boolean => {
    return hasRole([Role.Owner]);
  };
  const canDeleteCompensationPayments = (selectedItems: Set<string>): boolean => {
    if (!selectedItems || selectedItems.size <= 0) {
      return false;
    }
    return hasRole([Role.Owner]);
  };

  // Projects Specific Operation
  const canCreateProjectSpecificOperation = (): boolean => {
    return hasRole([Role.Owner]);
  };
  const canSelectProjectSpecificOperations = (): Boolean => {
    return hasRole([Role.Owner]);
  };
  const canEditProjectSpecificOperation = (): boolean => {
    return hasRole([Role.Owner]);
  };
  const canDeleteProjectSpecificOperations = (selectedItems: Set<string>): boolean => {
    if (!selectedItems || selectedItems.size <= 0) {
      return false;
    }
    return hasRole([Role.Owner]);
  };

  // Reports
  const canDownloadStatusReport = (): boolean => {
    return hasRole([Role.Owner, Role.Manager]);
  };

  return {
    canCreateFavorites,
    canSelectFavorites,
    canDeleteFavorites,
    canCreateExtraWorkAgreements,
    canSelectExtraWorkAgreements,
    canEditExtraWorkAgreement,
    canDeleteExtraWorkAgreements,
    canEditExtraAgreementsRates,
    canEditAgreedLumpsum,
    canEditProjectSetupProjectType,
    canEditProjectSetupProjectName,
    canEditProjectSetupProjectCompany,
    canCreateProjectUsers,
    canViewCompensationPayment,
    canCreateCompensationPayment,
    canSelectCompensationPayments,
    canDeleteCompensationPayments,
    canCreateProjectSpecificOperation,
    canSelectProjectSpecificOperations,
    canEditProjectSpecificOperation,
    canDeleteProjectSpecificOperations,
    canDownloadStatusReport,
  };
};
