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

export const ProjectSpecificOperationDescription = (props: Props) => {
  const { operation, register } = props;
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("captions.noteOptional")}
      </Typography>
      <TextField
        {...register("newDescription")}
        variant="filled"
        sx={{ width: "100%" }}
        defaultValue={operation?.description}
        label={t("dashboard.projectSpecificOperations.create.input.description.placeholder")}
        multiline
        minRows={4}
      />
    </InputContainer>
  );
};
