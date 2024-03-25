import { useTranslation } from "react-i18next";
import { SubmitHandler, useForm } from "react-hook-form";
import Box from "@mui/material/Box";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import Divider from "@mui/material/Divider";
import CloseIcon from "@mui/icons-material/Close";
import { ExtraWorkAgreementResponse } from "api/generatedApi";
import { FormDialogProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { EditExtraWorkAgreementName } from "./inputs/name";
import { EditExtraWorkAgreementDescription } from "./inputs/description";
import { EditExtraWorkAgreementSelectType } from "./inputs/select-type";
import { EditExtraWorkAgreementTimeOrPayment } from "./inputs/time-or-payment";
import { EditExtraWorkAgreementNumber } from "./inputs/number";
import { useEffect, useState } from "react";
import { ExtraWorkAgreementFormData, useValidateExtraWorkAgreementInput } from "./hooks/use-input-validation";

interface Props extends FormDialogProps<ExtraWorkAgreementFormData> {
  agreement?: ExtraWorkAgreementResponse;
}

export const CreateExtraWorkAgreementDialog = (props: Props) => {
  const { agreement } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const { hasValidateInputs } = useValidateExtraWorkAgreementInput();
  const [isInputValid, setIsInputValid] = useState(false);

  const { register, setValue, getValues, handleSubmit, control, watch } = useForm<ExtraWorkAgreementFormData>({
    mode: "all",
    defaultValues: {
      extraWorkAgreementType: "CompanyHours",
      workTime: { hours: 0, minutes: 0 },
    },
  });

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (
        name === "extraWorkAgreementType" ||
        name === "name" ||
        name === "paymentDkr" ||
        name === "workTime" ||
        name === "workTime.hours" ||
        name === "workTime.minutes"
      ) {
        const res = hasValidateInputs(getValues);
        setIsInputValid(res);
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, getValues, hasValidateInputs, setIsInputValid]);

  const onSubmit: SubmitHandler<ExtraWorkAgreementFormData> = (data) => {
    props.onSubmit(data);
  };

  const close = () => {
    if (props?.onClose) {
      props.onClose();
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
      <DialogTitle component="div" textAlign={"center"} p={0}>
        <Typography variant="h5" color="primary.dark">
          {t("dashboard.extraWorkAgreements.create.title")}
        </Typography>
        <IconButton onClick={close} sx={{ position: "absolute", top: 0, right: 0 }}>
          <CloseIcon />
        </IconButton>
        <Divider sx={{ marginTop: 2 }} />
      </DialogTitle>
      <DialogContent>
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
          <EditExtraWorkAgreementNumber register={register} agreement={agreement} disabled={false} />
          <EditExtraWorkAgreementName register={register} agreement={props.agreement} disabled={false} />
          <EditExtraWorkAgreementDescription register={register} agreement={props.agreement} disabled={false} />
          <EditExtraWorkAgreementSelectType control={control} agreement={props.agreement} disabled={false} />
          <EditExtraWorkAgreementTimeOrPayment
            watch={watch}
            getValue={getValues}
            setValue={setValue}
            register={register}
            agreement={props.agreement}
            disabled={false}
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={close}>
          {t("common.cancel")}
        </Button>
        <Button variant="contained" color="primary" onClick={handleSubmit(onSubmit)} disabled={!isInputValid}>
          {t("common.create")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
