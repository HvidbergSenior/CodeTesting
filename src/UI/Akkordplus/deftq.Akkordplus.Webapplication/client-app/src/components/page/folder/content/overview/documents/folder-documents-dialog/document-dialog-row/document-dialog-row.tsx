import Grid from "@mui/material/Grid";
import IconButton from "@mui/material/IconButton";
import styled from "@mui/system/styled";
import PictureAsPdfOutlinedIcon from "@mui/icons-material/PictureAsPdfOutlined";
import DeleteOutlinedIcon from "@mui/icons-material/DeleteOutlined";

import { DocumentReferenceResponse, ProjectFolderResponse, GetProjectResponse } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { formatTimestamp } from "utils/formats";

import { useDownloadDocument } from "../../hooks/use-download-document";
import { useDeleteDocument } from "../../hooks/use-delete-document";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";

interface DocumentRowProps {
  project: GetProjectResponse;
  folder: ProjectFolderResponse;
  document: DocumentReferenceResponse;
  documentDeletedProps: (id: string) => void;
}

export function DocumentDialogRow(props: DocumentRowProps) {
  const { project, folder, document, documentDeletedProps } = props;
  const { canDeleteDocument, canDownloadDocument } = useFolderRestrictions(project);
  const downloadFolderDocument = useDownloadDocument();
  const deleteDocument = useDeleteDocument({ project, folder, document, onCloseSuccessProps: documentDeletedProps });
  const { screenSize } = useScreenSize();

  const deleteClicked = (event: React.MouseEvent<HTMLButtonElement>) => {
    if (!document?.documentId) {
      return;
    }
    event.stopPropagation();
    deleteDocument();
  };

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
    <DocumentRowGridStyled container key={document.documentId} onClick={(e) => downloadClicked(e, document)}>
      <Grid item xs={1}>
        <PictureAsPdfOutlinedIcon fontSize="small" sx={{ mt: "2px" }} />
      </Grid>
      <DocumentNameGridItemStyled item xs={10} sm={5}>
        {document.name}
      </DocumentNameGridItemStyled>
      {screenSize !== ScreenSizeEnum.Mobile && (
        <Grid item xs={5}>
          {formatTimestamp(document.uploadedTimestamp)}
        </Grid>
      )}
      {canDeleteDocument() && (
        <Grid item xs={1} sx={{ textAlign: "right" }}>
          <IconButton onClick={(e) => deleteClicked(e)} sx={{ mt: "-8px" }}>
            <DeleteOutlinedIcon fontSize="small" />
          </IconButton>
        </Grid>
      )}
    </DocumentRowGridStyled>
  );
}
