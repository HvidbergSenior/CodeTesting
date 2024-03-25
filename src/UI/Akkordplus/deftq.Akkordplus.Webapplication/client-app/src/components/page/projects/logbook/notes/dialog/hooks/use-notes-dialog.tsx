import {
  GetProjectResponse,
  usePostApiProjectsByProjectIdLogbookWeeksMutation,
  GetProjectLogBookWeekQueryResponse,
  LogBookUserResponse,
} from "api/generatedApi";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { LogbookNotesDialog } from "../notes-dialog";

interface Props {
  project: GetProjectResponse;
  data: GetProjectLogBookWeekQueryResponse | undefined;
  userId: LogBookUserResponse["userId"] | undefined;
}

export function useLogbookNotesDialog(props: Props) {
  const { project, userId } = props;
  const { t } = useTranslation();
  const [openLogbookNoteDialog, closeLogbookNoteDialog] = useDialog(LogbookNotesDialog);
  const [updateNote] = usePostApiProjectsByProjectIdLogbookWeeksMutation();

  const wrapMutation = useMutation({
    onSuccess: closeLogbookNoteDialog,
    successProps: {
      description: t("logbook.description.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleSubmit: SubmitHandler<GetProjectLogBookWeekQueryResponse> = (data) => {
    wrapMutation(
      updateNote({
        projectId: project.id ?? "",
        registerProjectLogBookWeekRequest: {
          year: props.data?.year,
          week: props.data?.week,
          note: data.note,
          userId: userId,
          days: [],
        },
      })
    );
  };

  const handleClick = (week: GetProjectLogBookWeekQueryResponse) => {
    openLogbookNoteDialog({
      week,
      onSubmit: handleSubmit,
    });
  };

  return handleClick;
}
