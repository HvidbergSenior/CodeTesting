import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import { UseFormRegister } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { ProjectSetupFormData } from "../project-setup-form-data";

interface Props {
  register: UseFormRegister<ProjectSetupFormData>;
}

export function ProjectOrderAndPieceworkNumberStep(props: Props) {
  const { register } = props;
  const { t } = useTranslation();

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 4 }}>
      <Box>
        <Typography variant="caption" color={"primary.main"}>
          {t("project.projectSetup.orderAndPieceworkNumber.captionOrdreNumber")}
        </Typography>
        <TextField
          data-testid="edit-project-order-and-piecework-number-ordre"
          {...register("orderNumber")}
          variant="filled"
          sx={{ width: "100%" }}
          label={t("project.projectSetup.orderAndPieceworkNumber.ordreNumber")}
          inputRef={(input) => input && input.focus()}
          inputProps={{ maxLength: 15 }}
        />
      </Box>
      <Box>
        <Typography variant="caption" color={"primary.main"}>
          {t("project.projectSetup.orderAndPieceworkNumber.captionPieceworkNumber")}
        </Typography>
        <TextField
          data-testid="edit-project-order-and-piecework-number-piecework"
          {...register("pieceworkNumber")}
          variant="filled"
          sx={{ width: "100%" }}
          label={t("project.projectSetup.orderAndPieceworkNumber.pieceworkNumber")}
          inputProps={{ maxLength: 15 }}
        />
      </Box>
    </Box>
  );
}
