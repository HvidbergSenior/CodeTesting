import { useTranslation } from "react-i18next";
import { useState } from "react";
import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import Typography from "@mui/material/Typography";
import { InputContainer } from "../../../../../../../shared/styles/input-container-style";
import { CreateProjectUserFormData } from "./project-user-form-data";
import { InputFieldCommon, InputFieldProps } from "components/shared/number-input/input-common";
import { useValidateProjectUserInputs } from "../hooks/use-input-validation";

interface Props {
  getValue: UseFormGetValues<CreateProjectUserFormData>;
  setValue: UseFormSetValue<CreateProjectUserFormData>;
}

export const ProjectUserPhone = (props: Props) => {
  const { getValue, setValue } = props;
  const { t } = useTranslation();
  const { validateHasValidPhone } = useValidateProjectUserInputs(getValue);
  const [validPhone, setValidPhone] = useState(true);
  const [phoneValue, setPhoneValue] = useState<string>(getValue("phone") ?? "");

  const phoneChanges = (value: string) => {
    setPhoneValue(value);
    if (!validateHasValidPhone(value)) {
      setValidPhone(false);
      return;
    }
    setValidPhone(true);
    setValue("phone", value);
  };

  const phoneInputProps: InputFieldProps = {
    maxValue: 99999999,
    inputType: "integer",
    minValue: 0,
    value: phoneValue,
    disableInputField: false,
    onChange: phoneChanges,
  };

  return (
    <InputContainer height={100}>
      <Typography variant="caption" color={"primary.main"}>
        {t("dashboard.users.create.captionPhone")}
      </Typography>
      <InputFieldCommon inputFieldProps={phoneInputProps} />
      {!validPhone && (
        <Typography variant="caption" paddingLeft={2} color={"error.main"}>
          {t("dashboard.users.create.phoneInvalidError")}
        </Typography>
      )}
    </InputContainer>
  );
};
