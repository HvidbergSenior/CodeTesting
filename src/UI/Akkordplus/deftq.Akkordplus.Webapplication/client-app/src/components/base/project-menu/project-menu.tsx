import { useEffect, useState } from "react";
import type { MouseEvent } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { useMsal } from "@azure/msal-react";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";
import ListItemText from "@mui/material/ListItemText";
import ListItem from "@mui/material/ListItem";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import Typography from "@mui/material/Typography";
import LocationCityOutlinedIcon from "@mui/icons-material/LocationCityOutlined";
import CheckIcon from "@mui/icons-material/Check";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import { ProjectResponse, useGetApiProjectsQuery } from "api/generatedApi";
import { useCreateProject } from "components/page/projects/project-setup/create-project-dialog/hooks/use-create-project";
import { useProjectRestrictions } from "shared/user-restrictions/use-project-restrictions";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { useToast } from "shared/toast/hooks/use-toast";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { useOfflineStorage } from "utils/offline-storage/use-offline-storage";

export const ProjectSelectMenu = () => {
  const { t } = useTranslation();
  const { data } = useGetApiProjectsQuery();
  const { instance } = useMsal();
  const { screenSize } = useScreenSize();
  const navigate = useNavigate();
  const { canCreateProject } = useProjectRestrictions();
  const toast = useToast();
  const isOnline = useOnlineStatus();
  const { getOfflineProjectId, setOfflineProjectId } = useOfflineStorage();

  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const open = Boolean(anchorEl);
  const [projectList, setProjectList] = useState<ProjectResponse[] | undefined>(undefined);
  const [activeProject, setActiveProject] = useState<ProjectResponse | undefined>(undefined);

  const [activeProjectId, setActiveProjectId] = useState<string>(getOfflineProjectId());

  const handleLogout = () => {
    instance.logoutRedirect();
  };

  useEffect(() => {
    if (data && data.projects && data.projects.length > 0) {
      setProjectList(data.projects);
    }
    if (activeProjectId && projectList?.some((project) => project.projectId === activeProjectId)) {
      setActiveProject(projectList?.find((project) => project.projectId === activeProjectId));
    }
  }, [data, projectList, activeProjectId]);

  const openCreateProjectDialog = useCreateProject();
  const handleCreateProject = () => {
    setAnchorEl(null);
    openCreateProjectDialog();
  };

  const openMenu = (event: MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const closeMenu = () => {
    setAnchorEl(null);
  };

  const handleProjectSelect = (project?: ProjectResponse) => {
    if (!project?.projectId) {
      toast.error(t("project.menu.selectedError"));
      return;
    }
    setOfflineProjectId(project.projectId);
    setActiveProjectId(project.projectId);
    setActiveProject(project);
    setAnchorEl(null);
    navigate(`/projects/${project.projectId}`);
  };

  const navigateToAllProjects = () => {
    setActiveProject(undefined);
    setActiveProjectId("");
    navigate(`/projectlist`);
  };

  return (
    <Box>
      <Button
        data-testid="project-selector-open-menu-btn"
        variant="text"
        disabled={!isOnline}
        onClick={openMenu}
        sx={{ color: "#FFFFFF", marginTop: "3px" }}
      >
        <LocationCityOutlinedIcon sx={{ marginBottom: 0.5 }} />
        {screenSize !== ScreenSizeEnum.Mobile && (
          <Typography sx={{ marginX: 1, letterSpacing: "1px" }}>{activeProject ? activeProject.projectName : t("project.menu.select")}</Typography>
        )}
        <ArrowDropDownIcon sx={{ marginBottom: 0.5 }} />
      </Button>
      <Menu
        open={open}
        anchorEl={anchorEl}
        onClose={closeMenu}
        transformOrigin={{ horizontal: "right", vertical: "top" }}
        anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
        sx={{
          "& .MuiPaper-root": {
            backgroundColor: "primary.dark",
          },
          "& .MuiMenuItem-root:hover": {
            backgroundColor: "primary.light",
          },
        }}
      >
        {projectList &&
          projectList.map((project) => {
            return (
              <MenuItem
                key={project.projectId}
                data-testid={`project-selector-menu-${project.projectName}`}
                onClick={() => handleProjectSelect(project)}
                sx={{
                  color: "grey.50",
                }}
              >
                <ListItem sx={{ pl: 0, pb: 0.5, pt: 0.5 }}>
                  <Typography variant="subtitle1">{project.projectName}</Typography>
                  {project.projectId === activeProjectId && <CheckIcon sx={{ width: "1.1em", height: "1.1em", pl: 1, pb: 0.5 }} />}
                </ListItem>
              </MenuItem>
            );
          })}
        {canCreateProject() && (
          <MenuItem data-testid="project-selector-menu-create-project" onClick={handleCreateProject} sx={{ color: "white" }}>
            <ListItemText>
              <Typography variant="subtitle1">{t("project.create.button")}</Typography>
            </ListItemText>
          </MenuItem>
        )}

        <Divider color="white" sx={{ ml: 1, mr: 1 }} />
        <MenuItem data-testid="project-selector-menu-all-projects" onClick={navigateToAllProjects} sx={{ color: "white" }}>
          <ListItemText>
            <Typography variant="subtitle1">{t("common.allProjects")}</Typography>
          </ListItemText>
        </MenuItem>
        <Divider color="white" sx={{ ml: 1, mr: 1 }} />
        <MenuItem data-testid="project-selector-menu-logout" onClick={handleLogout} sx={{ color: "white" }}>
          <ListItemText>
            <Typography variant="subtitle1">{t("common.logout")}</Typography>
          </ListItemText>
        </MenuItem>
      </Menu>
    </Box>
  );
};
