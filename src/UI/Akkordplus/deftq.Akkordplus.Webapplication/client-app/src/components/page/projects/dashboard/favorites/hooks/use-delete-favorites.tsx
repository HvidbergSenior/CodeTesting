import { useTranslation } from "react-i18next";
import { GetProjectResponse, useDeleteApiProjectsByProjectIdFavoritesMutation } from "api/generatedApi";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useToast } from "shared/toast/hooks/use-toast";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";

interface Props {
  project: GetProjectResponse;
  selectedIds: Set<string>;
}

export function useDeleteFavorites(props: Props) {
  const { project, selectedIds } = props;
  const { t } = useTranslation();
  const toast = useToast();
  const { canDeleteFavorites } = useDashboardRestrictions(project);
  const [deleteFavorites] = useDeleteApiProjectsByProjectIdFavoritesMutation();

  const handleOnSuccessClose = () => {
    selectedIds.clear();
  };

  const wrapMutation = useMutation({
    onSuccess: handleOnSuccessClose,
    successProps: {
      description: t("dashboard.favorits.delete.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      deleteFavorites({
        projectId: project.id ? project.id : "",
        removeProjectFavoritesRequest: { favoriteIds: Array.from(selectedIds) },
      })
    );
  };

  const openConfirmDialog = useConfirm({
    title: t("dashboard.favorits.delete.title"),
    description: t("dashboard.favorits.delete.description"),
    submitButtonLabel: t("common.delete"),
    submit: handleOnSubmit,
  });

  const handleClick = () => {
    if (!canDeleteFavorites(selectedIds)) {
      toast.error(t("dashboard.favorits.delete.restrictionError"));
      return;
    }
    openConfirmDialog();
  };

  return handleClick;
}
