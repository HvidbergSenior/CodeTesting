import { GetProjectResponse } from "api/generatedApi";

export enum Role {
  Participant, // akkorddeltager
  Owner, // akkordejer
  Manager, // akkordholder
}

export const useRoleRestriction = (project: GetProjectResponse) => {
  const hasRole = (roles: Role[]): boolean => {
    if (project.currentUserRole === "ProjectOwner" && roles.includes(Role.Owner)) {
      return true;
    }

    if (project.currentUserRole === "ProjectParticipant" && roles.includes(Role.Participant)) {
      return true;
    }

    if (project.currentUserRole === "ProjectManager" && roles.includes(Role.Manager)) {
      return true;
    }

    return false;
  };

  return { hasRole };
};
