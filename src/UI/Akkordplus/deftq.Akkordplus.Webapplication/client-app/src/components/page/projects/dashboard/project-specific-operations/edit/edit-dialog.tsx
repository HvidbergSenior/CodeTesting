import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import type { SubmitHandler } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { GetProjectSpecificOperationResponse } from "api/generatedApi";
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
import { useAbortDialog } from "components/shared/alert/abort/hook/use-abort";

interface Props extends FormDialogProps<ProjectSpecificOperationFormData> {
  operation: GetProjectSpecificOperationResponse;
}

export function EditProjectSpecificOperationDialog(props: Props) {
  const { operation } = props;
  const { t } = useTranslation();
  const [disableSave, setDisableSave] = useState(true);
  const [showTimeType, setShowTimeType] = useState<ProjectSpecificOperationTimeType>(
    !!operation.workingTimeMs && operation.workingTimeMs > 0 ? "workingTime" : "operationTime"
  );
  const { hasValidUpdateInputs, hasDirtyUpdateInputs } = useValidateProjectSpecificOperationInput();
  const { screenSize } = useScreenSize();
  const nonFunction = () => {};
  const openAbortDialog = useAbortDialog({ closeDialog: props.onClose ?? nonFunction });

  const { watch, getValues, setValue, register, control, handleSubmit } = useForm<ProjectSpecificOperationFormData>({
    mode: "all",
    defaultValues: {
      timeType: showTimeType,
      extraWorkAgreementNumber: operation.extraWorkAgreementNumber ?? undefined,
      newExtraWorkAgreementNumber: operation.extraWorkAgreementNumber ?? undefined,
      name: operation.name ?? undefined,
      newName: operation.name ?? undefined,
      description: operation.description ?? undefined,
      newDescription: operation.description ?? undefined,
      operationTimeMs: operation.operationTimeMs,
      newOperationTimeMs: operation.operationTimeMs,
      workingTimeMs: operation.workingTimeMs,
      newWorkingTimeMs: operation.workingTimeMs,
    },
  });

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "timeType") {
        setShowTimeType(value.timeType ?? "operationTime");
      }

      if (
        name === "newExtraWorkAgreementNumber" ||
        name === "newName" ||
        name === "newDescription" ||
        name === "timeType" ||
        name === "newOperationTimeMs" ||
        name === "newWorkingTimeMs"
      ) {
        const res = hasValidUpdateInputs(getValues);
        setDisableSave(!res);
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, getValues, hasValidUpdateInputs, setDisableSave]);

  const onChangeTime = (type: ProjectSpecificOperationTimeType, timeMs: number) => {
    if (type === "operationTime") {
      setValue("newOperationTimeMs", timeMs);
    } else {
      setValue("newWorkingTimeMs", timeMs);
    }
  };

  const onClose = () => {
    if (!hasDirtyUpdateInputs(getValues) && props.onClose) {
      props.onClose();
      return;
    }
    openAbortDialog();
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
        title={t("dashboard.projectSpecificOperations.update.edit.title")}
        description={t("dashboard.projectSpecificOperations.update.edit.description")}
        onClose={onClose}
        isOpen={props.isOpen}
      />
      <DialogContent>
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
          <ProjectSpecificOperationExtraWorkAgreementNumber register={register} operation={operation} />
          <ProjectSpecificOperationName register={register} operation={operation} />
          <ProjectSpecificOperationDescription register={register} operation={operation} />
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
          data-testid="update-project-specific-operation-action-btn-update"
          variant="contained"
          color="primary"
          disabled={disableSave}
          onClick={handleSubmit(onSubmit)}
        >
          {t("common.update")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
