import { useCallback, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { UseFormGetValues, UseFormRegister, UseFormWatch } from "react-hook-form";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { InputContainer } from "../../../../../../../shared/styles/input-container-style";
import { CreateProjectUserFormData } from "./project-user-form-data";
import { useValidateProjectUserInputs } from "../hooks/use-input-validation";

interface Props {
  register: UseFormRegister<CreateProjectUserFormData>;
  watch: UseFormWatch<CreateProjectUserFormData>;
  getValue: UseFormGetValues<CreateProjectUserFormData>;
}

export const ProjectUserName = (props: Props) => {
  const { register, getValue, watch } = props;
  const { t } = useTranslation();
  const tRef = useRef(t);
  const { validateHasName } = useValidateProjectUserInputs(getValue);
  const [nameValidationError, setNameValidationError] = useState<string | undefined>(undefined);
  const [nameValidated, setNameValidated] = useState(true);

  const validate = useCallback(() => {
    if (validateHasName()) {
      setNameValidated(true);
      setNameValidationError(undefined);
    } else {
      setNameValidated(false);
      setNameValidationError(tRef.current("dashboard.users.create.nameRequiredError"));
    }
  }, [validateHasName, tRef, setNameValidated, setNameValidationError]);

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "name") {
        validate();
      }
    });
    return () => subscription.unsubscribe();
  }, [watch, validate]);

  return (
    <InputContainer height={85}>
      <Typography variant="caption" color={"primary.main"}>
        {t("captions.nameRequired")}
      </Typography>
      <TextField
        data-testid="create-project-users-name"
        {...register("name")}
        variant="filled"
        sx={{ width: "100%" }}
        label={t("dashboard.users.create.placeholderName")}
        inputProps={{ maxLength: 40 }}
        error={!nameValidated}
        helperText={nameValidationError}
      />
    </InputContainer>
  );
};
