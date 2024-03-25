import { useTranslation } from "react-i18next";
import { UseFormRegister } from "react-hook-form";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { ExtraWorkAgreementResponse } from "api/generatedApi";
import { ExtraWorkAgreementFormData } from "../hooks/use-input-validation";
import { InputContainer } from "shared/styles/input-container-style";

interface Props {
  agreement?: ExtraWorkAgreementResponse;
  disabled: boolean;
  register: UseFormRegister<ExtraWorkAgreementFormData>;
}

export const EditExtraWorkAgreementDescription = (props: Props) => {
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("captions.noteOptional")}
      </Typography>
      {props.disabled ? (
        <Typography variant="body1">{props.agreement?.description}</Typography>
      ) : (
        <TextField
          {...props.register("description")}
          variant="filled"
          disabled={props.disabled}
          sx={{ width: "100%" }}
          defaultValue={props.agreement?.description}
          label={t("dashboard.extraWorkAgreements.create.description.input")}
          multiline
          minRows={4}
        />
      )}
    </InputContainer>
  );
};
