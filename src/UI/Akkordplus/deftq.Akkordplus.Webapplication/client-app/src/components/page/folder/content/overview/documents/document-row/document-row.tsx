import Grid from "@mui/material/Grid";
import styled from "@mui/system/styled";
import PictureAsPdfOutlinedIcon from "@mui/icons-material/PictureAsPdfOutlined";
import { DocumentReferenceResponse, GetProjectResponse } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { formatTimestamp } from "utils/formats";
import { useDownloadDocument } from "../hooks/use-download-document";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";

interface DocumentRowProps {
  project: GetProjectResponse;
  document: DocumentReferenceResponse;
}

export function DocumentRow(props: DocumentRowProps) {
  const { document, project } = props;
  const downloadFolderDocument = useDownloadDocument();
  const { canDownloadDocument } = useFolderRestrictions(project);
  const { screenSize } = useScreenSize();

  const downloadClicked = (event: React.MouseEvent<HTMLElement>, doc: DocumentReferenceResponse) => {
    event.stopPropagation();
    if (canDownloadDocument()) {
      downloadFolderDocument(doc);
    }
  };

  const DocumentRowGridStyled = styled(Grid)`
    height: 28px;
    font-size: 14px;
    cursor: pointer;
  `;

  const DocumentNameGridItemStyled = styled(Grid)`
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    padding-right: 10px;
    text-align: left;
  `;

  return (
    <DocumentRowGridStyled container key={document?.documentId} onClick={(e) => downloadClicked(e, document)}>
      <Grid item xs={1}>
        <PictureAsPdfOutlinedIcon fontSize="small" sx={{ mt: "2px" }} />
      </Grid>
      <DocumentNameGridItemStyled item xs={11} sm={6}>
        {document?.name}
      </DocumentNameGridItemStyled>
      {screenSize === ScreenSizeEnum.Desktop && (
        <Grid item xs={5}>
          {formatTimestamp(document?.uploadedTimestamp).slice(0, -5)}
        </Grid>
      )}
      {screenSize === ScreenSizeEnum.LargeDesktop && (
        <Grid item xs={5}>
          {formatTimestamp(document?.uploadedTimestamp)}
        </Grid>
      )}
    </DocumentRowGridStyled>
  );
}
