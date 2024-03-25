import { useTranslation } from "react-i18next";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import AddIcon from "@mui/icons-material/Add";
import { GetProjectResponse } from "api/generatedApi";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useUpdateFolderSupplements } from "./edit/hooks/use-update-folder-supplements";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";
import { FolderSupplementsContent } from "./content/folder-supplements-content";
import { useDialog } from "shared/dialog/use-dialog";
import { FolderSupplementsViewMore } from "./view-more/folder-supplements-view-more";
import { useFolderSupplementsMapper } from "./hooks/use-folder-supplements-mapper";

interface Props {
  project: GetProjectResponse;
  folder: ExtendedProjectFolder;
}

export const FolderSupplements = ({ folder, project }: Props) => {
  const { t } = useTranslation();
  const updateFolderSupplements = useUpdateFolderSupplements({ folder, project });
  const { canUpdateSupplementsOnFolder } = useFolderRestrictions(project);
  const [openDialog] = useDialog(FolderSupplementsViewMore);
  const mapper = useFolderSupplementsMapper({ folder });
  const mappedSupplements = mapper();

  const hasFolderSupplements = folder.folderSupplements && folder.folderSupplements.length > 0;
  const hasSupplements = mappedSupplements.length > 0;

  const addIcon = (
    <Box color="primary.main" sx={{ position: "relative" }}>
      <AddIcon />
    </Box>
  );

  return (
    <Grid item xs={12} lg={4} xl={4} display={"flex"} justifyContent={"center"}>
      <CardWithHeaderAndFooter
        titleNamespace={t("content.calculation.folderSupplements.title")}
        description={t("content.calculation.folderSupplements.description")}
        height={"400px"}
        headerActionIcon={hasFolderSupplements ? undefined : addIcon}
        showHeaderAction={canUpdateSupplementsOnFolder()}
        showBottomAction={mappedSupplements.length > 2 ? "showMore" : "none"}
        headerActionClickedProps={updateFolderSupplements}
        bottomActionClickedProps={() => openDialog({ folder })}
        showContent={hasSupplements}
        noContentText={t("content.calculation.folderSupplements.emptyState")}
        showDescription={true}
        hasChildPadding={false}
      >
        <Box sx={{ pt: 3, pl: 5, pr: 4, height: "240px" }}>
          <FolderSupplementsContent supplements={mappedSupplements} showMax={2} />
        </Box>
      </CardWithHeaderAndFooter>
    </Grid>
  );
};
