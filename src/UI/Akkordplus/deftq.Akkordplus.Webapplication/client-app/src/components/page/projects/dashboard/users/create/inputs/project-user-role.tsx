import { useTranslation } from "react-i18next";
import { Control, Controller } from "react-hook-form";
import Typography from "@mui/material/Typography";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import { InputContainer } from "../../../../../../../shared/styles/input-container-style";
import { CreateProjectUserFormData } from "./project-user-form-data";

interface Props {
  control: Control<CreateProjectUserFormData>;
}

export const ProjectUserRole = (props: Props) => {
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("dashboard.users.create.captionRole")}
      </Typography>
      <FormControl>
        <Controller
          rules={{ required: true }}
          control={props.control}
          name="role"
          render={({ field }) => (
            <RadioGroup {...field}>
              <FormControlLabel
                data-testid="create-project-users-role-participant"
                value={"ProjectParticipant"}
                control={<Radio />}
                label={t("dashboard.users.roles.participant")}
              />
              <FormControlLabel
                data-testid="create-project-users-role-manager"
                value={"ProjectManager"}
                control={<Radio />}
                label={t("dashboard.users.roles.manager")}
              />
            </RadioGroup>
          )}
        />
      </FormControl>
    </InputContainer>
  );
};
