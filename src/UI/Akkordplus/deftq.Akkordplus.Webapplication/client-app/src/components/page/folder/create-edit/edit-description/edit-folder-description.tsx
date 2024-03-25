import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import type { CreateProjectFolderRequest, ProjectFolderResponse } from "api/generatedApi";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { SubmitHandler, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import type { FormDialogProps } from "shared/dialog/types";

export interface CreateProjectFormData extends CreateProjectFolderRequest {}

interface Props extends FormDialogProps<CreateProjectFormData> {
  folder?: ProjectFolderResponse;
}

export function FolderEditDescription(props: Props) {
  const { t } = useTranslation();
  const {
    formState: { isValid },
    register,
    handleSubmit,
  } = useForm<CreateProjectFormData>({
    mode: "all",
  });

  const onSubmit: SubmitHandler<CreateProjectFormData> = (data) => {
    props.onSubmit(data);
  };

  return (
    <Dialog fullWidth maxWidth="sm" open={props.isOpen}>
      <DialogTitleStyled
        title={t("folder.editDescription.title")}
        description={t("folder.createEdit.description.description")}
        onClose={props.onClose}
        isOpen={props.isOpen}
      />
      <DialogContent>
        <Typography variant="caption" color={"primary.main"}>
          {t("captions.noteOptional")}
        </Typography>
        <TextField
          {...register("folderDescription")}
          variant="filled"
          sx={{ width: "100%" }}
          multiline
          minRows={4}
          defaultValue={props.folder?.projectFolderDescription}
          label={t("folder.createEdit.description.input")}
        />
      </DialogContent>
      <DialogActions>
        <Button variant="contained" color="primary" disabled={!isValid} onClick={handleSubmit(onSubmit)}>
          {t("common.save")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
