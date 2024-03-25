import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Grid from "@mui/material/Grid";
import { DocumentReferenceResponse, GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import { useSortDocuments } from "../hooks/use-sort-documents";
import { DocumentDialogRow } from "./document-dialog-row/document-dialog-row";

interface Props extends DialogBaseProps {
  project: GetProjectResponse;
  folder: ProjectFolderResponse;
}

export const FolderDocuments = (props: Props) => {
  const { project, folder } = props;
  const { t } = useTranslation();
  const sortDocuments = useSortDocuments();
  const [documents, setDocuments] = useState<DocumentReferenceResponse[] | undefined | null>(folder.documents ? [...folder.documents] : undefined);

  const documentDeleted = (id: string) => {
    const res = documents?.filter((doc) => doc.documentId !== id);
    if (res?.length === 0 && props.onClose) {
      props.onClose();
      return;
    }
    setDocuments(res);
  };

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitleStyled title={t("content.overview.documents.title")} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent>
        <Grid container>
          {documents &&
            sortDocuments(documents).map((doc) => (
              <DocumentDialogRow key={doc.documentId} project={project} folder={folder} document={doc} documentDeletedProps={documentDeleted} />
            ))}
        </Grid>
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.close")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
