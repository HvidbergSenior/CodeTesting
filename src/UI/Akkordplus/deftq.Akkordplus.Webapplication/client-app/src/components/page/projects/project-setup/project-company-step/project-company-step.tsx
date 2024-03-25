import { Box, TextField, Typography } from "@mui/material";
import { UseFormGetValues, UseFormRegister, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { ProjectSetupFormData } from "../project-setup-form-data";
import { InputFieldCommon, InputFieldProps } from "components/shared/number-input/input-common";
import { useState } from "react";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

interface Props {
  register: UseFormRegister<ProjectSetupFormData>;
  getValue: UseFormGetValues<ProjectSetupFormData>;
  setValue: UseFormSetValue<ProjectSetupFormData>;
}

export function ProjectCompanyStep(props: Props) {
  const { register, getValue, setValue } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const [cvrValue, setCvrValue] = useState<string>(getValue("cvrNumber") ?? "");
  const [pValue, setPValue] = useState<string>(getValue("pNumber") ?? "");

  const cvrChanges = (value: string) => {
    setCvrValue(value);
    setValue("cvrNumber", value);
  };

  const pChanges = (value: string) => {
    setPValue(value);
    setValue("pNumber", value);
  };

  const cvrInputProps: InputFieldProps = {
    maxValue: 99999999,
    inputType: "integer",
    minValue: 0,
    value: cvrValue,
    disableInputField: false,
    onChange: cvrChanges,
  };

  const pInputProps: InputFieldProps = {
    maxValue: 9999999999,
    inputType: "integer",
    minValue: 0,
    value: pValue,
    disableInputField: false,
    onChange: pChanges,
  };

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
      <Box>
        <Typography variant="caption" color={"primary.main"}>
          {t("project.projectSetup.company.adrCaption")}
        </Typography>
        <TextField
          {...register("workplaceAdr")}
          variant="filled"
          sx={{ width: "100%" }}
          label={t("project.projectSetup.company.adr")}
          inputProps={{ maxLength: 40 }}
        />
      </Box>
      <Box>
        <Typography variant="caption" color={"primary.main"}>
          {t("project.projectSetup.company.companyCaption")}
        </Typography>
        <TextField
          {...register("companyName")}
          variant="filled"
          sx={{ width: "100%" }}
          label={t("project.projectSetup.company.company")}
          inputProps={{ maxLength: 40 }}
        />
      </Box>
      <Box sx={{ display: "flex", flexDirection: screenSize === ScreenSizeEnum.Mobile ? "column" : "row", gap: 3 }}>
        <Box sx={{ width: "50%" }}>
          <Typography variant="caption" color={"primary.main"}>
            {t("project.projectSetup.company.cvrNumberCaption")}
          </Typography>
          <InputFieldCommon inputFieldProps={cvrInputProps} />
        </Box>
        <Box sx={{ width: "50%" }}>
          <Typography variant="caption" color={"primary.main"}>
            {t("project.projectSetup.company.pNumberCaption")}
          </Typography>
          <InputFieldCommon inputFieldProps={pInputProps} />
        </Box>
      </Box>
    </Box>
  );
}
