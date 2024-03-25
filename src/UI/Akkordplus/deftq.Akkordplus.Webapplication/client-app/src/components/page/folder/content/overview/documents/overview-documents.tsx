import { useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import CircularProgress from "@mui/material/CircularProgress";
import FileUploadOutlinedIcon from "@mui/icons-material/FileUploadOutlined";

import { DocumentReferenceResponse, GetProjectResponse, useGetApiConfigQuery } from "api/generatedApi";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { useDialog } from "shared/dialog/use-dialog";
import { FolderDocuments } from "./folder-documents-dialog/folder-documents";
import { useSortDocuments } from "./hooks/use-sort-documents";
import { useUploadDocument } from "./hooks/use-upload-document";
import { DocumentRow } from "./document-row/document-row";
import { useToast } from "shared/toast/hooks/use-toast";
import { useFolderRestrictions } from "shared/user-restrictions/use-folder-restrictions";

interface Props {
  folder: ExtendedProjectFolder;
  project: GetProjectResponse;
  height: string;
}

export const OverviewDocuments = (props: Props) => {
  const { folder, project, height } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canUploadeDocument } = useFolderRestrictions(project);
  const [openDialog] = useDialog(FolderDocuments);
  const { data: configData } = useGetApiConfigQuery();
  const [fileUploading, setFileUploading] = useState(false);

  const clickUploadDocument = useUploadDocument(folder, project, configData?.maxUploadFileSizeMB);
  const sortDocuments = useSortDocuments();

  // to use a hidden upload file input and click on it
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const maxViewDocumets = 4;

  const uploadDocument = () => {
    if (fileUploading) return;
    if (!fileInputRef || !fileInputRef.current) return;
    fileInputRef.current.click();
  };

  const getAllowedNumberOfDocuments = (documents?: DocumentReferenceResponse[] | null) => {
    if (!documents || documents.length <= 0) {
      return [];
    }
    const sortedDocuments = sortDocuments(documents);
    return sortedDocuments?.length > maxViewDocumets ? sortedDocuments.slice(0, maxViewDocumets) : sortedDocuments;
  };

  const handleFileChange = async (event: any) => {
    setFileUploading(true);
    clickUploadDocument(event)
      .then((filename) => {
        if (!filename || filename === "") throw new Error();
        toast.success(t("content.overview.documents.documentUploaded", { filename: filename }));
      })
      .catch((error) => {
        if (error instanceof RangeError) {
          toast.error(t("content.overview.documents.uploadFileSizeError", { maxFileSize: configData?.maxUploadFileSizeMB ?? 10 }));
        } else {
          toast.error(t("content.overview.documents.uploadError"));
          console.error(error);
        }
      })
      .finally(() => {
        setFileUploading(false);
      });
  };

  const fileUploadIcon = (
    <Box sx={{ position: "relative" }}>
      <FileUploadOutlinedIcon />
      {fileUploading && <CircularProgress size={30} sx={{ position: "absolute", top: -3, left: -3, zIndex: 1 }} />}
    </Box>
  );

  return (
    <Box>
      <CardWithHeaderAndFooter
        titleNamespace={"content.overview.documents.title"}
        height={height}
        headerActionIcon={fileUploadIcon}
        showHeaderAction={canUploadeDocument()}
        showBottomAction={!!folder.documents && folder.documents.length > 0 ? "showMore" : "none"}
        headerActionClickedProps={uploadDocument}
        bottomActionClickedProps={() => openDialog({ project, folder })}
        showContent={!!folder.documents?.length && folder.documents?.length > 0}
        noContentText={t("content.overview.documents.noDocuments")}
        description={undefined}
        showDescription={false}
        hasChildPadding={true}
      >
        <Grid container>
          {getAllowedNumberOfDocuments(folder?.documents)?.map((doc) => (
            <DocumentRow key={doc.documentId} project={project} document={doc} />
          ))}
        </Grid>
      </CardWithHeaderAndFooter>
      <input style={{ display: "none" }} ref={fileInputRef} type="file" accept="application/pdf, .png, .jpg, .jpeg" onChange={handleFileChange} />
    </Box>
  );
};
