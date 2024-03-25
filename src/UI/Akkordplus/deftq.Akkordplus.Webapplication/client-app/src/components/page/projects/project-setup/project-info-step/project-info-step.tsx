import { Box, FormHelperText, TextField, Typography } from "@mui/material";
import { UseFormGetFieldState, UseFormRegister, UseFormWatch } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { ProjectSetupFormData } from "../project-setup-form-data";

interface Props {
  register: UseFormRegister<ProjectSetupFormData>;
  getFieldState: UseFormGetFieldState<ProjectSetupFormData>;
  watch: UseFormWatch<ProjectSetupFormData>;
}

export function ProjectInfoStep(props: Props) {
  const { register, getFieldState, watch } = props;
  const { t } = useTranslation();
  const watchTitle = watch("title");

  return (
    <Box>
      <Typography variant="caption" color={"primary.main"}>
        {t("captions.projectNameRequired")}
      </Typography>
      <TextField
        data-testid="create-project-title"
        {...register("title", { required: true })}
        variant="filled"
        sx={{ width: "100%" }}
        label={t("project.projectSetup.info.name.input")}
        required
        inputRef={(input) => input && input.focus()}
        inputProps={{ maxLength: 40 }}
        error={Boolean(getFieldState("title").error)}
      />
      <Box sx={{ display: "flex", justifyContent: "space-between" }}>
        <FormHelperText variant="filled" error>
          {getFieldState("title").error ? t("project.projectSetup.info.name.error") : " "}
        </FormHelperText>
        <FormHelperText variant="filled">{watchTitle ? watchTitle.length : 0}/40</FormHelperText>
      </Box>
      <Box sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
        <Typography variant="caption" color={"primary.main"} sx={{ textAlign: "left", pt: 3 }}>
          {t("project.projectSetup.info.description.caption")}
        </Typography>
        <Typography variant="caption" color={"primary.main"} sx={{ textAlign: "left" }}>
          {t("project.projectSetup.info.description.description")}
        </Typography>
        <TextField
          data-testid="create-project-description"
          {...register("description")}
          label={t("project.projectSetup.info.description.input")}
          multiline
          sx={{ width: "100%" }}
          minRows={4}
          variant="filled"
        />
      </Box>
    </Box>
  );
}
