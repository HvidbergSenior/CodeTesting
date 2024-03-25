import { useTranslation } from "react-i18next";
import { UseFormRegister } from "react-hook-form";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { ExtraWorkAgreementResponse } from "api/generatedApi";
import { InputContainer } from "shared/styles/input-container-style";
import { ExtraWorkAgreementFormData } from "../hooks/use-input-validation";

interface Props {
  agreement?: ExtraWorkAgreementResponse;
  disabled: boolean;
  register: UseFormRegister<ExtraWorkAgreementFormData>;
}

export const EditExtraWorkAgreementName = (props: Props) => {
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("captions.nameRequired")}
      </Typography>
      {props.disabled ? (
        <Typography variant="body1">{props.agreement?.name}</Typography>
      ) : (
        <TextField
          {...props.register("name")}
          variant="filled"
          disabled={props.disabled}
          sx={{ width: "100%" }}
          defaultValue={props.agreement?.name}
          inputProps={{ maxLength: 40 }}
          label={t("dashboard.extraWorkAgreements.create.name.input")}
        />
      )}
    </InputContainer>
  );
};
