import { useTranslation } from "react-i18next";
import { SubmitHandler, useForm } from "react-hook-form";
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import TextField from "@mui/material/TextField";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import type { CreateProjectFolderRequest, ProjectFolderResponse } from "api/generatedApi";
import type { FormDialogProps } from "shared/dialog/types";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";

export interface EditFolderNameFormData extends CreateProjectFolderRequest {}

interface Props extends FormDialogProps<EditFolderNameFormData> {
  folder?: ProjectFolderResponse;
}

export function FolderEditName(props: Props) {
  const { t } = useTranslation();
  const {
    formState: { isValid },
    register,
    handleSubmit,
  } = useForm<EditFolderNameFormData>({
    mode: "all",
  });

  const onSubmit: SubmitHandler<EditFolderNameFormData> = (data) => {
    props.onSubmit(data);
  };

  return (
    <Dialog fullWidth maxWidth="sm" open={props.isOpen}>
      <DialogTitleStyled title={t("folder.editName.title")} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent>
        <Typography variant="caption" color={"primary.main"}>
          {t("folder.createEdit.nameLabel")}
        </Typography>
        <TextField
          {...register("folderName", {
            required: true,
          })}
          variant="filled"
          sx={{ width: "100%", marginBottom: 2 }}
          defaultValue={props.folder?.projectFolderName}
          label={t("folder.editName.name")}
          required
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
}
