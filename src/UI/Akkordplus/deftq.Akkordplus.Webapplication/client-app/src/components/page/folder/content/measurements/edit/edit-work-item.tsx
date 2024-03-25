import { useTranslation } from "react-i18next";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import CloseIcon from "@mui/icons-material/Close";
import type { WorkItemResponse } from "api/generatedApi";
import type { FormDialogProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { SelectAmountStep } from "../create/create-work-item-dialog/step-amount/select-amount";
import { FormDataWorkItemUpdate } from "../create/create-work-item-dialog/create-work-item-form-data";

interface Props extends FormDialogProps<FormDataWorkItemUpdate> {
  workItem: WorkItemResponse;
}

export function EditWorkItemDialog(props: Props) {
  const { workItem } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const [disableSave, setDisableSave] = useState(true);

  const { getValues, setValue, watch } = useForm<FormDataWorkItemUpdate>({
    mode: "all",
    defaultValues: {
      workItem: workItem,
      workitemType: workItem.workItemType,
      amount: workItem.workItemAmount,
      material: workItem.workItemType === "Material" ? { name: workItem.workItemText } : undefined,
      operation: workItem.workItemType === "Operation" ? { operationText: workItem.workItemText } : undefined,
      oldAmount: workItem.workItemAmount,
    },
  });

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "amount") {
        setDisableSave(value.amount === value.oldAmount);
      }
    });
    return () => subscription.unsubscribe();
  }, [watch, setDisableSave]);

  const getHeader = (): string => {
    return t("content.measurements.edit.selectAmountStep.header");
  };

  const getContent = () => {
    return <SelectAmountStep setValue={setValue} getValue={getValues} />;
  };

  const onSubmit = () => {
    setDisableSave(true);
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
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitle component="div" textAlign={"center"}>
        <Typography variant="h5" color="primary.dark">
          {getHeader()}
        </Typography>
        <IconButton onClick={props.onClose} sx={{ position: "absolute", top: 0, right: 0 }}>
          <CloseIcon />
        </IconButton>
      </DialogTitle>
      <DialogContent data-testid="is-the-the-container" sx={{ padding: 0 }}>
        {getContent()}
      </DialogContent>
      <DialogActions>
        <Button data-testid="edit-workitem-action-btn-save" disabled={disableSave} variant="contained" color="primary" onClick={onSubmit}>
          {t("common.saveAndClose")}
        </Button>
        <Button data-testid="edit-workitem-action-btn-close" variant="contained" onClick={props.onClose}>
          {t("common.close")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
