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
import StepDialogTitleStyled from "components/shared/dialog/step-dialog-title-styled";
import { GetProjectResponse } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { ProjectCompanyStep } from "../project-company-step/project-company-step";

interface Props extends FormDialogProps<ProjectSetupFormData> {
  project: GetProjectResponse;
}

export function EditProjectCompanyAndWorkplaceDialog(props: Props) {
  const { project } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();

  const [disableSave, setDisableSave] = useState(false);

  const stepProjectCompany = 0;
  const firstStep = stepProjectCompany;
  const lastStep = stepProjectCompany;
  const [steps] = useState(lastStep + 1);
  const [activeStep, setActiveStep] = useState(firstStep);

  const { getValues, handleSubmit, setValue, register } = useForm<ProjectSetupFormData>({
    mode: "all",
    defaultValues: {
      companyName: project.companyName ?? undefined,
      workplaceAdr: project.companyAddress ?? undefined,
      cvrNumber: project.companyCvrNo ?? undefined,
      pNumber: project.companyPNo ?? undefined,
    },
  });

  const getHeader = () => {
    switch (activeStep) {
      case stepProjectCompany:
        return t("project.projectSetup.company.title");
      default:
        return undefined;
    }
  };

  const getDescription = () => {
    switch (activeStep) {
      case stepProjectCompany:
        return undefined;
      default:
        return undefined;
    }
  };

  const getContent = () => {
    switch (activeStep) {
      case stepProjectCompany:
        return <ProjectCompanyStep register={register} getValue={getValues} setValue={setValue} />;
      default:
        return undefined;
    }
  };

  const handleNext = () => {
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  const handleBack = () => {
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
