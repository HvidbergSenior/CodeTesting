import { useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";

import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import FormControlLabel from "@mui/material/FormControlLabel";
import Switch from "@mui/material/Switch";
import Typography from "@mui/material/Typography";

import { ProjectFolderResponse } from "api/generatedApi";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { FormDialogProps } from "shared/dialog/types";

export interface FolderLockFormData {
  recursive: boolean;
}

interface Props extends FormDialogProps<FolderLockFormData> {
  folder: ProjectFolderResponse;
}

export function FolderLock(props: Props) {
  const { folder } = props;
  const { t } = useTranslation();
  const baseTranslation = folder?.projectFolderLocked === "Locked" ? "folder.lock.locked" : "folder.lock.open";
  const [checked, setChecked] = useState(false);

  const { handleSubmit, setValue } = useForm<FolderLockFormData>({
    mode: "all",
  });

  const recursiveChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setValue("recursive", event.target.checked);
    setChecked(event.target.checked);
  };

  const onSubmit: SubmitHandler<FolderLockFormData> = (data) => {
    props.onSubmit(data);
  };

  return (
    <Dialog fullWidth maxWidth="xs" open={props.isOpen}>
      <DialogTitleStyled
        title={t(`${baseTranslation}.header`)}
        description={t(`${baseTranslation}.description`)}
        onClose={props.onClose}
        isOpen={props.isOpen}
        showDivider={false}
      />
      <DialogContent>
        <FormControlLabel
          sx={{ display: "flex", justifyContent: "space-between", width: "100%", ml: 0, pr: 1 }}
          labelPlacement="start"
          control={<Switch checked={checked} onChange={recursiveChange} />}
          label={
            <Typography variant="body1" color="primary.main" fontWeight="800">
              {t("folder.lock.includeSumFolders")}
            </Typography>
          }
        />
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.cancel")}
        </Button>
        <Button variant="contained" color="primary" onClick={handleSubmit(onSubmit)}>
          {t(t(`${baseTranslation}.button`))}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
