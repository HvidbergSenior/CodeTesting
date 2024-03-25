import { SubmitHandler, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import { GetProjectLogBookWeekQueryResponse } from "api/generatedApi";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import type { FormDialogProps } from "shared/dialog/types";

interface Props extends FormDialogProps<GetProjectLogBookWeekQueryResponse> {
  week: GetProjectLogBookWeekQueryResponse;
}

export const LogbookNotesDialog = (props: Props) => {
  const { t } = useTranslation();
  const { week } = props;

  const {
    formState: { isValid },
    register,
    handleSubmit,
  } = useForm<GetProjectLogBookWeekQueryResponse>({
    mode: "all",
  });

  const onSubmit: SubmitHandler<GetProjectLogBookWeekQueryResponse> = (data) => {
    props.onSubmit(data);
  };

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitleStyled title={t("logbook.description.title", { week: week.week })} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent>
        <Typography variant="caption" color={"primary.main"}>
          {t("captions.noteOptional")}
        </Typography>
        <TextField
          {...register("note")}
          label={t("common.note")}
          multiline
          sx={{ width: "100%" }}
          minRows={5}
          variant="filled"
          defaultValue={week.note}
          inputRef={(input) => input && input.focus()}
        />
      </DialogContent>
      <DialogActions>
        <Button variant="contained" color="primary" disabled={!isValid} onClick={handleSubmit(onSubmit)}>
          {t("common.save")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
