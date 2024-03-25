import { Grid, Typography } from "@mui/material";
import Box from "@mui/system/Box";
import { GetProjectResponse } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useTranslation } from "react-i18next";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { FolderLockedButton } from "../../lock-folder/folder-locked-button";
import { OverviewDescription } from "./description/overview-description";
import { OverviewDocuments } from "./documents/overview-documents";
import { FolderExtraWorkButton } from "./extra-work/folder-extra-work-button";
import { GroupingWidget } from "./overview-grouping/widget/grouping-widget";
import DefaultSummations from "./summations/default-summations";

interface Props {
  folder: ExtendedProjectFolder | undefined;
  project: GetProjectResponse;
}

export const FolderOverview = (props: Props) => {
  const { folder, project } = props;
  const spacing = 2;
  const { t } = useTranslation();

  const { screenSize } = useScreenSize();
  const maxWidth = screenSize === ScreenSizeEnum.LargeDesktop ? "1200px" : "900px";

  return (
    <Grid container alignItems={"center"} maxWidth={maxWidth} marginLeft={"auto"} marginRight={"auto"} rowSpacing={spacing} overflow={"auto"}>
      <Grid item xs={12} pb={0} style={{ display: "flex", justifyContent: "space-between", alignItems: "center", gap: 2 }}>
        <Typography pt={2} variant="h5" color={"primary.main"}>
          {t("content.overview.subtitles.status")}
        </Typography>
        <Box display={"flex"} flexDirection={"row"}>
          {folder && <FolderLockedButton project={project} folder={folder} showText={true} />}
          <Box width={"10px"}></Box>
          {screenSize !== ScreenSizeEnum.Mobile && folder && !folder.isRoot && (
            <FolderExtraWorkButton project={project} folder={folder} textDirection={"Bottom"} />
          )}
        </Box>
      </Grid>
      <DefaultSummations folder={folder} project={project} />
      {screenSize === ScreenSizeEnum.Mobile && folder && !folder.isRoot && (
        <Grid item xs={12}>
          <FolderExtraWorkButton project={project} folder={folder} textDirection={"Right"} />
        </Grid>
      )}
      <Typography pt={2} variant="h5" color={"primary.main"}>
        {t("content.overview.subtitles.info")}
      </Typography>
      <Grid container item xs={12} spacing={spacing} pb={9}>
        <Grid container item xs={12} lg={6} xl={6} spacing={spacing}>
          <Grid item xs={12}>
            {folder && <OverviewDescription folder={folder} project={project} height={"200px"} />}
          </Grid>
          <Grid item xs={12}>
            {folder && <OverviewDocuments folder={folder} project={project} height={"200px"} />}
          </Grid>
        </Grid>
        <Grid item xs={12} lg={6} xl={6}>
          <GroupingWidget folderId={folder?.projectFolderId ?? ""} projectId={project.id ?? ""} />
        </Grid>
      </Grid>
    </Grid>
  );
};
