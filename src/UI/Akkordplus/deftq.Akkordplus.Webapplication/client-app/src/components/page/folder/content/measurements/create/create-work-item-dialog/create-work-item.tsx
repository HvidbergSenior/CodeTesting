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
import type { FavoritesResponse, GetProjectResponse, ProjectFolderResponse } from "api/generatedApi";
import type { FormDialogProps } from "shared/dialog/types";
import { SelectMountingCodeStep } from "./step-mounting-code/select-mounting-code-step";
import { SelectSupplementOperationsStep } from "./step-supplement-operations/select-supplement-operations-step";
import { CalculationForPreviewStep } from "./step-calculate-work-item-preview/calculate-work-item-preview-step";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { SelectSupplementsStep } from "./step-supplements/select-supplements-step";
import { ExtendedSupplementOperationResponse } from "../hooks/use-map-work-item";
import { WorkitemTypeStep } from "./step-workitem-type/workitem-type-step";
import { SelectAmountStep } from "./step-amount/select-amount";
import { WorkItemInfoStep } from "./step-info/info-step";
import { FormDataWorkItem } from "./create-work-item-form-data";

interface Props extends FormDialogProps<FormDataWorkItem> {
  project?: GetProjectResponse;
  folder?: ProjectFolderResponse;
  favorites: FavoritesResponse[];
}

export function CreateWorkItemDialog(props: Props) {
  const stepWorkitemType = 0;
  const stepMaterialAmount = 1;
  const stepMaterialMountingCode = 2;
  const stepMaterialSupplementOperations = 3;
  const stepMaterialSupplements = 4;
  const stepMaterialCalculatedWorkItemPreview = 5;
  const stepShowInfo = 10;
  const lastStep = stepShowInfo;

  const stepOperationAmount = 1;
  const stepOperationSupplements = 2;
  const stepOperationCalculatedWorkItemPreview = 3;

  const { project, folder, favorites } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const [disableSave, setDisableSave] = useState(false);
  const [activeStep, setActiveStep] = useState(0);
  const [validationError, setValidationError] = useState<string | undefined>(undefined);

  const { getValues, setValue, watch, control } = useForm<FormDataWorkItem>({
    mode: "all",
    defaultValues: {
      workitemType: "Material",
      workItem: undefined,
      favorites: favorites,
    },
  });

  const isMaterialWorkItem = (): boolean => {
    return getValues("workitemType") === "Material";
  };

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

  const getHeader = (): string => {
    switch (activeStep) {
      case stepWorkitemType:
        return t("content.measurements.create.header");
      case stepShowInfo:
        return t("content.measurements.info.headerFlow");
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
      case stepMaterialCalculatedWorkItemPreview:
        return t("content.measurements.create.header");
    }
    return t("content.measurements.create.header");
  };

  const getOperationHeader = (): string => {
    switch (activeStep) {
      case stepOperationAmount:
        return t("content.measurements.create.selectAmountStep.header");
      case stepOperationSupplements:
        return t("content.measurements.create.selectSupplementsStep.header");
      case stepOperationCalculatedWorkItemPreview:
        return t("content.measurements.create.header");
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
    if (activeStep > stepWorkitemType && activeStep < stepShowInfo) {
      return isMaterialWorkItem() ? getMaterialContent() : getOperationContent();
    }
    if (activeStep === stepShowInfo) {
      return <WorkItemInfoStep getValue={getValues} />;
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
      case stepMaterialCalculatedWorkItemPreview:
        return (
          <CalculationForPreviewStep
            getValue={getValues}
            setValue={setValue}
            onPreviewCalculatedProps={handlePreviewCalculated}
            projectId={project?.id ?? ""}
            folderId={folder?.projectFolderId ?? ""}
            isMaterialWorkItem={isMaterialWorkItem()}
          />
        );
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
      case stepOperationCalculatedWorkItemPreview:
        return (
          <CalculationForPreviewStep
            getValue={getValues}
            setValue={setValue}
            onPreviewCalculatedProps={handlePreviewCalculated}
            projectId={project?.id ?? ""}
            folderId={folder?.projectFolderId ?? ""}
            isMaterialWorkItem={isMaterialWorkItem()}
          />
        );
      default:
        return <Box>{`${t("content.measurements.create.unknownStep")} - Operation Step[${activeStep}]`}</Box>;
    }
  };

  const validateStep = () => {
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
          const suplements = getValues("supplementOperations");
          if (!mountingCode || mountingCode <= 0) {
            return stepMaterialSupplements;
          }
          if (!suplements || suplements.length <= 0) {
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
      if (prevActiveStep === stepShowInfo) {
        return isMaterialWorkItem() ? stepMaterialSupplements : stepOperationSupplements;
      }
      return prevActiveStep - 1;
    });
  };

  const handlePreviewCalculated = () => {
    if (isMaterialWorkItem() && activeStep === stepMaterialCalculatedWorkItemPreview) {
      setActiveStep(stepShowInfo);
    }
    if (getValues("workitemType") === "Operation" && activeStep === stepOperationCalculatedWorkItemPreview) {
      setActiveStep(stepShowInfo);
    }
  };

  const onSubmit = () => {
    setDisableSave(true);
    props.onSubmit(getValues());
  };

  const showBack = () => {
    if (isMaterialWorkItem()) {
      return activeStep > stepMaterialAmount;
    }
    return activeStep > stepOperationAmount;
  };

  const showSave = () => {
    return activeStep === stepShowInfo;
  };

  const showSaveAndClose = () => {
    if (isMaterialWorkItem()) {
      return activeStep >= stepMaterialAmount && activeStep < stepMaterialCalculatedWorkItemPreview;
    }
    return activeStep >= stepOperationAmount && activeStep < stepMaterialCalculatedWorkItemPreview;
  };

  const showNext = () => {
    return activeStep !== stepMaterialCalculatedWorkItemPreview && activeStep !== lastStep && activeStep !== stepShowInfo;
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
          <Button data-testid="create-workitem-action-btn-back" disabled={disableSave} variant="outlined" onClick={handleBack}>
            {t("common.back")}
          </Button>
        )}
        {showSave() && (
          <Button data-testid="create-workitem-action-btn-save" disabled={disableSave} variant="contained" color="primary" onClick={onSubmit}>
            {t("common.save")}
          </Button>
        )}
        {showSaveAndClose() && (
          <Button data-testid="create-workitem-action-btn-save" disabled={disableSave} variant="contained" color="primary" onClick={onSubmit}>
            {t("common.saveAndClose")}
          </Button>
        )}
        {showNext() && (
          <Button data-testid="create-workitem-action-btn-next" disabled={disableSave} variant="contained" onClick={handleNext}>
            {t("common.next")}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
