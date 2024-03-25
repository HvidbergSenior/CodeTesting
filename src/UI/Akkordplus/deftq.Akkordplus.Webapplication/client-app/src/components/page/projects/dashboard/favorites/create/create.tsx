import { useState } from "react";
import { useTranslation } from "react-i18next";
import { useForm } from "react-hook-form";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import Divider from "@mui/material/Divider";
import CloseIcon from "@mui/icons-material/Close";
import { FavoritesResponse, FoundMaterial, FoundOperation, WorkItemResponse, WorkItemType } from "api/generatedApi";
import { FormDialogProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { WorkitemTypeStep } from "components/page/folder/content/measurements/create/create-work-item-dialog/step-workitem-type/workitem-type-step";

export interface CreateFavoriteFormData {
  workitemType: WorkItemType;
  material: FoundMaterial;
  operation: FoundOperation;
  workItem: WorkItemResponse | undefined;
  favorites: FavoritesResponse[];
}

interface Props extends FormDialogProps<CreateFavoriteFormData> {}

export const CreateFavoriteDialog = (props: Props) => {
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const [validationError, setValidationError] = useState<string | undefined>(undefined);

  const { getValues, setValue } = useForm<CreateFavoriteFormData>({
    mode: "all",
    defaultValues: {
      workitemType: "Material",
      favorites: [],
    },
  });

  const clearValidationError = () => {
    setValidationError(undefined);
  };

  const onAdd = () => {
    if (getValues("workitemType") === "Material" && !getValues("material")) {
      setValidationError(t("content.measurements.create.selectMaterialStep.missingMaterial"));
      return;
    }
    if (getValues("workitemType") === "Operation" && !getValues("operation")) {
      setValidationError(t("content.measurements.create.selectOperationStep.missingOperation"));
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
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "750px", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100´%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitle component="div" textAlign={"center"} p={0}>
        <Typography variant="h5" color="primary.dark">
          {t("dashboard.favorits.create.title")}
        </Typography>
        <IconButton onClick={props.onClose} sx={{ position: "absolute", top: 0, right: 0 }}>
          <CloseIcon />
        </IconButton>
        <Divider sx={{ marginTop: 2 }} />
      </DialogTitle>
      <DialogContent sx={{ padding: 0 }}>
        <WorkitemTypeStep setValue={setValue} getValue={getValues} validationError={validationError} clearValidateError={clearValidationError} />
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.cancel")}
        </Button>
        <Button variant="contained" color="primary" onClick={onAdd}>
          {t("common.add")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
