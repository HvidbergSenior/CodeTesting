import {
  GetProjectResponse,
  GetProjectLogBookWeekQueryResponse,
  usePostApiProjectsByProjectIdLogbookSalaryadvanceMutation,
  RegisterLogbookSalaryAdvanceRequest,
} from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { EditSalaryAdvanceDialog } from "../edit-salary-advance-dialog";

interface Props {
  project: GetProjectResponse;
  logbookWeekData: GetProjectLogBookWeekQueryResponse;
  selectedUserId?: string;
}

export function useEditSalaryAdvance(props: Props) {
  const { project, logbookWeekData, selectedUserId } = props;
  const { t } = useTranslation();
  const [openDialog, closeDialog] = useDialog(EditSalaryAdvanceDialog);
  const [update] = usePostApiProjectsByProjectIdLogbookSalaryadvanceMutation();

  const wrapMutation = useMutation({
    onSuccess: closeDialog,
    successProps: {
      description: t("logbook.timeRegistration.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleSubmit: () => SubmitHandler<RegisterLogbookSalaryAdvanceRequest> = () => (data) => {
    wrapMutation(
      update({
        projectId: project.id ?? "",
        registerLogbookSalaryAdvanceRequest: {
          userId: selectedUserId,
          amount: data.amount,
          type: data.type,
          week: logbookWeekData.week,
          year: logbookWeekData.year,
        },
      })
    );
  };

  const handleClick = () => {
    if (!logbookWeekData.salaryAdvance) {
      return;
    }

    openDialog({
      salaryAdvance: logbookWeekData.salaryAdvance,
      viewedWeek: logbookWeekData.week ?? 0,
      viewedYear: logbookWeekData.year ?? 0,
      onSubmit: handleSubmit(),
    });
  };

  return handleClick;
}
