import { makeFullPath } from "api/path";
import { DocumentReferenceResponse } from "api/generatedApi";
import { useDownloadFile } from "components/shared/download-file/use-download-file";
import { useToast } from "shared/toast/hooks/use-toast";
import { useTranslation } from "react-i18next";

export function useDownloadDocument() {
  const downloadFile = useDownloadFile();
  const { t } = useTranslation();
  const toast = useToast();

  const clickDownloadDocument = async (folderDoc: DocumentReferenceResponse) => {
    if (!folderDoc || !folderDoc.documentId || !folderDoc.name) return;

    const docName = folderDoc?.name ?? t("content.overview.documents.downloadUnknownDoc");
    const url = makeFullPath(`/api/documents/${folderDoc.documentId}`);
    try {
      downloadFile(url, docName);
    } catch {
      toast.error(t("content.overview.documents.downloadError"));
    }
  };

  return clickDownloadDocument;
}
