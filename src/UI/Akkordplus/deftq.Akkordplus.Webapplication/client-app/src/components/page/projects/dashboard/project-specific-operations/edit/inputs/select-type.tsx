import { useTranslation } from "react-i18next";
import { Control, Controller, UseFormGetValues } from "react-hook-form";
import Typography from "@mui/material/Typography";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import { GetProjectSpecificOperationResponse } from "api/generatedApi";
import { InputContainer } from "shared/styles/input-container-style";
import { ProjectSpecificOperationFormData } from "../hooks/project-specific-operation-formdata";

interface Props {
  control: Control<ProjectSpecificOperationFormData>;
  getValue: UseFormGetValues<ProjectSpecificOperationFormData>;
  operation?: GetProjectSpecificOperationResponse;
}

export const ProjectSpecificOperationSelectType = (props: Props) => {
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("dashboard.projectSpecificOperations.create.input.type.label")}
      </Typography>

      <FormControl>
        <Controller
          rules={{ required: true }}
          control={props.control}
          name="timeType"
          render={({ field }) => (
            <RadioGroup {...field}>
              <FormControlLabel
                data-testid="create-project-specific-operation-working-time"
                value={"workingTime"}
                control={<Radio />}
                label={t("dashboard.projectSpecificOperations.types.workingTime")}
              />
              <FormControlLabel
                data-testid="create-project-specific-operation-operation-time"
                value={"operationTime"}
                control={<Radio />}
                label={t("dashboard.projectSpecificOperations.types.operationTime")}
              />
            </RadioGroup>
          )}
        />
      </FormControl>
    </InputContainer>
  );
};
