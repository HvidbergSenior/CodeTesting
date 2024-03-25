import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import { ExtraWorkAgreementResponse } from "api/generatedApi";
import { useAbortDialog } from "components/shared/alert/abort/hook/use-abort";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { Fragment, useEffect, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { FormDialogProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { ExtraWorkAgreementFormData, useValidateExtraWorkAgreementInput } from "./hooks/use-input-validation";
import { EditExtraWorkAgreementDescription } from "./inputs/description";
import { EditExtraWorkAgreementName } from "./inputs/name";
import { EditExtraWorkAgreementNumber } from "./inputs/number";
import { EditExtraWorkAgreementSelectType } from "./inputs/select-type";
import { EditExtraWorkAgreementTimeOrPayment } from "./inputs/time-or-payment";

interface Props extends FormDialogProps<ExtraWorkAgreementFormData> {
  agreement?: ExtraWorkAgreementResponse;
  allowChanges: boolean;
}

export const UpdateExtraWorkAgreementDialog = (props: Props) => {
  const { agreement, allowChanges, onClose } = props;
  const oldAgreeement: ExtraWorkAgreementResponse | undefined = agreement ? JSON.parse(JSON.stringify(agreement)) : undefined;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const { hasValidateInputs, hasDirtyInputs } = useValidateExtraWorkAgreementInput();
  const [isInputValid, setIsInputValid] = useState(false);
  const nonFunction = () => {};
  const openAbortDialog = useAbortDialog({ closeDialog: onClose ?? nonFunction });

  const { register, setValue, getValues, handleSubmit, control, watch } = useForm<ExtraWorkAgreementFormData>({
    mode: "all",
    defaultValues: {
      extraWorkAgreementType: agreement ? agreement.extraWorkAgreementType : "CompanyHours",
      workTime: agreement ? agreement.workTime : { hours: 0, minutes: 0 },
      paymentDkr: agreement?.paymentDkr,
    },
  });

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (
        name === "extraWorkAgreementNumber" ||
        name === "name" ||
        name === "description" ||
        name === "extraWorkAgreementType" ||
        name === "paymentDkr" ||
        name === "workTime" ||
        name === "workTime.hours" ||
        name === "workTime.minutes"
      ) {
        const isValidated = hasValidateInputs(getValues);
        const isDirty = hasDirtyInputs(getValues, oldAgreeement);
        setIsInputValid(isValidated && isDirty);
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, getValues, hasValidateInputs, oldAgreeement, hasDirtyInputs, setIsInputValid]);

  const onSubmit: SubmitHandler<ExtraWorkAgreementFormData> = (data) => {
    props.onSubmit(data);
  };

  const close = () => {
    if (hasDirtyInputs(getValues, oldAgreeement)) {
      openAbortDialog();
      return;
    }
    if (onClose) {
      onClose();
    }
  };

  return (
    <Dialog
      fullWidth
      maxWidth="sm"
      PaperProps={{
        sx: {
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100´%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitleStyled
        title={allowChanges ? t("dashboard.extraWorkAgreements.update.title") : t("dashboard.extraWorkAgreements.update.disabledTitle")}
        onClose={props.onClose}
        isOpen={props.isOpen}
        handleIconClose={close}
      />
      <DialogContent>
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
          <EditExtraWorkAgreementNumber register={register} agreement={agreement} disabled={!allowChanges} />
          <EditExtraWorkAgreementName register={register} agreement={props.agreement} disabled={!allowChanges} />
          <EditExtraWorkAgreementDescription register={register} agreement={props.agreement} disabled={!allowChanges} />
          <EditExtraWorkAgreementSelectType control={control} agreement={props.agreement} disabled={!allowChanges} />
          <EditExtraWorkAgreementTimeOrPayment
            watch={watch}
            getValue={getValues}
            setValue={setValue}
            register={register}
            agreement={props.agreement}
            disabled={!allowChanges}
          />
        </Box>
      </DialogContent>
      <DialogActions>
        {allowChanges ? (
          <Fragment>
            <Button variant="outlined" onClick={close}>
              {t("common.cancel")}
            </Button>
            <Button variant="contained" color="primary" onClick={handleSubmit(onSubmit)} disabled={!isInputValid}>
              {t("common.save")}
            </Button>
          </Fragment>
        ) : (
          <Button variant="outlined" onClick={close}>
            {t("common.close")}
          </Button>
        )}
      </DialogActions>
    </Dialog>
  );
};
