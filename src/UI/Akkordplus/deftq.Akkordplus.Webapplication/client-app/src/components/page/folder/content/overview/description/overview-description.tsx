import { styled, Typography } from "@mui/material";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useEditFolderDescription } from "components/page/folder/create-edit/hooks/edit-folder-description/use-edit-folder-description";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { useTranslation } from "react-i18next";
import { GetProjectResponse } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ShowStringDialog } from "components/shared/show-string-dialog/show-string-dialog";

interface Props {
  folder: ExtendedProjectFolder;
  project: GetProjectResponse;
  height: string;
}

export const OverviewDescription = (props: Props) => {
  const { folder, project, height } = props;
  const { t } = useTranslation();
  const [openDialog] = useDialog(ShowStringDialog);

  const folderDescription = folder?.projectFolderDescription;

  const openEditFolderDescriptionDialog = useEditFolderDescription({
    project,
    folder,
  });

  const headerActionClicked = () => {
    openEditFolderDescriptionDialog();
  };

  const showMoreActionClicked = () => {
    if (folderDescription) {
      openDialog({ stringValue: folderDescription });
    }
  };

  const DescriptionTypographyStyled = styled(Typography)`
    height: 100%;
    white-space: pre-wrap;
  `;

  return (
    <CardWithHeaderAndFooter
      titleNamespace={"common.note"}
      height={height}
      headerActionIcon={undefined}
      showHeaderAction={true}
      showBottomAction={"showMore"}
      headerActionClickedProps={headerActionClicked}
      bottomActionClickedProps={showMoreActionClicked}
      showContent={!!folderDescription}
      noContentText={t("content.overview.description.noDescription")}
      description={undefined}
      showDescription={false}
      hasChildPadding={true}
    >
      <DescriptionTypographyStyled variant="body1">{folderDescription}</DescriptionTypographyStyled>
    </CardWithHeaderAndFooter>
  );
};
