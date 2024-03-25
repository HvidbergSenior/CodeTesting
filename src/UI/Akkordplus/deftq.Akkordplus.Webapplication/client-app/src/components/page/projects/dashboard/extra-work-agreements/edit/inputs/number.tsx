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

export const EditExtraWorkAgreementNumber = (props: Props) => {
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("dashboard.extraWorkAgreements.create.number.label")}
      </Typography>
      {props.disabled ? (
        <Typography variant="body1">{props.agreement?.extraWorkAgreementNumber}</Typography>
      ) : (
        <TextField
          {...props.register("extraWorkAgreementNumber")}
          disabled={props.disabled}
          variant="filled"
          sx={{ width: "50%" }}
          defaultValue={props.agreement?.extraWorkAgreementNumber}
          inputProps={{ maxLength: 20 }}
          label={t("dashboard.extraWorkAgreements.create.number.input")}
        />
      )}
    </InputContainer>
  );
};
