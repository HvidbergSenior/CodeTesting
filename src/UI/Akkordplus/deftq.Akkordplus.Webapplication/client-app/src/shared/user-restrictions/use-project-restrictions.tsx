export const useProjectRestrictions = () => {
  const canCreateProject = (): boolean => {
    return true;
  };

  const canDeleteProject = (): boolean => {
    return true;
  };

  return { canCreateProject, canDeleteProject };
};
