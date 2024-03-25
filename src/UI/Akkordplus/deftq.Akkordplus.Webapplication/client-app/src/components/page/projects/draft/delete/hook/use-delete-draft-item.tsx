import { GetProjectResponse } from "api/generatedApi";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";
import { useTranslation } from "react-i18next";
import { useToast } from "shared/toast/hooks/use-toast";
import { useOfflineStorage } from "utils/offline-storage/use-offline-storage";

interface Props {
  project: GetProjectResponse;
  selectedIds: Set<string>;
  updateTable: () => void;
}

export function useDeleteDraftItem(props: Props) {
  const { project, selectedIds, updateTable } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { removeOfflineDraftWorkItems } = useOfflineStorage();

  const handleOnSubmit = () => {
    const result = removeOfflineDraftWorkItems(project.id ?? "", Array.from(selectedIds));
    if (result) {
      selectedIds.clear();
      updateTable();
      toast.success(t("project.offline.delete.success"));
    } else {
      toast.error(t("project.offline.delete.error"));
    }
  };

  const openConfirmDialog = useConfirm({
    title: t("project.offline.delete.title"),
    description: t("project.offline.delete.description"),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = () => {
    openConfirmDialog();
  };

  return handleClick;
}
