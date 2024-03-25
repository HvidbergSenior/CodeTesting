import LocationCityOutlinedIcon from "@mui/icons-material/LocationCityOutlined";
import MuiBreadcrumbs from "@mui/material/Breadcrumbs";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import { NavLink } from "react-router-dom";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { Box, IconButton } from "@mui/material";
import type { GetProjectResponse } from "api/generatedApi";

interface Props {
  project: GetProjectResponse;
  data: ExtendedProjectFolder | undefined;
  includeGoToContent: boolean;
  folderSelectedProps: (nodeId: string) => void;
}

export const ProjectBreadcrumbs = (props: Props) => {
  const { project, data, includeGoToContent } = props;

  const breadcrumbs: ExtendedProjectFolder[] = [];
  const createBreadcrumbList = (currentFolder: ExtendedProjectFolder) => {
    breadcrumbs.push(currentFolder);
    if (currentFolder.parent) {
      createBreadcrumbList(currentFolder.parent);
    }
  };

  const renderNavigationBreadcrumb = () => {
    if (data) {
      createBreadcrumbList(data);
      if (breadcrumbs.length > 0) {
        breadcrumbs.reverse();
        return breadcrumbs.map((item: ExtendedProjectFolder, index) => {
          let to = "/projects/" + project.id;
          if (index !== 0) {
            to += "/folders/" + item.projectFolderId;
          }
          return (
            <NavLink key={index} to={to} style={{ textDecoration: "none" }}>
              <Stack key={index} direction="row" alignItems="center" gap={1}>
                {index === 0 && <LocationCityOutlinedIcon fontSize="inherit" sx={{ color: (theme) => theme.palette.primary.main }} />}
                {index !== 0 && (
                  <Typography fontSize={16} sx={{ color: (theme) => theme.palette.primary.main }}>
                    {item.projectFolderName}
                  </Typography>
                )}
              </Stack>
            </NavLink>
          );
        });
      }
    }
    return <Box></Box>;
  };

  const renderLinkBreadcrumb = () => {
    if (data) {
      createBreadcrumbList(data);
      if (breadcrumbs.length > 0) {
        breadcrumbs.reverse();
        return breadcrumbs.map((item: ExtendedProjectFolder, index) => {
          return (
            <Stack key={index} direction="row" alignItems="center" gap={1}>
              {index === 0 && (
                <IconButton onClick={() => props.folderSelectedProps(item?.projectFolderId ?? "")}>
                  <LocationCityOutlinedIcon fontSize="inherit" sx={{ color: (theme) => theme.palette.primary.main }} />
                </IconButton>
              )}
              {index !== 0 && (
                <Box onClick={() => props.folderSelectedProps(item?.projectFolderId ?? "")}>
                  <Typography fontSize={16} sx={{ color: (theme) => theme.palette.primary.main }}>
                    {item.projectFolderName}
                  </Typography>
                </Box>
              )}
            </Stack>
          );
        });
      }
    }
    return <Box></Box>;
  };

  return (
    <MuiBreadcrumbs separator=">" maxItems={3}>
      {includeGoToContent ? renderNavigationBreadcrumb() : renderLinkBreadcrumb()}
    </MuiBreadcrumbs>
  );
};
