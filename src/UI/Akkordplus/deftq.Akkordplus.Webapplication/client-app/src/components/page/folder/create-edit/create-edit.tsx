import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import Divider from "@mui/material/Divider";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import { useTranslation } from "react-i18next";
import { SubmitHandler, useForm } from "react-hook-form";
import type { FormDialogProps } from "shared/dialog/types";
import CloseIcon from "@mui/icons-material/Close";
import type { CreateProjectFolderRequest, ProjectFolderResponse } from "api/generatedApi";

export interface CreateFolderFormData extends CreateProjectFolderRequest {
  folderDescription: string;
}

interface Props extends FormDialogProps<CreateFolderFormData> {
  isEdit?: boolean;
  folder?: ProjectFolderResponse;
}

export function FolderCreateEdit(props: Props) {
  const { t } = useTranslation();
  const {
    formState: { isValid },
    register,
    handleSubmit,
  } = useForm<CreateFolderFormData>({
    mode: "all",
  });

  const onSubmit: SubmitHandler<CreateFolderFormData> = (data) => {
    props.onSubmit(data);
  };

  return (
    <Dialog fullWidth maxWidth="sm" open={props.isOpen}>
      <DialogTitle component="div">
        <Typography variant="h5" color="primary.dark" sx={{ marginBottom: 2 }}>
          {props.isEdit && t("folder.createEdit.titleEdit")}
          {!props.isEdit && t("folder.createEdit.titleCreate")}
        </Typography>
        <IconButton onClick={props.onClose} sx={{ position: "absolute", top: 0, right: 0 }}>
          <CloseIcon />
        </IconButton>
        <Divider />
      </DialogTitle>
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
          defaultValue={props.isEdit ? props.folder?.projectFolderName : ""}
          label={t("folder.createEdit.name")}
          required
          inputRef={(input) => input && input.focus()}
        />
        <Typography variant="subtitle1">{t("common.note")}</Typography>
        <Typography variant="body2" sx={{ marginBottom: 1 }}>
          {t("folder.createEdit.description.description")}
        </Typography>
        <Typography variant="caption" color={"primary.main"}>
          {t("captions.note")}
        </Typography>
        <TextField
          {...register("folderDescription")}
          variant="filled"
          sx={{ width: "100%" }}
          multiline
          minRows={4}
          // defaultValue={props.folder?.projectFolderDescription}
          label={t("folder.createEdit.description.input")}
        />
      </DialogContent>
      <DialogActions>
        <Button variant="contained" color="primary" disabled={!isValid} onClick={handleSubmit(onSubmit)}>
          {props.isEdit && t("common.save")}
          {!props.isEdit && t("common.create")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
