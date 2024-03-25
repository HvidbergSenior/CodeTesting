import DeleteIcon from "@mui/icons-material/Delete";
import IconButton from "@mui/material/IconButton";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { ProjectResponse } from "api/generatedApi";
import { useDeleteProject } from "components/page/projects/hooks/use-delete-projects";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { useProjectRestrictions } from "shared/user-restrictions/use-project-restrictions";

interface PropsRow {
  project: ProjectResponse;
}

export const ProjectTableRow = ({ project }: PropsRow) => {
  const { canDeleteProject } = useProjectRestrictions();
  const { screenSize } = useScreenSize();
  const navigate = useNavigate();
  const { t } = useTranslation();

  const openDeleteProjectDialog = useDeleteProject();
  const handleDeleteProject = (event: React.MouseEvent<HTMLElement>, projectId: string) => {
    event.stopPropagation();
    openDeleteProjectDialog(projectId);
  };

  const handleProjectSelect = (event: React.MouseEvent<HTMLElement>, projectId?: string) => {
    event.stopPropagation();

    if (projectId) {
      localStorage.setItem("projectId", projectId);
      navigate(`/projects/${projectId}`);
    }
  };

  return (
    <TableRow data-testid={`overview-grouping-dialog-row-${project.projectName}`} onClick={(e) => handleProjectSelect(e, project.projectId)}>
      <TableCell sx={{ maxWidth: "300px" }} data-testid="overview-grouping-dialog-row-title" title={project.projectName ?? ""}>
        <Typography variant="body2">{project.projectName}</Typography>
      </TableCell>

      <TableCell sx={{ textAlign: "center", paddingRight: "40px" }}>{t("project.pieceworkTypes." + project.pieceworkType + ".short")}</TableCell>
      {screenSize !== ScreenSizeEnum.Mobile && <TableCell>{project.description}</TableCell>}
      <TableCell align="center">
        {canDeleteProject() && (
          <IconButton onClick={(e) => handleDeleteProject(e, project.projectId ?? "")}>
            <DeleteIcon />
          </IconButton>
        )}
      </TableCell>
    </TableRow>
  );
};
