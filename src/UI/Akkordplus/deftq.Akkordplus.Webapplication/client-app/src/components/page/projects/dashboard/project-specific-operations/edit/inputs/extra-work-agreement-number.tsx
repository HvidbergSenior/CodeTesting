import { useTranslation } from "react-i18next";
import { UseFormRegister } from "react-hook-form";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { GetProjectSpecificOperationResponse } from "api/generatedApi";
import { InputContainer } from "shared/styles/input-container-style";
import { ProjectSpecificOperationFormData } from "../hooks/project-specific-operation-formdata";

interface Props {
  operation?: GetProjectSpecificOperationResponse;
  register: UseFormRegister<ProjectSpecificOperationFormData>;
}

export const ProjectSpecificOperationExtraWorkAgreementNumber = (props: Props) => {
  const { operation, register } = props;
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("dashboard.projectSpecificOperations.create.input.number.label")}
      </Typography>
      <TextField
        {...register("newExtraWorkAgreementNumber")}
        variant="filled"
        sx={{ width: "50%" }}
        defaultValue={operation?.extraWorkAgreementNumber}
        inputProps={{ maxLength: 20 }}
        label={t("dashboard.projectSpecificOperations.create.input.number.placeholder")}
      />
    </InputContainer>
  );
};
