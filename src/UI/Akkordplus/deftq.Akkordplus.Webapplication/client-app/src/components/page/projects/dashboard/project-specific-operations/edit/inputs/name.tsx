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

export const ProjectSpecificOperationName = (props: Props) => {
  const { operation, register } = props;
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("captions.nameRequired")}
      </Typography>
      <TextField
        {...register("newName")}
        variant="filled"
        sx={{ width: "100%" }}
        defaultValue={operation?.name}
        inputProps={{ maxLength: 40 }}
        label={t("dashboard.projectSpecificOperations.create.input.name.placeholder")}
      />
    </InputContainer>
  );
};
