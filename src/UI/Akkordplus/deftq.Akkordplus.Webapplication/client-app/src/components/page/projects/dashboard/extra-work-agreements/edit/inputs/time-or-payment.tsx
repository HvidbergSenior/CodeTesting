import { useTranslation } from "react-i18next";
import { useState, useEffect, Fragment } from "react";
import { UseFormSetValue, UseFormGetValues, UseFormWatch, UseFormRegister } from "react-hook-form";
import Box from "@mui/material/Box";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";
import Typography from "@mui/material/Typography";
import { ExtraWorkAgreementResponse, ExtraWorkAgreementTypeRequest } from "api/generatedApi";
import { InputContainer } from "shared/styles/input-container-style";
import { InputFieldCommon, InputFieldProps } from "components/shared/number-input/input-common";
import { formatHoursAndMinutes, formatNumberToPrice } from "utils/formats";
import { ExtraWorkAgreementFormData } from "../hooks/use-input-validation";

interface Props {
  agreement?: ExtraWorkAgreementResponse;
  setValue: UseFormSetValue<ExtraWorkAgreementFormData>;
  getValue: UseFormGetValues<ExtraWorkAgreementFormData>;
  watch: UseFormWatch<ExtraWorkAgreementFormData>;
  register: UseFormRegister<ExtraWorkAgreementFormData>;
  disabled: boolean;
}

export const EditExtraWorkAgreementTimeOrPayment = (props: Props) => {
  const { agreement, setValue, getValue, watch, register, disabled } = props;
  const { t } = useTranslation();
  type ShowElementsType = "Time" | "Payment" | "None";
  const getShowElementType = (type?: ExtraWorkAgreementTypeRequest): ShowElementsType => {
    if (!type) {
      return "None";
    }
    switch (type) {
      case "CompanyHours":
      case "CustomerHours":
        return "Time";
      case "AgreedPayment":
        return "Payment";
      case "Other":
        return "None";
    }
  };
  const [showElements, setShowElements] = useState<ShowElementsType>(getShowElementType(getValue("extraWorkAgreementType")));
  const [numberValue, setNumberValue] = useState<string>(agreement?.paymentDkr?.toString() ?? "");

  const paymentChanges = (value: string) => {
    setNumberValue(value);
    try {
      const val = parseFloat(value.replaceAll(".", "").replace(",", "."));
      setValue("paymentDkr", val);
    } catch {
      setValue("paymentDkr", undefined);
    }
  };

  const paymentInputConfig: InputFieldProps = {
    disableInputField: false,
    inputType: "currency",
    maxValue: 10000000,
    value: numberValue,
    alwaysDisableInputField: false,
    minValue: -10000000,
    onChange: paymentChanges,
  };

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "extraWorkAgreementType") {
        switch (value.extraWorkAgreementType) {
          case "CompanyHours":
          case "CustomerHours":
            if (showElements !== "Time") {
              setShowElements("Time");
            }
            break;
          case "AgreedPayment":
            if (showElements !== "Payment") {
              setShowElements("Payment");
            }
            break;
          case "Other":
            if (showElements !== "None") {
              setShowElements("None");
            }
            break;
        }
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, showElements]);

  return (
    <InputContainer sx={{ height: disabled ? 1 : 100, maxWidth: 250 }}>
      {showElements === "Payment" && (
        <Fragment>
          <Typography variant="body2">{t("captions.paymentRequired")}</Typography>
          {disabled ? (
            <Typography variant="body1">{formatNumberToPrice(agreement?.paymentDkr)}</Typography>
          ) : (
            <InputFieldCommon inputFieldProps={paymentInputConfig} />
          )}
        </Fragment>
      )}
      {showElements === "Time" && (
        <Fragment>
          <Typography variant="caption" color={"primary.main"}>
            {t("captions.timeRequired")}
          </Typography>
          {disabled ? (
            <Typography variant="body1">{formatHoursAndMinutes(agreement?.workTime?.hours, agreement?.workTime?.minutes)}</Typography>
          ) : (
            <Box sx={{ display: "flex", justifyContent: "space-between" }}>
              <FormControl sx={{ my: 1, width: "45%" }}>
                <InputLabel>{t("common.time.hours")}</InputLabel>
                <Select label="hours" defaultValue={agreement?.workTime?.hours ?? 0} {...register("workTime.hours")}>
                  {Array.from(Array(301).keys()).map((number, index) => {
                    return (
                      <MenuItem key={index} value={number}>
                        {number}
                      </MenuItem>
                    );
                  })}
                </Select>
              </FormControl>
              <FormControl sx={{ my: 1, width: "45%" }}>
                <InputLabel>{t("common.time.minutes")}</InputLabel>
                <Select label="minutes" defaultValue={agreement?.workTime?.minutes ?? 0} {...register("workTime.minutes")}>
                  <MenuItem value={0}>0</MenuItem>
                  <MenuItem value={15}>15</MenuItem>
                  <MenuItem value={30}>30</MenuItem>
                  <MenuItem value={45}>45</MenuItem>
                </Select>
              </FormControl>
            </Box>
          )}
        </Fragment>
      )}
    </InputContainer>
  );
};
