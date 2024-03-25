import { useTranslation } from "react-i18next";
import { useForm } from "react-hook-form";
import { useState, useEffect } from "react";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import Divider from "@mui/material/Divider";
import Box from "@mui/material/Box";
import CloseIcon from "@mui/icons-material/Close";
import { FormDialogProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { CreateProjectUserFormData } from "./inputs/project-user-form-data";
import { useValidateProjectUserInputs } from "./hooks/use-input-validation";
import { ProjectUserName } from "./inputs/project-user-name";
import { ProjectUserEmail } from "./inputs/project-user-email";
import { ProjectUserRole } from "./inputs/project-user-role";
import { ProjectUserAdr } from "./inputs/project-user-adr";
import { ProjectUserPhone } from "./inputs/project-user-phone";

interface Props extends FormDialogProps<CreateProjectUserFormData> {}

export const CreateProjectUserDialog = (props: Props) => {
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const [disableSave, setDisableSave] = useState(true);

  const { getValues, setValue, register, watch, control } = useForm<CreateProjectUserFormData>({
    mode: "all",
    defaultValues: {
      role: "ProjectParticipant",
    },
  });

  const { validateCreateParameter } = useValidateProjectUserInputs(getValues);

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "name" || name === "email" || name === "role") {
        setDisableSave(!validateCreateParameter());
      }
    });
    return () => subscription.unsubscribe();
  }, [watch, setDisableSave, validateCreateParameter]);

  const onAdd = () => {
    if (!validateCreateParameter()) {
      return;
    }
    props.onSubmit(getValues());
  };

  return (
    <Dialog
      fullWidth
      maxWidth="sm"
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "auto", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100´%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitle component="div" textAlign={"center"} p={0}>
        <Typography variant="h5" color="primary.dark">
          {t("dashboard.users.create.title")}
        </Typography>
        <IconButton onClick={props.onClose} sx={{ position: "absolute", top: 0, right: 0 }}>
          <CloseIcon />
        </IconButton>
        <Divider sx={{ marginTop: 2 }} />
      </DialogTitle>
      <DialogContent>
        <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
          <ProjectUserName getValue={getValues} register={register} watch={watch} />
          <ProjectUserEmail getValue={getValues} register={register} watch={watch} />
          <ProjectUserRole control={control} />
          <ProjectUserAdr register={register} />
          <ProjectUserPhone getValue={getValues} setValue={setValue} />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.cancel")}
        </Button>
        <Button variant="contained" color="primary" onClick={onAdd} disabled={disableSave}>
          {t("common.add")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
