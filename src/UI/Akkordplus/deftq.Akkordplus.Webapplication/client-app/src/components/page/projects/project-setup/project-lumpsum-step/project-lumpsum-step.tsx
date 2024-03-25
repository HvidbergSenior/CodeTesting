import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { ProjectSetupFormData } from "../project-setup-form-data";
import { InputFieldCommon, InputFieldProps } from "components/shared/number-input/input-common";
import { useState } from "react";
import { formatNumberToPrice, parseCurrencyToFloat } from "utils/formats";

interface Props {
  getValue: UseFormGetValues<ProjectSetupFormData>;
  setValue: UseFormSetValue<ProjectSetupFormData>;
}

export function ProjectLumpSumStep(props: Props) {
  const { getValue, setValue } = props;
  const { t } = useTranslation();
  const [sumValue, setSumValue] = useState<string>(formatNumberToPrice(getValue("lumpSum") ?? 0));

  const sumChanges = (value: string) => {
    setSumValue(value);
    try {
      const val = parseCurrencyToFloat(value);
      setValue("lumpSum", val);
    } catch {
      setValue("lumpSum", undefined);
    }
  };
  const inputProps: InputFieldProps = {
    value: sumValue,
    inputType: "currency",
    minValue: 0,
    disableInputField: false,
    alwaysDisableInputField: false,
    maxValue: 100000000000,
    onChange: sumChanges,
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column" }}>
      <Typography width={"200px"} pb={1} variant="caption" color={"primary.main"}>
        {t("project.projectSetup.lumpSum.inputTitle")}
      </Typography>
      <InputFieldCommon inputFieldProps={inputProps} />
    </Box>
  );
}
