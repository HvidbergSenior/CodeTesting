import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useForm } from "react-hook-form";
import Box from "@mui/material/Box";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";
import Typography from "@mui/material/Typography";
import { InputContainer } from "shared/styles/input-container-style";
import { convertMilliSecondsFromHms, convertMilliSecondsToHms, getHmsFromMilliSeconds } from "utils/formats";

interface Props {
  timeMs?: number;
  disabled: boolean;
  onChange: (timeMs: number) => void;
}

export interface ProjectSpecificOperationTimeFormData {
  hours: number;
  minutes: number;
  seconds: number;
}

export const ProjectSpecicifOperationTime = (props: Props) => {
  const { timeMs, disabled, onChange } = props;
  const { t } = useTranslation();
  const hms = convertMilliSecondsToHms(timeMs);

  const { watch, register } = useForm<ProjectSpecificOperationTimeFormData>({
    mode: "all",
    defaultValues: {
      hours: hms.hours,
      minutes: hms.minutes,
      seconds: hms.seconds,
    },
  });

  useEffect(() => {
    const subscription = watch((value, { name, type }) => {
      if (name === "hours" || name === "minutes" || name === "seconds") {
        var ms = convertMilliSecondsFromHms(value.hours, value.minutes, value.seconds);
        onChange(ms);
      }
    });

    return () => subscription.unsubscribe();
  }, [watch, onChange]);

  return (
    <InputContainer sx={{ height: disabled ? 1 : 100, maxWidth: 400 }}>
      <Typography variant="caption" color={"primary.main"}>
        {t("captions.timeWithSecRequired")}
      </Typography>
      {disabled ? (
        <Typography variant="body1">{getHmsFromMilliSeconds(timeMs)}</Typography>
      ) : (
        <Box sx={{ display: "flex", justifyContent: "space-between" }}>
          <FormControl sx={{ my: 1, width: "30%" }}>
            <InputLabel>{t("common.time.hours")}</InputLabel>
            <Select label="hours" defaultValue={hms.hours} {...register("hours")}>
              {Array.from(Array(11).keys()).map((number, index) => {
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
            <Select label="minutes" defaultValue={hms.minutes} {...register("minutes")}>
              {Array.from(Array(61).keys()).map((number, index) => {
                return (
                  <MenuItem key={index} value={number}>
                    {number}
                  </MenuItem>
                );
              })}
            </Select>
          </FormControl>
          <FormControl sx={{ my: 1, width: "30%" }}>
            <InputLabel>{t("common.time.seconds")}</InputLabel>
            <Select label="seconds" defaultValue={hms.seconds} {...register("seconds")}>
              {Array.from(Array(61).keys()).map((number, index) => {
                return (
                  <MenuItem key={index} value={number}>
                    {number}
                  </MenuItem>
                );
              })}
            </Select>
          </FormControl>
        </Box>
      )}
    </InputContainer>
  );
};
