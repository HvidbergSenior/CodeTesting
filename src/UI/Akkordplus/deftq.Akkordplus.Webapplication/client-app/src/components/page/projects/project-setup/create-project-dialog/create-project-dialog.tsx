import { useState } from "react";
import { useForm } from "react-hook-form";
import type { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import type { FormDialogProps } from "shared/dialog/types";
import { ProjectInfoStep } from "../project-info-step/project-info-step";
import { ProjectPieceworkTypeSelectStep } from "../project-piecework-type-step/piecework-type-step";
import { ProjectSummaryStep } from "../project-summary-step/project-summary-step";
import { ProjectSetupFormData } from "../project-setup-form-data";
import StepDialogTitleStyled from "components/shared/dialog/step-dialog-title-styled";
import { ProjectLumpSumStep } from "../project-lumpsum-step/project-lumpsum-step";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

interface Props extends FormDialogProps<ProjectSetupFormData> {}

export function CreateProjectDialog(props: Props) {
  const { t } = useTranslation();
  const [disableSave, setDisableSave] = useState(false);
  const { screenSize } = useScreenSize();

  const stepProjectInfo = 0;
  const stepProjectPieceworkTypeSelect = 1;
  const stepProjectLumpSum = 2;
  const stepProjectSummary = 3;
  const firstStep = stepProjectInfo;
  const lastStep = stepProjectSummary;
  const [steps] = useState(lastStep + 1);
  const [activeStep, setActiveStep] = useState(firstStep);

  const handleNext = () => {
    if (activeStep === stepProjectInfo && getValues().title === "") {
      setError("title", {});
      return;
    }
    if (activeStep === stepProjectPieceworkTypeSelect && getValues("pieceworkType") !== "TwelveTwo") {
      setActiveStep(stepProjectSummary);
      return;
    }
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  const handleBack = () => {
    if (activeStep === stepProjectSummary && getValues("pieceworkType") !== "TwelveTwo") {
      setActiveStep(stepProjectPieceworkTypeSelect);
      return;
    }
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const {
    formState: { isValid },
    register,
    getFieldState,
    getValues,
    setError,
    handleSubmit,
    watch,
    setValue,
    control,
  } = useForm<ProjectSetupFormData>({
    mode: "all",
    defaultValues: {
      title: "",
      description: "",
      importedFile: undefined,
      pieceworkType: "TwelveOneA",
      pieceworkAgreement: undefined,
      companyName: "",
      participants: [],
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
      case stepProjectPieceworkTypeSelect:
        return t("project.projectSetup.pieceworkType.title");
      case stepProjectLumpSum:
        return t("project.projectSetup.lumpSum.title");
      case stepProjectSummary:
        return t("project.projectSetup.summary.title");
    }
  };

  const getDescription = () => {
    switch (activeStep) {
      case stepProjectInfo:
        return undefined;
      case stepProjectPieceworkTypeSelect:
        return t("project.projectSetup.pieceworkType.description");
      case stepProjectLumpSum:
        return undefined;
      case stepProjectSummary:
        return undefined;
      default:
        return undefined;
    }
  };

  const getContent = () => {
    switch (activeStep) {
      case stepProjectInfo:
        return <ProjectInfoStep register={register} getFieldState={getFieldState} watch={watch} />;
      case stepProjectPieceworkTypeSelect:
        return <ProjectPieceworkTypeSelectStep control={control} />;
      case stepProjectLumpSum:
        return <ProjectLumpSumStep getValue={getValues} setValue={setValue} />;
      case stepProjectSummary:
        return <ProjectSummaryStep data={getValues()} />;
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
          <Button data-testid="create-project-action-btn-back" disabled={disableSave} variant="contained" onClick={handleBack}>
            {t("common.back")}
          </Button>
        )}
        {activeStep === lastStep && (
          <Button
            data-testid="create-project-action-btn-create"
            variant="contained"
            color="primary"
            disabled={!isValid || disableSave}
            onClick={handleSubmit(onSubmit)}
          >
            {t("common.create")}
          </Button>
        )}
        {activeStep !== lastStep && (
          <Button data-testid="create-project-action-btn-next" variant="contained" onClick={handleNext}>
            {t("common.next")}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
}
