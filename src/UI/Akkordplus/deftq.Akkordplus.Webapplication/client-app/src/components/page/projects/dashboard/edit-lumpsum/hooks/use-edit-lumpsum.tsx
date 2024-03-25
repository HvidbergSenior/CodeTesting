import { GetProjectResponse, usePutApiProjectsByProjectIdProjectlumpsumMutation } from "api/generatedApi";
import { useTranslation } from "react-i18next";
import { useDialog } from "shared/dialog/use-dialog";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { EditAgreedLumpsum } from "../edit-lumpsum";
import { parseCurrencyToFloat } from "utils/formats";

interface Props {
  project: GetProjectResponse;
  lumpsum: number;
}

export function useEditAgreedLumpsum({ project, lumpsum }: Props) {
  const { t } = useTranslation();
  const { canEditAgreedLumpsum } = useDashboardRestrictions(project);
  const [updateAgreedLumpsum] = usePutApiProjectsByProjectIdProjectlumpsumMutation();

  const [openEditSumDialog, closeEditSumDialog] = useDialog(EditAgreedLumpsum);

  const handleSuccess = () => {
    closeEditSumDialog();
  };

  const wrapMutation = useMutation({
    onSuccess: handleSuccess,
    successProps: {
      description: t("dashboard.pieceworkTotalSaved"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = (sum: string) => {
    if (!canEditAgreedLumpsum()) return;

    const sumTemp = parseCurrencyToFloat(sum);

    wrapMutation(
      updateAgreedLumpsum({
        projectId: project.id ?? "",
        updateProjectLumpSumRequest: { lumpSumDkr: sumTemp },
      })
    );
  };

  const handleClick = () => {
    if (!canEditAgreedLumpsum()) return;

    openEditSumDialog({ onSubmit: handleOnSubmit, lumpsum });
  };

  return handleClick;
}
