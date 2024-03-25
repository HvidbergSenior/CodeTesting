import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import type { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import type { FormDialogProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { ProjectSpecificOperationFormData, ProjectSpecificOperationTimeType } from "./hooks/project-specific-operation-formdata";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { ProjectSpecificOperationExtraWorkAgreementNumber } from "./inputs/extra-work-agreement-number";
import { ProjectSpecificOperationName } from "./inputs/name";
import { ProjectSpecificOperationDescription } from "./inputs/description";
import { ProjectSpecicifOperationTime } from "./inputs/time";
import { ProjectSpecificOperationSelectType } from "./inputs/select-type";
import { useValidateProjectSpecificOperationInput } from "./hooks/use-input-validation";

interface Props extends FormDialogProps<ProjectSpecificOperationFormData> {
  projectId: string;
}

export function CreateProjectSpecificOperationDialog(props: Props) {
  const { t } = useTranslation();
  const [disableSave, setDisableSave] = useState(true);
  const [showTimeType, setShowTimeType] = useState<ProjectSpecificOperationTimeType>("workingTime");
  const { hasValidCreateInputs } = useValidateProjectSpecificOperationInput();
  const { screenSize } = useScreenSize();

  const { watch, getValues, setValue, register, control, handleSubmit } = useForm<ProjectSpecificOperationFormData>({
    mode: "all",
    defaultValues: {
      timeType: showTimeType,
    },
  });

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "timeType") {
        setShowTimeType(value.timeType ?? "operationTime");
      }
      if (name === "newName" || name === "newOperationTimeMs" || name === "newWorkingTimeMs") {
        var res = hasValidCreateInputs(getValues);
        setDisableSave(!res);
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, getValues, hasValidCreateInputs, setDisableSave]);

  const onChangeTime = (type: ProjectSpecificOperationTimeType, timeMs: number) => {
    if (type === "operationTime") {
      setValue("newOperationTimeMs", timeMs);
    } else {
      setValue("newWorkingTimeMs", timeMs);
    }
  };

  const onSubmit: SubmitHandler<ProjectSpecificOperationFormData> = (data) => {
    setDisableSave(true);
    props.onSubmit(data);
  };

  return (
    <Dialog
      open={props.isOpen}
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "850px", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100%-64px)",
        },
      }}
    >
      <DialogTitleStyled
        title={t("dashboard.projectSpecificOperations.create.title")}
        description={t("dashboard.projectSpecificOperations.create.description")}
        onClose={props.onClose}
        isOpen={props.isOpen}
      />
      <DialogContent>
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
          <ProjectSpecificOperationExtraWorkAgreementNumber register={register} />
          <ProjectSpecificOperationName register={register} />
          <ProjectSpecificOperationDescription register={register} />
          <ProjectSpecificOperationSelectType control={control} getValue={getValues} />
          {showTimeType === "operationTime" && (
            <ProjectSpecicifOperationTime
              timeMs={getValues("newOperationTimeMs")}
              disabled={false}
              onChange={(timeMs) => onChangeTime("operationTime", timeMs)}
            />
          )}
          {showTimeType === "workingTime" && (
            <ProjectSpecicifOperationTime
              timeMs={getValues("newWorkingTimeMs")}
              disabled={false}
              onChange={(timeMs) => onChangeTime("workingTime", timeMs)}
            />
          )}
        </Box>
      </DialogContent>
      <DialogActions>
        <Button
          data-testid="create-project-specific-operation-action-btn-create"
          variant="contained"
          color="primary"
          disabled={disableSave}
          onClick={handleSubmit(onSubmit)}
        >
          {t("common.create")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
