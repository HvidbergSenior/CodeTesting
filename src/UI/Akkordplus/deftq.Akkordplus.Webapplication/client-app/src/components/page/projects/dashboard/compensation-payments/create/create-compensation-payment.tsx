import { useState } from "react";
import { useForm } from "react-hook-form";
import type { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import type { FormDialogProps } from "shared/dialog/types";
import StepDialogTitleStyled from "components/shared/dialog/step-dialog-title-styled";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { CompensationPaymentFormData } from "./compensation-payment-form-data";
import { CompensationPaymentPeriodAndAmountSelectorStep } from "./period-and-amount-step";
import { CompensationPaymentUsersSelectorStep } from "./select-participants-step";

interface Props extends FormDialogProps<CompensationPaymentFormData> {
  projectId: string;
}

export function CreateCompensationPayment(props: Props) {
  const { t } = useTranslation();
  const [disableSave, setDisableSave] = useState(true);
  const { screenSize } = useScreenSize();

  const stepPeriodAndAmount = 0;
  const stepUsers = 1;

  const firstStep = stepPeriodAndAmount;
  const lastStep = stepUsers;
  const [steps] = useState(lastStep + 1);
  const [activeStep, setActiveStep] = useState(firstStep);

  const onStepIsValid = (isValid: boolean) => {
    setDisableSave(!isValid);
  };

  const handleNext = () => {
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
    setDisableSave(true);
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const { getValues, setValue, handleSubmit, watch } = useForm<CompensationPaymentFormData>({
    mode: "all",
    defaultValues: {},
  });

  const onSubmit: SubmitHandler<CompensationPaymentFormData> = (data) => {
    setDisableSave(true);
    props.onSubmit(data);
  };

  const getHeader = () => {
    switch (activeStep) {
      case stepPeriodAndAmount:
        return t("dashboard.compensationPayments.create.step1PeriodAndAmount.title");
      case stepUsers:
        return t("dashboard.compensationPayments.create.step2Users.title");
      default:
        return undefined;
    }
  };

  const getDescription = () => {
    switch (activeStep) {
      case stepPeriodAndAmount:
        return undefined;
      case stepUsers:
        return undefined;
      default:
        return undefined;
    }
  };

  const getContent = () => {
    switch (activeStep) {
      case stepPeriodAndAmount:
        return (
          <CompensationPaymentPeriodAndAmountSelectorStep getValue={getValues} setValue={setValue} watch={watch} onStepIsValid={onStepIsValid} />
        );
      case stepUsers:
        return (
          <CompensationPaymentUsersSelectorStep
            getValue={getValues}
            setValue={setValue}
            watch={watch}
            onStepIsValid={onStepIsValid}
            projectId={props.projectId}
          />
        );
      default:
        return undefined;
    }
  };

  return (
    <Dialog
      open={props.isOpen}
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "750px", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100%-64px)",
        },
      }}
    >
      <StepDialogTitleStyled
        steps={steps}
        activeStep={activeStep}
        title={getHeader()}
        description={getDescription()}
        onClose={props.onClose}
        isOpen={props.isOpen}
      />
      <DialogContent>{getContent()}</DialogContent>
      <DialogActions>
        {activeStep !== firstStep && (
          <Button data-testid="create-compensation-payment-action-btn-back" variant="contained" disabled={disableSave} onClick={handleBack}>
            {t("common.back")}
          </Button>
        )}
        {activeStep === lastStep && (
          <Button
            data-testid="create-compensation-payment-action-btn-create"
            variant="contained"
            color="primary"
            disabled={disableSave}
            onClick={handleSubmit(onSubmit)}
          >
            {t("common.create")}
          </Button>
        )}
        {activeStep !== lastStep && (
          <Button data-testid="create-compensation-payment-action-btn-next" variant="contained" disabled={disableSave} onClick={handleNext}>
            {t("common.next")}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
