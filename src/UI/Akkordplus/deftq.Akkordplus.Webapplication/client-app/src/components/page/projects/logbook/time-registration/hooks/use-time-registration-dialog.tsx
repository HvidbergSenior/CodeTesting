import {
  GetProjectLogBookDayResponse,
  GetProjectResponse,
  RegisterProjectLogBookDay,
  usePostApiProjectsByProjectIdLogbookWeeksMutation,
  GetProjectLogBookWeekQueryResponse,
  LogBookUserResponse,
} from "api/generatedApi";
import { TimeRegistrationDialog } from "components/page/projects/logbook/time-registration/time-registration-dialog";
import { useDialog } from "shared/dialog/use-dialog";
import { ResultDialogType, useMutation } from "utils/hooks/use-mutation";
import { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";

interface Props {
  project: GetProjectResponse;
  data: GetProjectLogBookWeekQueryResponse | undefined;
  userId: LogBookUserResponse["userId"] | undefined;
}

export function useTimeRegistrationDialog(props: Props) {
  const { project, userId } = props;
  const { t } = useTranslation();
  const [openTimeRegistrationDialog, closeTimeRegistrationDialog] = useDialog(TimeRegistrationDialog);
  const [registerTime] = usePostApiProjectsByProjectIdLogbookWeeksMutation();

  const wrapMutation = useMutation({
    onSuccess: closeTimeRegistrationDialog,
    successProps: {
      description: t("logbook.timeRegistration.success.description"),
    },
    resultDialogType: ResultDialogType.Toast,
  });

  const handleSubmit: (day: GetProjectLogBookDayResponse) => SubmitHandler<RegisterProjectLogBookDay> = (day) => (data) => {
    wrapMutation(
      registerTime({
        projectId: project.id ?? "",
        registerProjectLogBookWeekRequest: {
          year: props.data?.year,
          week: props.data?.week,
          note: props.data?.note,
          userId: userId,
          days: [
            {
              date: day?.date ?? "",
              hours: data.hours ?? 0,
              minutes: data.minutes ?? 0,
            },
          ],
        },
      })
    );
  };

  const handleClick = (day: GetProjectLogBookDayResponse) => {
    openTimeRegistrationDialog({
      day,
      onSubmit: handleSubmit(day),
    });
  };

  return handleClick;
}
