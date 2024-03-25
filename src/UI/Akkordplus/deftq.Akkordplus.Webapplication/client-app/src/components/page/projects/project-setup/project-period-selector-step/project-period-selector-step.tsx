import { UseFormGetValues, UseFormSetValue } from "react-hook-form";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import { ProjectSetupFormData } from "../project-setup-form-data";
import { DateSelector } from "components/shared/date-selector/date-selector";

interface Props {
  getValue: UseFormGetValues<ProjectSetupFormData>;
  setValue: UseFormSetValue<ProjectSetupFormData>;
}

export function ProjectPeriodSelectorStep(props: Props) {
  const { getValue, setValue } = props;
  const { t } = useTranslation();

  return (
    <Box sx={{ display: "flex", flexDirection: "column" }}>
      <Typography variant="caption" color={"primary.main"} sx={{ pb: 2 }}>
        {t("project.projectSetup.period.from")}
      </Typography>
      <DateSelector defaultDate={getValue("projectDateStart")} onChangeProps={(value) => setValue("projectDateStart", value)} />
      <Typography variant="caption" color={"primary.main"} sx={{ pt: 4, pb: 2 }}>
        {t("project.projectSetup.period.to")}
      </Typography>
      <DateSelector defaultDate={getValue("projectDateEnd")} onChangeProps={(value) => setValue("projectDateEnd", value)} />
    </Box>
  );
}
