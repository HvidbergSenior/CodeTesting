import { GetProjectLogBookWeekQueryResponse, LogBookUserResponse, usePostApiProjectsByProjectIdLogbookWeeksOpenMutation } from "api/generatedApi";
import type { GetProjectResponse } from "api/generatedApi";
import { LogbookUnlock } from "components/page/projects/logbook/unlock/unlock";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { useTranslation } from "react-i18next";

interface Props {
  project: GetProjectResponse;
  data: GetProjectLogBookWeekQueryResponse | undefined;
  userId: LogBookUserResponse["userId"] | undefined;
}

export function useLogbookUnlock(props: Props) {
  const { project, data, userId } = props;
  const { t } = useTranslation();

  const [openLogbookUnlockDialog, closeLogbookUnlockDialog] = useDialog(LogbookUnlock);
  const [unlockLogbook] = usePostApiProjectsByProjectIdLogbookWeeksOpenMutation();

  const wrapMutation = useMutation({
    onSuccess: closeLogbookUnlockDialog,
    successProps: {
      description: t("logbook.unlock.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      unlockLogbook({
        projectId: project.id ? project.id : "",
        openProjectLogBookWeekRequest: {
          userId: userId,
          year: data?.year,
          week: data?.week,
        },
      })
    );
  };

  const handleClick = () => {
    openLogbookUnlockDialog({ onSubmit: handleOnSubmit });
  };

  return handleClick;
}
