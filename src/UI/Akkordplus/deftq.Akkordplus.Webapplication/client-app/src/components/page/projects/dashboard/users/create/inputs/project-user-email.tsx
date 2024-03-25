import { UseFormGetValues, UseFormRegister, UseFormWatch } from "react-hook-form";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { InputContainer } from "../../../../../../../shared/styles/input-container-style";
import { CreateProjectUserFormData } from "./project-user-form-data";
import { useCallback, useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useValidateProjectUserInputs } from "../hooks/use-input-validation";

interface Props {
  register: UseFormRegister<CreateProjectUserFormData>;
  watch: UseFormWatch<CreateProjectUserFormData>;
  getValue: UseFormGetValues<CreateProjectUserFormData>;
}

export const ProjectUserEmail = (props: Props) => {
  const { register, getValue, watch } = props;
  const { t } = useTranslation();
  const tRef = useRef(t);
  const { validateHasEmail, validateHasValidEmail } = useValidateProjectUserInputs(getValue);
  const [nameValidationError, setNameValidationError] = useState<string | undefined>(undefined);
  const [nameValidated, setNameValidated] = useState(true);

  const validate = useCallback(() => {
    if (!validateHasEmail()) {
      setNameValidated(false);
      setNameValidationError(tRef.current("dashboard.users.create.emailRequiredError"));
    } else if (!validateHasValidEmail()) {
      setNameValidated(false);
      setNameValidationError(tRef.current("dashboard.users.create.emailInvalidError"));
    } else {
      setNameValidated(true);
      setNameValidationError(undefined);
    }
  }, [validateHasEmail, validateHasValidEmail, tRef, setNameValidated, setNameValidationError]);

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "email") {
        validate();
      }
    });
    return () => subscription.unsubscribe();
  }, [watch, validate]);

  return (
    <InputContainer height={85}>
      <Typography variant="caption" color={"primary.main"}>
        {t("dashboard.users.create.captionEmail")}
      </Typography>
      <TextField
        data-testid="create-project-users-email"
        {...register("email")}
        variant="filled"
        sx={{ width: "100%" }}
        label={t("dashboard.users.create.placeholderEmail")}
        inputProps={{ maxLength: 60 }}
        error={!nameValidated}
        helperText={nameValidationError}
      />
    </InputContainer>
  );
};
