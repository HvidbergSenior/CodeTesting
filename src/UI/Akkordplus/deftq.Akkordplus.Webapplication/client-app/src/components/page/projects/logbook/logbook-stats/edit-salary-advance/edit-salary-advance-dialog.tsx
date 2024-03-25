import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import FormControl from "@mui/material/FormControl";
import RadioGroup from "@mui/material/RadioGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import Radio from "@mui/material/Radio";
import Typography from "@mui/material/Typography";
import { LogBookSalaryAdvanceResponse, RegisterLogbookSalaryAdvanceRequest } from "api/generatedApi";
import type { FormDialogProps } from "shared/dialog/types";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { InputFieldCommon, InputFieldProps } from "components/shared/number-input/input-common";
import { formatNumberToPrice, parseCurrencyToFloat } from "utils/formats";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { useConfirm } from "components/shared/alert/confirm/hooks/use-confirm";

interface Props extends FormDialogProps<RegisterLogbookSalaryAdvanceRequest> {
  viewedWeek: number;
  viewedYear: number;
  salaryAdvance: LogBookSalaryAdvanceResponse;
}

export const EditSalaryAdvanceDialog = (props: Props) => {
  const { salaryAdvance, viewedWeek, viewedYear } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const [disableSave, setDisableSave] = useState(true);

  const isThisWeek =
    !!salaryAdvance.start && salaryAdvance.start.year === viewedYear && salaryAdvance.start.week === viewedWeek && salaryAdvance.role !== "Undefined";
  const oldAmount = isThisWeek ? salaryAdvance.amount ?? 0 : 0;
  const [amount, setAmount] = useState<string>(oldAmount > 0 ? formatNumberToPrice(oldAmount) : "");

  const { control, setValue, getValues, watch } = useForm<RegisterLogbookSalaryAdvanceRequest>({
    mode: "all",
    defaultValues: {
      type: isThisWeek ? (salaryAdvance.role === "Undefined" ? "Participant" : salaryAdvance.role) : "Participant",
      amount: oldAmount,
    },
  });

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "amount" || name === "type") {
        if (isThisWeek) {
          if (!value) {
            setDisableSave(true);
            return;
          }
          var disabled = true;
          if (value.amount !== salaryAdvance.amount) {
            disabled = false;
          } else if (value.type !== salaryAdvance.role) {
            disabled = false;
          }
          setDisableSave(disabled);
        } else {
          setDisableSave(!value.amount || value.amount === 0);
        }
      }
    });
    return () => subscription.unsubscribe();
  }, [watch, setDisableSave, salaryAdvance, isThisWeek]);

  const amountChanges = (value: string) => {
    setAmount(value);
    try {
      setValue("amount", parseCurrencyToFloat(value));
    } catch (error) {
      setValue("amount", 0);
    }
  };

  const amountInputProps: InputFieldProps = {
    maxValue: 1000,
    inputType: "currency",
    minValue: 0,
    value: amount,
    disableInputField: false,
    onChange: amountChanges,
  };

  const onClickSave = () => {
    if (isThisWeek && getValues("amount") === 0) {
      openConfirmRemoveZeroSetSalvaryAdvanceDialog();
      return;
    }
    openConfirmSaveSalaryAdvanceDialog();
  };

  const onSaveSalaryAdvance = () => {
    props.onSubmit(getValues());
  };

  const onRemoveSalvaryAdvance = () => {
    setValue("amount", 0);
    props.onSubmit(getValues());
  };

  const openConfirmSaveSalaryAdvanceDialog = useConfirm({
    title: t("logbook.salaryAdvance.edit.confirmation.update.title"),
    description: t("logbook.salaryAdvance.edit.confirmation.update.description"),
    submitButtonLabel: t("common.ok"),
    submit: onSaveSalaryAdvance,
  });

  const openConfirmRemoveSalvaryAdvanceDialog = useConfirm({
    title: t("logbook.salaryAdvance.edit.confirmation.remove.title"),
    description: t("logbook.salaryAdvance.edit.confirmation.remove.description"),
    submitButtonLabel: t("common.ok"),
    submit: onRemoveSalvaryAdvance,
  });

  const openConfirmRemoveZeroSetSalvaryAdvanceDialog = useConfirm({
    title: t("logbook.salaryAdvance.edit.confirmation.remove.title"),
    description: t("logbook.salaryAdvance.edit.confirmation.remove.zeroSetDescription"),
    submitButtonLabel: t("common.ok"),
    submit: onRemoveSalvaryAdvance,
  });

  const showRemoveButton = (): boolean => {
    if (!salaryAdvance.start?.year || !salaryAdvance.start?.week || !salaryAdvance.role || salaryAdvance.role === "Undefined") {
      return false;
    }
    if (viewedWeek !== salaryAdvance.start?.week || viewedYear !== salaryAdvance.start.year) {
      return false;
    }
    return true;
  };

  return (
    <Dialog
      fullWidth
      maxWidth="xs"
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "auto", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100´%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitleStyled title={t("logbook.salaryAdvance.edit.title")} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent sx={{ display: "flex", justifyContent: "space-between", flexDirection: "column", p: 0 }}>
        <Typography variant="body2" sx={{ pr: 3, pl: 3, pb: 2 }}>
          {t("logbook.salaryAdvance.edit.description")}
        </Typography>
        <Typography variant="body2" sx={{ pr: 3, pl: 3, pb: 2 }}>
          {t("logbook.salaryAdvance.edit.description2")}
        </Typography>
        <Divider variant="fullWidth" sx={{ color: "primary.dark" }}></Divider>
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2, p: 3 }}>
          <FormControl>
            <Typography variant="caption" color={"primary.main"}>
              {t("logbook.salaryAdvance.edit.type.caption")}
            </Typography>
            <Controller
              rules={{ required: true }}
              control={control}
              name="type"
              render={({ field }) => (
                <RadioGroup {...field}>
                  <FormControlLabel
                    data-testid="edit-salary-advance-type-participant"
                    value={"Participant"}
                    control={<Radio />}
                    label={t("logbook.salaryAdvance.edit.type.participant")}
                  />
                  <FormControlLabel
                    data-testid="edit-salary-advance-type-apprentice"
                    value={"Apprentice"}
                    control={<Radio />}
                    label={t("logbook.salaryAdvance.edit.type.apprentice")}
                  />
                </RadioGroup>
              )}
            />
          </FormControl>
          <Box>
            <Typography variant="caption" color={"primary.main"}>
              {t("logbook.salaryAdvance.edit.amount.caption")}
            </Typography>
            <InputFieldCommon inputFieldProps={amountInputProps} />
          </Box>
          {showRemoveButton() && <Typography variant="body2">{t("logbook.salaryAdvance.edit.footer")}</Typography>}
        </Box>
      </DialogContent>
      <DialogActions>
        <Button variant="contained" disabled={!showRemoveButton()} color="primary" onClick={openConfirmRemoveSalvaryAdvanceDialog}>
          {t("common.remove")}
        </Button>

        <Button variant="contained" color="primary" disabled={disableSave} onClick={onClickSave}>
          {t("common.save")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
