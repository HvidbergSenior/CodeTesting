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
import Box from "@mui/material/Box";
import CloseIcon from "@mui/icons-material/Close";
import { ExtendedSupplementOperationResponse } from "components/page/folder/content/measurements/create/hooks/use-map-work-item";
import { FormDialogProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { SelectAmountStep } from "components/page/folder/content/measurements/create/create-work-item-dialog/step-amount/select-amount";
import { SelectMountingCodeStep } from "components/page/folder/content/measurements/create/create-work-item-dialog/step-mounting-code/select-mounting-code-step";
import { SelectSupplementOperationsStep } from "components/page/folder/content/measurements/create/create-work-item-dialog/step-supplement-operations/select-supplement-operations-step";
import { SelectSupplementsStep } from "components/page/folder/content/measurements/create/create-work-item-dialog/step-supplements/select-supplements-step";
import { WorkitemTypeStep } from "components/page/folder/content/measurements/create/create-work-item-dialog/step-workitem-type/workitem-type-step";
import { useOfflineStorage } from "utils/offline-storage/use-offline-storage";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { SelectNoteStep } from "./note/select-note-step";
import { FormDataWorkItemDraft } from "components/page/folder/content/measurements/create/create-work-item-dialog/create-work-item-form-data";

interface Props extends FormDialogProps<FormDataWorkItemDraft> {}

export function CreateDraftWorkItemDialog(props: Props) {
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const isOnline = useOnlineStatus();
  const { getOfflineProjectId, getOfflineFavorites } = useOfflineStorage();

  const stepWorkitemType = 0;
  const stepMaterialAmount = 1;
  const stepMaterialMountingCode = 2;
  const stepMaterialSupplementOperations = 3;
  const stepMaterialSupplements = 4;
  const stepMaterialNote = 5;

  const stepOperationAmount = 1;
  const stepOperationSupplements = 2;
  const stepOperationNote = 3;

  const [disableSave, setDisableSave] = useState(false);
  const [activeStep, setActiveStep] = useState(0);
  const [validationError, setValidationError] = useState<string | undefined>(undefined);

  const { getValues, setValue, watch, control } = useForm<FormDataWorkItemDraft>({
    mode: "all",
    defaultValues: {
      workitemType: "Material",
      favorites: getOfflineFavorites(getOfflineProjectId()),
    },
  });

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "mountingCode") {
        const selectedMountingCode = value.mountingCode;
        const oldUsedMountingCodeForSupplementOperations = value.supplementOperationsUsedMountingCode;
        if (!selectedMountingCode && oldUsedMountingCodeForSupplementOperations) {
          setValue("supplementOperationsUsedMountingCode", undefined);
          setValue("supplementOperations", undefined);
        }
        if (selectedMountingCode && selectedMountingCode.mountingCode !== oldUsedMountingCodeForSupplementOperations) {
          setValue("supplementOperationsUsedMountingCode", selectedMountingCode.mountingCode);
          setValue(
            "supplementOperations",
            selectedMountingCode?.supplementOperations
              ? (selectedMountingCode?.supplementOperations as ExtendedSupplementOperationResponse[])
              : undefined
          );
        }
      }
    });
    return () => subscription.unsubscribe();
  }, [watch, setValue]);

  const isMaterialWorkItem = (): boolean => {
    return getValues("workitemType") === "Material";
  };

  const getHeader = (): string => {
    switch (activeStep) {
      case stepWorkitemType:
        return t("content.measurements.create.header");
    }
    return isMaterialWorkItem() ? getMaterialHeader() : getOperationHeader();
  };

  const getMaterialHeader = (): string => {
    switch (activeStep) {
      case stepMaterialAmount:
        return t("content.measurements.create.selectAmountStep.header");
      case stepMaterialMountingCode:
        return t("content.measurements.create.selectMountingCodesStep.header");
      case stepMaterialSupplementOperations:
        return t("content.measurements.create.selectSupplementOperationStep.header");
      case stepMaterialSupplements:
        return t("content.measurements.create.selectSupplementsStep.header");
      case stepMaterialNote:
        return t("project.offline.create.note.header");
    }
    return t("content.measurements.create.header");
  };

  const getOperationHeader = (): string => {
    switch (activeStep) {
      case stepOperationAmount:
        return t("content.measurements.create.selectAmountStep.header");
      case stepOperationSupplements:
        return t("content.measurements.create.selectSupplementsStep.header");
      case stepOperationNote:
        return t("project.offline.create.note.header");
    }
    return t("content.measurements.create.header");
  };

  const clearValidateError = () => {
    setValidationError(undefined);
  };

  const getContent = () => {
    if (activeStep === stepWorkitemType) {
      return <WorkitemTypeStep setValue={setValue} getValue={getValues} clearValidateError={clearValidateError} validationError={validationError} />;
    }
    if (activeStep > stepWorkitemType && activeStep <= stepMaterialNote) {
      return isMaterialWorkItem() ? getMaterialContent() : getOperationContent();
    }
    return <Box>{`${t("content.measurements.create.unknownStep")} - Step[${activeStep}]`}</Box>;
  };

  const getMaterialContent = () => {
    switch (activeStep) {
      case stepMaterialAmount:
        return <SelectAmountStep setValue={setValue} getValue={getValues} />;
      case stepMaterialMountingCode:
        return <SelectMountingCodeStep setValue={setValue} getValue={getValues} />;
      case stepMaterialSupplementOperations:
        return <SelectSupplementOperationsStep setValue={setValue} getValue={getValues} control={control} />;
      case stepMaterialSupplements:
        return <SelectSupplementsStep setValue={setValue} getValue={getValues} control={control} />;
      case stepMaterialNote:
        return <SelectNoteStep setValue={setValue} getValue={getValues} />;
      default:
        return <Box>{`${t("content.measurements.create.unknownStep")} - Material Step[${activeStep}]`}</Box>;
    }
  };

  const getOperationContent = () => {
    switch (activeStep) {
      case stepOperationAmount:
        return <SelectAmountStep setValue={setValue} getValue={getValues} />;
      case stepOperationSupplements:
        return <SelectSupplementsStep setValue={setValue} getValue={getValues} control={control} />;
      case stepOperationNote:
        return <SelectNoteStep setValue={setValue} getValue={getValues} />;
      default:
        return <Box>{`${t("content.measurements.create.unknownStep")} - Operation Step[${activeStep}]`}</Box>;
    }
  };

  const validateStep = () => {
    if (!isOnline && !getValues("material") && !getValues("operation")) {
      setValidationError(t("content.measurements.create.workitemTypeStep.missingOfflineFavoritSelection"));
      return false;
    }
    return isMaterialWorkItem() ? validateMaterialStep() : validateOperationStep();
  };

  const validateMaterialStep = () => {
    if (activeStep === stepWorkitemType) {
      if (!getValues("material")) {
        setValidationError(t("content.measurements.create.selectMaterialStep.missingMaterial"));
        return false;
      }
    }
    return true;
  };

  const validateOperationStep = () => {
    if (activeStep === stepWorkitemType) {
      if (!getValues("operation")) {
        setValidationError(t("content.measurements.create.selectOperationStep.missingOperation"));
        return false;
      }
    }
    return true;
  };

  const handleNext = () => {
    if (!validateStep()) {
      return;
    }

    setValidationError(undefined);
    setActiveStep((prevActiveStep) => {
      if (isMaterialWorkItem()) {
        if (activeStep === stepMaterialMountingCode) {
          const mountingCode = getValues("mountingCode.mountingCode");
          const supplements = getValues("supplementOperations");
          if (!mountingCode || mountingCode <= 0) {
            return stepMaterialSupplements;
          }
          if (!supplements || supplements.length <= 0) {
            return stepMaterialSupplements;
          }
        }
      }
      return prevActiveStep + 1;
    });
  };

  const handleBack = () => {
    setValidationError(undefined);
    setActiveStep((prevActiveStep) => {
      if (isMaterialWorkItem()) {
        if (activeStep === stepMaterialSupplements) {
          const mountingCode = getValues("mountingCode.mountingCode");
          if (!mountingCode || mountingCode <= 0) {
            return stepMaterialMountingCode;
          }
        }
      }
      return prevActiveStep - 1;
    });
  };

  const onSubmit = () => {
    setDisableSave(true);
    props.onSubmit(getValues());
  };

  const showBack = (): boolean => {
    if (isMaterialWorkItem()) {
      return activeStep > stepMaterialAmount;
    }
    return activeStep > stepOperationAmount;
  };

  const showSave = (): boolean => {
    if (isMaterialWorkItem()) {
      return activeStep === stepMaterialNote;
    }
    return activeStep === stepOperationNote;
  };

  const showSaveAndClose = (): boolean => {
    if (isMaterialWorkItem()) {
      return activeStep >= stepMaterialAmount && activeStep < stepMaterialNote;
    }
    return activeStep >= stepOperationAmount && activeStep < stepOperationNote;
  };

  const showNext = (): boolean => {
    if (activeStep === stepWorkitemType && !isOnline && getValues("favorites")?.length <= 0) {
      return false;
    }
    if (isMaterialWorkItem()) {
      return activeStep < stepMaterialNote;
    }
    return activeStep < stepOperationNote;
  };

  const showClose = (): boolean => {
    if (activeStep === stepWorkitemType && !isOnline && getValues("favorites")?.length <= 0) {
      return true;
    }
    return false;
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
        {showBack() && (
          <Button data-testid="create-draft-work-item-action-btn-back" disabled={disableSave} variant="outlined" onClick={handleBack}>
            {t("common.back")}
          </Button>
        )}
        {showSave() && (
          <Button data-testid="create-draft-work-item-action-btn-save" disabled={disableSave} variant="contained" color="primary" onClick={onSubmit}>
            {t("common.save")}
          </Button>
        )}
        {showSaveAndClose() && (
          <Button data-testid="create-draft-work-item-action-btn-save" disabled={disableSave} variant="contained" color="primary" onClick={onSubmit}>
            {t("common.saveAndClose")}
          </Button>
        )}
        {showNext() && (
          <Button data-testid="create-draft-work-item-action-btn-next" disabled={disableSave} variant="contained" onClick={handleNext}>
            {t("common.next")}
          </Button>
        )}
        {showClose() && (
          <Button data-testid="create-draft-work-item-action-btn-close" variant="contained" onClick={props.onClose}>
            {t("common.close")}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
