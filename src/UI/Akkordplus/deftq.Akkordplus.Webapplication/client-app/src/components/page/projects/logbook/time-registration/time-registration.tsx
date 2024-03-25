import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";

import { GetProjectLogBookDayResponse, GetProjectLogBookWeekQueryResponse, GetProjectResponse } from "api/generatedApi";
import { useTimeRegistrationDialog } from "components/page/projects/logbook/time-registration/hooks/use-time-registration-dialog";
import { getShortDayFromTimestamp } from "utils/formats";
import { useLogbookRestrictions } from "shared/user-restrictions/use-logbook-restrictions";
import { timeToDecimal } from "../hooks/use-format-logbook";

interface Props {
  project: GetProjectResponse;
  logbookWeekData: GetProjectLogBookWeekQueryResponse;
  selectedUserId: string;
}

export const ProjectLogbookTimeRegistration = (props: Props) => {
  const { project, logbookWeekData, selectedUserId } = props;
  const { canEditLogbook } = useLogbookRestrictions(project);

  const openTimeRegistrationDialog = useTimeRegistrationDialog({ project, data: logbookWeekData, userId: selectedUserId });
  const handleTimeRegistration = (day: GetProjectLogBookDayResponse) => {
    openTimeRegistrationDialog(day);
  };

  return (
    <Box sx={{ display: "flex", overflowX: "auto", pb: 2, mb: 1 }}>
      {logbookWeekData.days &&
        logbookWeekData.days.map((day, index) => {
          return (
            <Box key={index} sx={{ display: "flex", flexDirection: "column", alignItems: "center", flex: 1, mx: 0.5 }}>
              <Typography variant="overline">{getShortDayFromTimestamp(day.date ?? "")}</Typography>
              <Button
                variant="contained"
                sx={{ backgroundColor: "white", color: "black", py: 1.5, px: 0, minWidth: "45px", width: "100%" }}
                disabled={!canEditLogbook(logbookWeekData, selectedUserId)}
                onClick={() => handleTimeRegistration(day)}
              >
                {timeToDecimal(day.time?.hours, day.time?.minutes)}
              </Button>
            </Box>
          );
        })}
    </Box>
  );
};
