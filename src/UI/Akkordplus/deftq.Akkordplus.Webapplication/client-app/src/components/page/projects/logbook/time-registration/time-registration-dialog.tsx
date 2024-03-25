import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import { GetProjectLogBookDayResponse, RegisterProjectLogBookDay } from "api/generatedApi";
import { SubmitHandler, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import type { FormDialogProps } from "shared/dialog/types";
import { formatTimestamp } from "utils/formats";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { Box } from "@mui/material";

interface Props extends FormDialogProps<RegisterProjectLogBookDay> {
  day: GetProjectLogBookDayResponse | undefined;
}

export const TimeRegistrationDialog = (props: Props) => {
  const { t } = useTranslation();
  const { day } = props;

  const {
    formState: { isValid },
    register,
    handleSubmit,
  } = useForm<RegisterProjectLogBookDay>({
    mode: "all",
  });

  const onSubmit: SubmitHandler<RegisterProjectLogBookDay> = (data) => {
    props.onSubmit(data);
  };

  return (
    <Dialog maxWidth="sm" open={props.isOpen}>
      <DialogTitleStyled
        title={t("logbook.timeRegistration.title", { date: formatTimestamp(day?.date, false, "full") })}
        onClose={props.onClose}
        isOpen={props.isOpen}
      />
      <DialogContent sx={{ display: "flex", flexDirection: "column" }}>
        <Typography variant="caption" color={"primary.main"}>
          {t("captions.time")}
        </Typography>
        <Box sx={{ display: "flex", gap: 2, pt: 1 }}>
          <FormControl sx={{ my: 1, width: "30%" }}>
            <InputLabel>{t("common.time.hours")}</InputLabel>
            <Select label="hours" defaultValue={day?.time?.hours ?? 0} {...register("hours")}>
              {Array.from(Array(25).keys()).map((number, index) => {
                return (
                  <MenuItem key={index} value={number}>
                    {number}
                  </MenuItem>
                );
              })}
            </Select>
          </FormControl>
          <FormControl sx={{ my: 1, width: "30%" }}>
            <InputLabel>{t("common.time.minutes")}</InputLabel>
            <Select label="minutes" defaultValue={day?.time?.minutes ?? 0} {...register("minutes")}>
              <MenuItem value={0}>0</MenuItem>
              <MenuItem value={15}>15</MenuItem>
              <MenuItem value={30}>30</MenuItem>
              <MenuItem value={45}>45</MenuItem>
            </Select>
          </FormControl>
        </Box>
      </DialogContent>
      <DialogActions>
        <Button variant="contained" color="primary" disabled={!isValid} onClick={handleSubmit(onSubmit)}>
          {t("common.save")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
