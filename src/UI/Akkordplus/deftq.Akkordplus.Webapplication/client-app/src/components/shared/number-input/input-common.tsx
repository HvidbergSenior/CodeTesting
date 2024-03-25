import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import InputBase from "@mui/material/InputBase";
import React, { Dispatch, SetStateAction } from "react";
import { useTranslation } from "react-i18next";
import { formatStringOfNumbers } from "utils/formats";
import { useInputValidation } from "../validation/input-validation";
import { costumPalette } from "theme/palette";

export type InputFieldProps = {
  value: string;
  disableInputField: boolean;
  inputType: "currency" | "percent" | "integer";
  maxValue: number;
  minValue?: number;
  dispatchChange?: Dispatch<SetStateAction<string>>;
  onChange?: (value: string) => void;
  alwaysDisableInputField?: boolean;
};

interface Props {
  inputFieldProps: InputFieldProps;
}

export const InputFieldCommon = ({ inputFieldProps }: Props) => {
  const backgroundColor = inputFieldProps.alwaysDisableInputField || inputFieldProps.disableInputField ? costumPalette.white : costumPalette.gray;
  const pointerEvents: string = inputFieldProps.disableInputField ? "none" : "auto";
  const { t } = useTranslation();
  const { validateSum, validateOnChangeInputPercent, validateInteger } = useInputValidation();
  const textAlign = inputFieldProps.inputType === "currency" ? "right" : "center";
  const opacity: string = inputFieldProps.disableInputField ? "0.5" : "1";

  const getWidth = (): string[] => {
    if (inputFieldProps.maxValue < 101) {
      return ["50px", "120px"];
    }

    if (inputFieldProps.maxValue < 100000000001) {
      return ["140px", "210px"];
    }

    return ["160px", "160px"];
  };

  const onChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    let hasCorrectInput = true;
    switch (inputFieldProps.inputType) {
      case "currency":
        hasCorrectInput = e.target.value === "" || validateSum(e.target.value, inputFieldProps.maxValue, inputFieldProps.minValue);
        break;
      case "percent":
        hasCorrectInput = e.target.value === "" || validateOnChangeInputPercent(e.target.value);
        break;
      case "integer":
        hasCorrectInput = e.target.value === "" || validateInteger(e.target.value, inputFieldProps.minValue, inputFieldProps.maxValue);
        break;
    }

    if (!hasCorrectInput) {
      return;
    }

    if (inputFieldProps.inputType === "integer") {
      sentChanges(e.target.value);
      return;
    }

    if (inputFieldProps.minValue && inputFieldProps.minValue < 0 && e.target.value === "-") {
      sentChanges(e.target.value);
    } else if (
      e.target.value.charAt(e.target.value.length - 1) !== "," &&
      e.target.value.charAt(e.target.value.length - 1) !== "0" &&
      e.target.value !== ""
    ) {
      sentChanges(formatStringOfNumbers(e.target.value));
    } else {
      sentChanges(e.target.value);
    }
  };

  const sentChanges = (value: string) => {
    if (inputFieldProps.dispatchChange) {
      inputFieldProps.dispatchChange(value);
    }
    if (inputFieldProps.onChange) {
      inputFieldProps.onChange(value);
    }
  };

  return (
    <Box
      display={"flex"}
      width={getWidth()[1]}
      alignItems={"center"}
      sx={{ backgroundColor: backgroundColor, borderRadius: "10px", opacity: opacity }}
      justifyContent={"center"}
      padding={2}
    >
      <InputBase
        sx={{ pointerEvents: pointerEvents, opacity: opacity, pt: 1, pr: 1, "& .MuiInputBase-input": { textAlign: textAlign, width: getWidth()[0] } }}
        placeholder={inputFieldProps.inputType === "integer" ? "" : "0,00"}
        value={inputFieldProps.value}
        onChange={(e) => onChange(e)}
      />

      {inputFieldProps.inputType === "currency" && (
        <Typography sx={{ pt: "6px", fontSize: "16px" }} variant="subtitle1">
          {t("common.currency")}
        </Typography>
      )}
      {inputFieldProps.inputType === "percent" && (
        <Typography sx={{ pt: "6px", pr: "1px", fontSize: "16px" }} variant="overline">
          {t("common.percent")}
        </Typography>
      )}
    </Box>
  );
};
