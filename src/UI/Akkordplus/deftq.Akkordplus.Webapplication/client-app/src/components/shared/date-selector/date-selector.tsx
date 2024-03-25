import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import FormControl from "@mui/material/FormControl";
import InputLabel from "@mui/material/InputLabel";
import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";
import Typography from "@mui/material/Typography";

type Props = {
  defaultDate?: Date;
  onChangeProps: (data: Date | undefined) => void;
};

export const DateSelector = (props: Props) => {
  const { defaultDate, onChangeProps } = props;
  const { t } = useTranslation();
  const yearNow = new Date().getFullYear();
  const defaultDateValue = defaultDate?.getDate() ?? 0;
  const defaultMonthValue = defaultDate ? defaultDate.getMonth() + 1 : 0;
  const defaultYearValue = defaultDate?.getFullYear() ?? 0;

  const [dateValue, setDateValue] = useState<number>(defaultDateValue);
  const [monthValue, setMonthValue] = useState<number>(defaultMonthValue);
  const [yearValue, setYearValue] = useState<number>(defaultYearValue);

  const [illegalDate, setIllegalDate] = useState(false);

  useEffect(() => {
    setIllegalDate(false);
    if (dateValue === 0 && monthValue === 0 && yearValue === 0) {
      onChangeProps(undefined);
      return;
    }
    if (dateValue === 0 || monthValue === 0 || yearValue === 0) {
      setIllegalDate(true);
      return;
    }
    const newDate = new Date(yearValue, monthValue - 1, dateValue);
    if (newDate.getDate() !== dateValue || newDate.getMonth() + 1 !== monthValue) {
      setIllegalDate(true);
      return;
    }
    onChangeProps(newDate);
  }, [dateValue, monthValue, yearValue, setIllegalDate, onChangeProps]);

  return (
    <Box>
      <Box sx={{ display: "flex", flexDirection: "row", gap: 1 }}>
        <FormControl sx={{ minWidth: 75 }}>
          <InputLabel>{t("common.time.date")}</InputLabel>
          <Select label="date" value={dateValue} onChange={(e) => setDateValue(e.target.value as number)}>
            {Array.from(Array(32).keys()).map((number, index) => {
              return (
                <MenuItem key={index} value={number}>
                  {number === 0 ? "-" : number}
                </MenuItem>
              );
            })}
          </Select>
        </FormControl>
        <FormControl sx={{ minWidth: 75 }}>
          <InputLabel>{t("common.time.month")}</InputLabel>
          <Select label="month" value={monthValue} onChange={(e) => setMonthValue(e.target.value as number)}>
            {Array.from(Array(13).keys()).map((number, index) => {
              return (
                <MenuItem key={index} value={number}>
                  {number === 0 ? "-" : number}
                </MenuItem>
              );
            })}
          </Select>
        </FormControl>
        <FormControl sx={{ minWidth: 105 }}>
          <InputLabel>{t("common.time.year")}</InputLabel>
          <Select label="year" value={yearValue} onChange={(e) => setYearValue(e.target.value as number)}>
            <MenuItem value={0}>-</MenuItem>
            {Array.from(Array(12), (_, i) => i + yearNow - 3).map((number, index) => {
              return (
                <MenuItem key={index} value={number}>
                  {number}
                </MenuItem>
              );
            })}
          </Select>
        </FormControl>
      </Box>
      {illegalDate && (
        <Typography variant="caption" color="error">
          {t("shared.dateSelector.illegalDate")}
        </Typography>
      )}
    </Box>
  );
};
