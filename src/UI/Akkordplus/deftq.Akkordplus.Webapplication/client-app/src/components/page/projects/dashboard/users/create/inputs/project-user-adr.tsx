import { useTranslation } from "react-i18next";
import { UseFormRegister } from "react-hook-form";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import { InputContainer } from "../../../../../../../shared/styles/input-container-style";
import { CreateProjectUserFormData } from "./project-user-form-data";

interface Props {
  register: UseFormRegister<CreateProjectUserFormData>;
}

export const ProjectUserAdr = (props: Props) => {
  const { register } = props;
  const { t } = useTranslation();

  return (
    <InputContainer>
      <Typography variant="caption" color={"primary.main"}>
        {t("dashboard.users.create.captionAdr")}
      </Typography>
      <TextField
        data-testid="create-project-users-adr"
        {...register("adr")}
        variant="filled"
        sx={{ width: "100%" }}
        label={t("dashboard.users.create.placeholderAdr")}
        inputProps={{ maxLength: 60 }}
      />
    </InputContainer>
  );
};
