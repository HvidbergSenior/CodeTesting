import { useTranslation } from "react-i18next";
import {
  DocumentReferenceResponse,
  ProjectFolderResponse,
  GetProjectResponse,
  useDeleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentIdMutation,
} from "api/generatedApi";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";
import { formatTimestamp } from "utils/formats";

interface Props {
  project: GetProjectResponse;
  folder: ProjectFolderResponse;
  document: DocumentReferenceResponse;
  onCloseSuccessProps: (id: string) => void;
}

export function useDeleteDocument(props: Props) {
  const { project, folder, document, onCloseSuccessProps } = props;
  const { t } = useTranslation();
  const [deleteDocument] = useDeleteApiProjectsByProjectIdFoldersAndFolderIdDocumentsDocumentIdMutation();

  const onCloseSuccess = () => {
    onCloseSuccessProps(document.documentId ?? "");
  };
  const wrapMutation = useMutation({
    onSuccess: onCloseSuccess,
    successProps: {
      description: t("content.overview.documents.delete.success.description", { documentname: document.name }),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      deleteDocument({
        projectId: project.id ? project.id : "",
        folderId: folder?.projectFolderId ? folder.projectFolderId : "",
        documentId: document.documentId ? document.documentId : "",
      })
    );
  };

  const openConfirmDialog = useConfirm({
    title: t("content.overview.documents.delete.title", { documentname: document.name }),
    description: t("content.overview.documents.delete.description", {
      documentname: document.name,
      uploaddate: formatTimestamp(document.uploadedTimestamp),
    }),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = () => {
    openConfirmDialog();
  };

  return handleClick;
}
