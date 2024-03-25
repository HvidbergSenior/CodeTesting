import { useEffect, useRef, useState } from "react";
import { UseFormGetValues, UseFormSetValue, UseFormWatch } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { DateSelector } from "components/shared/date-selector/date-selector";
import { CompensationPaymentFormData } from "./compensation-payment-form-data";
import { InputFieldCommon, InputFieldProps } from "components/shared/number-input/input-common";
import { formatNumberToPrice, parseCurrencyToFloat } from "utils/formats";

interface Props {
  getValue: UseFormGetValues<CompensationPaymentFormData>;
  setValue: UseFormSetValue<CompensationPaymentFormData>;
  watch: UseFormWatch<CompensationPaymentFormData>;
  onStepIsValid: (isValid: boolean) => void;
}

export function CompensationPaymentPeriodAndAmountSelectorStep(props: Props) {
  const { getValue, setValue, watch, onStepIsValid } = props;
  const { t } = useTranslation();
  const tRef = useRef(t);
  const oldAmount = getValue("amount");
  const [amount, setAmount] = useState<string>(oldAmount ? formatNumberToPrice(oldAmount) : "");
  const [periodError, setPeriodError] = useState<string>("");

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

  useEffect(() => {
    const isPeriodValid = (): boolean => {
      const start = getValue("periodDateStart");
      const end = getValue("periodDateEnd");
      if (!start || !end) {
        setPeriodError("");
        return false;
      }
      if (start > end) {
        setPeriodError(tRef.current("dashboard.compensationPayments.create.step1PeriodAndAmount.errorPeriodEndBeforeStart"));
        return false;
      }
      setPeriodError("");
      return true;
    };

    const isAmountValid = (): boolean => {
      const amount = getValue("amount");
      return !!amount && amount > 0;
    };

    const subscription = watch((value, { name, type }) => {
      if (name === "periodDateStart" || name === "periodDateEnd" || name === "amount") {
        console.log(name, value);
        onStepIsValid(isPeriodValid() && isAmountValid());
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, getValue, setPeriodError, tRef, onStepIsValid]);

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
      <Typography variant="caption" color={"primary.main"}>
        {t("dashboard.compensationPayments.create.step1PeriodAndAmount.captionAmount")}
      </Typography>
      <InputFieldCommon inputFieldProps={amountInputProps} />
      <Typography variant="body2" sx={{ pt: 2 }}>
        {t("dashboard.compensationPayments.create.step1PeriodAndAmount.captionPeriod")}
      </Typography>
      <Box sx={{ display: "flex", flexDirection: "column", height: 105 }}>
        <Typography variant="caption" color={"primary.main"} sx={{ pb: 2 }}>
          {t("dashboard.compensationPayments.create.step1PeriodAndAmount.captionPeriodStart")}
        </Typography>
        <DateSelector defaultDate={getValue("periodDateStart")} onChangeProps={(value) => setValue("periodDateStart", value)} />
      </Box>
      <Box sx={{ display: "flex", flexDirection: "column", height: 105 }}>
        <Typography variant="caption" color={"primary.main"} sx={{ pb: 2 }}>
          {t("dashboard.compensationPayments.create.step1PeriodAndAmount.captionPeriodEnd")}
        </Typography>
        <DateSelector defaultDate={getValue("periodDateEnd")} onChangeProps={(value) => setValue("periodDateEnd", value)} />
      </Box>
      <Box>
        <Typography variant="caption" color={"error"}>
          {periodError}
        </Typography>
      </Box>
    </Box>
  );
}
