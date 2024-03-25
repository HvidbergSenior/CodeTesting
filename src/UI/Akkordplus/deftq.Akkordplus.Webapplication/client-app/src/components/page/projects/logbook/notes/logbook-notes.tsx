import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import styled from "@mui/system/styled";
import { GetProjectLogBookWeekQueryResponse, GetProjectResponse } from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { useLogbookNotesDialog } from "./dialog/hooks/use-notes-dialog";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { ShowStringDialog } from "components/shared/show-string-dialog/show-string-dialog";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { useLogbookRestrictions } from "shared/user-restrictions/use-logbook-restrictions";

interface Props {
  project: GetProjectResponse;
  logbookWeekData: GetProjectLogBookWeekQueryResponse;
  selectedUserId: string;
}

export const ProjectLogbookNotes = (props: Props) => {
  const { project, logbookWeekData, selectedUserId } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const { canEditLogbook } = useLogbookRestrictions(project);

  const openLogbookNoteDialog = useLogbookNotesDialog({ project, data: logbookWeekData, userId: selectedUserId });
  const handleOpenLogbookNote = () => {
    if (logbookWeekData) {
      openLogbookNoteDialog(logbookWeekData);
    }
  };

  const [openNoteDialog] = useDialog(ShowStringDialog);
  const showMoreLogbookNoteActionClicked = () => {
    if (logbookWeekData?.note) {
      openNoteDialog({ stringValue: logbookWeekData.note });
    }
  };

  const DescriptionTypographyStyled = styled(Typography)`
    height: 100%;
    white-space: pre-wrap;
  `;

  return (
    <Box sx={{ mb: screenSize === ScreenSizeEnum.Mobile ? 1 : 2, ml: 0.5, mr: 0.5 }}>
      <CardWithHeaderAndFooter
        titleNamespace={"common.note"}
        height={screenSize === ScreenSizeEnum.Mobile ? "130px" : "300px"}
        headerActionIcon={undefined}
        showHeaderAction={canEditLogbook(logbookWeekData, selectedUserId)}
        showBottomAction={"showMore"}
        headerActionClickedProps={handleOpenLogbookNote}
        bottomActionClickedProps={showMoreLogbookNoteActionClicked}
        showContent={!!logbookWeekData.note}
        noContentText={t("logbook.description.noDescription")}
        description={undefined}
        showDescription={false}
        hasChildPadding={true}
      >
        <DescriptionTypographyStyled variant="body1">{logbookWeekData.note}</DescriptionTypographyStyled>
      </CardWithHeaderAndFooter>
    </Box>
  );
};
