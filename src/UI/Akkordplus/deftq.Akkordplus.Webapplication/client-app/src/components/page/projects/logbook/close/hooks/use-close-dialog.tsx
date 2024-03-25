import { useTranslation } from "react-i18next";

import {
  GetProjectResponse,
  GetProjectLogBookWeekQueryResponse,
  LogBookUserResponse,
  usePostApiProjectsByProjectIdLogbookWeeksCloseMutation,
} from "api/generatedApi";
import { LogbookCloseDialog } from "components/page/projects/logbook/close/close-dialog";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";

interface Props {
  project: GetProjectResponse;
  data: GetProjectLogBookWeekQueryResponse | undefined;
  userId: LogBookUserResponse["userId"] | undefined;
}

export function useLogbookCloseDialog(props: Props) {
  const { project, data, userId } = props;
  const { t } = useTranslation();

  const [openLogbookCloseDialog, closeLogbookCloseDialog] = useDialog(LogbookCloseDialog);
  const [closeLogbook] = usePostApiProjectsByProjectIdLogbookWeeksCloseMutation();

  const wrapMutation = useMutation({
    onSuccess: closeLogbookCloseDialog,
    successProps: {
      description: t("logbook.close.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleOnSubmit = () => {
    wrapMutation(
      closeLogbook({
        projectId: project.id ? project.id : "",
        closeProjectLogBookWeekRequest: {
          userId: userId,
          year: data?.year,
          week: data?.week,
        },
      })
    );
  };

  const handleClick = () => {
    openLogbookCloseDialog({ onSubmit: handleOnSubmit });
  };

  return handleClick;
}
