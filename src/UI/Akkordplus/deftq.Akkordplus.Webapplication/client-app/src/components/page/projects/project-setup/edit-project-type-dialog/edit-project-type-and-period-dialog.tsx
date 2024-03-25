import { useState } from "react";
import { useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { SubmitHandler } from "react-hook-form";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { FormDialogProps } from "shared/dialog/types";
import { ProjectSetupFormData } from "../project-setup-form-data";
import { ProjectPeriodSelectorStep } from "../project-period-selector-step/project-period-selector-step";
import StepDialogTitleStyled from "components/shared/dialog/step-dialog-title-styled";
import { GetProjectResponse } from "api/generatedApi";
import { ProjectPieceworkTypeSelectStep } from "../project-piecework-type-step/piecework-type-step";
import { ProjectLumpSumStep } from "../project-lumpsum-step/project-lumpsum-step";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { getDateFromDateOnly } from "utils/formats";

interface Props extends FormDialogProps<ProjectSetupFormData> {
  project: GetProjectResponse;
}

export function EditProjectTypeAndPeriodDialog(props: Props) {
  const { project } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();

  const [disableSave, setDisableSave] = useState(false);

  const stepProjectPieceworkTypeSelect = 0;
  const stepProjectLumpSum = 1;
  const stepProjectPeriodSelect = 2;
  const firstStep = stepProjectPieceworkTypeSelect;
  const lastStep = stepProjectPeriodSelect;
  const [steps] = useState(lastStep + 1);
  const [activeStep, setActiveStep] = useState(firstStep);

  const { getValues, handleSubmit, setValue, control } = useForm<ProjectSetupFormData>({
    mode: "all",
    defaultValues: {
      pieceworkType: project.pieceworkType ?? "TwelveOneA",
      lumpSum: project.lumpSumPaymentDkr ?? 0,
      projectDateStart: getDateFromDateOnly(project.startDate ?? undefined),
      projectDateEnd: getDateFromDateOnly(project.endDate ?? undefined),
    },
  });

  const getHeader = () => {
    switch (activeStep) {
      case stepProjectPieceworkTypeSelect:
        return t("project.projectSetup.pieceworkType.title");
      case stepProjectLumpSum:
        return t("project.projectSetup.lumpSum.title");
      case stepProjectPeriodSelect:
        return t("project.projectSetup.period.title");
    }
  };

  const getDescription = () => {
    switch (activeStep) {
      case stepProjectPieceworkTypeSelect:
        return t("project.projectSetup.pieceworkType.description");
      case stepProjectLumpSum:
        return undefined;
      case stepProjectPeriodSelect:
        return undefined;
      default:
        return undefined;
    }
  };

  const getContent = () => {
    switch (activeStep) {
      case stepProjectPieceworkTypeSelect:
        return <ProjectPieceworkTypeSelectStep control={control} />;
      case stepProjectLumpSum:
        return <ProjectLumpSumStep getValue={getValues} setValue={setValue} />;
      case stepProjectPeriodSelect:
        return <ProjectPeriodSelectorStep getValue={getValues} setValue={setValue} />;
      default:
        return undefined;
    }
  };

  const handleNext = () => {
    if (activeStep === stepProjectPieceworkTypeSelect && getValues("pieceworkType") !== "TwelveTwo") {
      setActiveStep(stepProjectPeriodSelect);
      return;
    }
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  const handleBack = () => {
    if (activeStep === stepProjectPeriodSelect && getValues("pieceworkType") !== "TwelveTwo") {
      setActiveStep(stepProjectPieceworkTypeSelect);
      return;
    }
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const onSubmit: SubmitHandler<ProjectSetupFormData> = (data) => {
    setDisableSave(true);
    props.onSubmit(data);
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
          <Button data-testid="edit-project-type-action-btn-back" disabled={disableSave} variant="contained" onClick={handleBack}>
            {t("common.back")}
          </Button>
        )}
        {activeStep === lastStep && (
          <Button data-testid="edit-project-type-action-btn-save" disabled={disableSave} variant="contained" onClick={handleSubmit(onSubmit)}>
            {t("common.save")}
          </Button>
        )}
        {activeStep !== lastStep && (
          <Button data-testid="edit-project-type-action-btn-next" variant="contained" onClick={handleNext}>
            {t("common.next")}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
