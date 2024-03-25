import { useState } from "react";
import { useForm } from "react-hook-form";
import type { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { GetProjectResponse } from "api/generatedApi";
import type { FormDialogProps } from "shared/dialog/types";
import { ProjectInfoStep } from "../project-info-step/project-info-step";
import { ProjectSetupFormData } from "../project-setup-form-data";
import StepDialogTitleStyled from "components/shared/dialog/step-dialog-title-styled";
import { ProjectOrderAndPieceworkNumberStep } from "../project-order-and-piecework-number-step/project-order-and-piecework-number-step";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

interface Props extends FormDialogProps<ProjectSetupFormData> {
  project: GetProjectResponse;
}

export function EditProjectNameDialog(props: Props) {
  const { project } = props;
  const { t } = useTranslation();
  const [disableSave, setDisableSave] = useState(false);
  const { screenSize } = useScreenSize();

  const stepProjectInfo = 0;
  const stepOrderNumber = 1;
  const firstStep = stepProjectInfo;
  const lastStep = stepOrderNumber;
  const [steps] = useState(lastStep + 1);
  const [activeStep, setActiveStep] = useState(firstStep);

  const handleNext = () => {
    if (activeStep === stepProjectInfo && getValues().title === "") {
      setError("title", {});
      return;
    }
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const { register, getFieldState, getValues, setError, handleSubmit, watch } = useForm<ProjectSetupFormData>({
    mode: "all",
    defaultValues: {
      title: project.title,
      description: project.description,
      orderNumber: project.orderNumber ?? undefined,
      pieceworkNumber: project.pieceWorkNumber ?? undefined,
    },
  });

  const onSubmit: SubmitHandler<ProjectSetupFormData> = (data) => {
    setDisableSave(true);
    props.onSubmit(data);
  };

  const getHeader = () => {
    switch (activeStep) {
      case stepProjectInfo:
        return t("project.projectSetup.info.name.title");
      case stepOrderNumber:
        return t("project.projectSetup.orderAndPieceworkNumber.title");
      default:
        return undefined;
    }
  };

  const getDescription = () => {
    switch (activeStep) {
      case stepProjectInfo:
        return undefined;
      case stepOrderNumber:
        return undefined;
      default:
        return undefined;
    }
  };

  const getContent = () => {
    switch (activeStep) {
      case stepProjectInfo:
        return <ProjectInfoStep register={register} getFieldState={getFieldState} watch={watch} />;
      case stepOrderNumber:
        return <ProjectOrderAndPieceworkNumberStep register={register} />;
      default:
        return undefined;
    }
  };

  return (
    <Dialog
      open={props.isOpen}
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "550px", // mobile is ignored
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
          <Button data-testid="edit-project-name-action-btn-back" disabled={disableSave} variant="contained" onClick={handleBack}>
            {t("common.back")}
          </Button>
        )}
        {activeStep === lastStep && (
          <Button data-testid="edit-project-name-action-btn-save" disabled={disableSave} variant="contained" onClick={handleSubmit(onSubmit)}>
            {t("common.save")}
          </Button>
        )}
        {activeStep !== lastStep && (
          <Button data-testid="edit-project-name-action-btn-next" variant="contained" onClick={handleNext}>
            {t("common.next")}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
