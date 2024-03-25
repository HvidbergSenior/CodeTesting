import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import TodayIcon from "@mui/icons-material/Today";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";

import { GetProjectLogBookWeekQueryResponse } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

interface Props {
  logbookWeekData: GetProjectLogBookWeekQueryResponse;
  dateRange: string;
  onChangePeriod: (content?: "back" | "next") => void;
}

export const ProjectLogbookSelectPeriod = (props: Props) => {
  const { logbookWeekData, dateRange } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();

  return (
    <Box>
      <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        {screenSize === ScreenSizeEnum.Mobile ? (
          <Button variant="text" sx={{ p: 1, pb: 1.5, fontSize: "12px", minWidth: "10px" }} onClick={() => props.onChangePeriod()}>
            <TodayIcon />
          </Button>
        ) : (
          <Button variant="text" sx={{ p: 0, fontSize: "12px" }} onClick={() => props.onChangePeriod()}>
            <TodayIcon />
            {t("common.time.today")}
          </Button>
        )}
        <Box>
          <IconButton onClick={() => props.onChangePeriod("back")} sx={{ p: 0, pb: 0.5 }}>
            <KeyboardArrowLeftIcon fontSize="medium" />
          </IconButton>
          <Typography variant="caption" fontSize="medium">
            {t("common.time.week")} {logbookWeekData.week}: {dateRange}
          </Typography>
          <IconButton onClick={() => props.onChangePeriod("next")} sx={{ p: 0, pb: 0.5 }}>
            <KeyboardArrowRightIcon fontSize="medium" />
          </IconButton>
        </Box>
      </Box>
    </Box>
  );
};
