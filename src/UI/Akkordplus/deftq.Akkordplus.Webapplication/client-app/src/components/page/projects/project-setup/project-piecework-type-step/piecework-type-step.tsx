import Box from "@mui/material/Box";
import FormControl from "@mui/material/FormControl";
import FormControlLabel from "@mui/material/FormControlLabel";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import Typography from "@mui/material/Typography";
import Divider from "@mui/material/Divider";
import { ProjectSetupFormData } from "../project-setup-form-data";
import { Control, Controller } from "react-hook-form";
import { useTranslation } from "react-i18next";

interface Props {
  control: Control<ProjectSetupFormData>;
}

export function ProjectPieceworkTypeSelectStep(props: Props) {
  const { control } = props;
  const { t } = useTranslation();

  return (
    <Box>
      <FormControl>
        <Controller
          rules={{ required: true }}
          control={control}
          name="pieceworkType"
          render={({ field }) => (
            <RadioGroup {...field}>
              <FormControlLabel
                data-testid="create-project-piecework-12.1a"
                value={"TwelveOneA"}
                control={<Radio />}
                sx={{ paddingBottom: 2 }}
                label={
                  <Box>
                    <Typography variant="body1" color="secondary">
                      {t("project.pieceworkTypes.TwelveOneA.header")}
                    </Typography>
                    <Typography variant="body1">{t("project.pieceworkTypes.TwelveOneA.description")}</Typography>
                  </Box>
                }
              />
              <Divider sx={{ pt: 1, mb: 1 }} />
              <FormControlLabel
                data-testid="create-project-piecework-12.1b"
                value={"TwelveOneB"}
                control={<Radio />}
                sx={{
                  paddingBottom: 2,
                  paddingTop: 2,
                }}
                label={
                  <Box>
                    <Typography variant="body1" color="secondary">
                      {t("project.pieceworkTypes.TwelveOneB.header")}
                    </Typography>
                    <Typography variant="body1">{t("project.pieceworkTypes.TwelveOneB.description")}</Typography>
                  </Box>
                }
              />
              <Divider sx={{ pt: 1, mb: 1 }} />
              <FormControlLabel
                value={"TwelveOneC"}
                control={<Radio />}
                sx={{
                  paddingBottom: 2,
                  paddingTop: 2,
                }}
                label={
                  <Box>
                    <Typography variant="body1" color="secondary">
                      {t("project.pieceworkTypes.TwelveOneC.header")}
                    </Typography>
                    <Typography variant="body1">{t("project.pieceworkTypes.TwelveOneC.description")}</Typography>
                  </Box>
                }
              />
              <Divider sx={{ pt: 1, mb: 1 }} />
              <FormControlLabel
                value={"TwelveTwo"}
                control={<Radio />}
                sx={{ paddingTop: 2 }}
                label={
                  <Box>
                    <Typography variant="body1" color="secondary">
                      {t("project.pieceworkTypes.TwelveTwo.header")}
                    </Typography>
                    <Typography variant="body1">{t("project.pieceworkTypes.TwelveTwo.description")}</Typography>
                  </Box>
                }
              />
            </RadioGroup>
          )}
        />
      </FormControl>
    </Box>
  );
}
