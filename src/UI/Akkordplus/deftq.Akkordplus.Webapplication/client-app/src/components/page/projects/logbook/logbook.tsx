import { useTranslation } from "react-i18next";
import { useEffect, useState, useRef } from "react";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Button from "@mui/material/Button";
import LockOpenOutlinedIcon from "@mui/icons-material/LockOpenOutlined";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import CircularProgress from "@mui/material/CircularProgress";

import { api } from "api/enhancedEndpoints";
import {
  GetProjectLogBookWeekQueryResponse,
  GetProjectResponse,
  LogBookUserResponse,
  useGetApiProjectsByProjectIdLogbookQuery,
} from "api/generatedApi";
import { formatTimestamp, formatTimestampToDate } from "utils/formats";
import { useLogbookCloseDialog } from "./close/hooks/use-close-dialog";
import { useLogbookUnlock } from "./unlock/hooks/use-unlock";
import { useToast } from "shared/toast/hooks/use-toast";
import { useLogbookRestrictions } from "shared/user-restrictions/use-logbook-restrictions";
import { getPCA } from "index";
import { ProjectLogbookNotes } from "./notes/logbook-notes";
import { ProjectLogbookStats } from "./logbook-stats/logbook-stats";
import { ProjectLogbookSelectUser } from "./select-user/logbook-select-user";
import { ProjectLogbookSelectPeriod } from "./select-period/logbook-select-period";
import { ProjectLogbookTimeRegistration } from "./time-registration/time-registration";

interface Props {
  project: GetProjectResponse;
}

export const ProjectLogbook = (props: Props) => {
  const { project } = props;
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const { canViewLogbook, canCloseLogbookPeriod, canOpenLogbookPeriod } = useLogbookRestrictions(project);
  const activeAccountId = getPCA()?.getActiveAccount()?.localAccountId;
  const [logbookWeekData, setlogbookWeekData] = useState<GetProjectLogBookWeekQueryResponse | undefined>(undefined);
  const [selectedUserId, setSelectedUserId] = useState<LogBookUserResponse["userId"] | undefined>(undefined);
  const [dateRange, setDateRange] = useState("");

  const { data: getLogbookUsersData, isError: getLogbookUsersDataError } = useGetApiProjectsByProjectIdLogbookQuery({
    projectId: project.id ? project.id : "",
  });
  const [queryLogbookByDay, { data: getLogbookWeekData, isError: getLogbookWeekDataError }] =
    api.endpoints.getApiProjectsByProjectIdLogbookAndUserIdWeeksYearMonthDay.useLazyQuery();

  useEffect(() => {
    setlogbookWeekData(getLogbookWeekData);
    if (logbookWeekData && logbookWeekData.days) {
      setDateRange(
        formatTimestamp(logbookWeekData.days[0].date, false, "short") + " - " + formatTimestamp(logbookWeekData.days[6].date, false, "short")
      );
    }
    if (getLogbookUsersDataError) {
      console.error(getLogbookUsersDataError);
      toastRef.current.error(tRef.current("logbook.getLogbook.error"));
    }
  }, [logbookWeekData, getLogbookWeekData, getLogbookUsersDataError, toastRef, tRef]);

  useEffect(() => {
    if (getLogbookWeekDataError) {
      console.error(getLogbookWeekDataError);
      toastRef.current.error(tRef.current("logbook.getLogbook.error"));
    }
  }, [getLogbookWeekDataError, toastRef, tRef]);

  useEffect(() => {
    if (selectedUserId) {
      let currentDate = new Date();
      if (logbookWeekData && logbookWeekData.days) {
        currentDate = formatTimestampToDate(logbookWeekData.days[0].date);
      }
      queryLogbookByDay({
        projectId: project.id ? project.id : "",
        userId: selectedUserId,
        year: currentDate.getFullYear(),
        month: currentDate.getMonth() + 1,
        day: currentDate.getDate(),
      });
    }
  }, [logbookWeekData, project.id, queryLogbookByDay, selectedUserId]);

  useEffect(() => {
    if (getLogbookUsersData && getLogbookUsersData.users && activeAccountId) {
      const found = getLogbookUsersData.users.find((u) => u.userId === activeAccountId);
      if (found) {
        setSelectedUserId(activeAccountId);
      }
    }
  }, [getLogbookUsersData, setSelectedUserId, activeAccountId]);

  const changeDate = (content?: "back" | "next") => {
    let date = new Date();
    if (logbookWeekData && logbookWeekData.days && content) {
      date = formatTimestampToDate(logbookWeekData.days[0].date);
    }
    if (content === "back") {
      date.setDate(date.getDate() - 7);
    } else if (content === "next") {
      date.setDate(date.getDate() + 7);
    }
    queryLogbookByDay({
      projectId: project.id ? project.id : "",
      userId: selectedUserId ?? "",
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate(),
    });
  };

  const openLogbookCloseDialog = useLogbookCloseDialog({ project, data: logbookWeekData, userId: selectedUserId });
  const openLogbookUnlockDialog = useLogbookUnlock({ project, data: logbookWeekData, userId: selectedUserId });

  return (
    <Grid item sx={{ display: "flex", flexDirection: "column", width: "100%", height: "100%", p: 1 }} xs={true}>
      {getLogbookUsersData && (
        <ProjectLogbookSelectUser
          getLogbookUsersData={getLogbookUsersData}
          project={project}
          selectedUserId={selectedUserId}
          onSelectUserId={setSelectedUserId}
        />
      )}
      {logbookWeekData && canViewLogbook() && (
        <Box sx={{ display: "flex", flexDirection: "column", flex: 1, p: 1 }}>
          <ProjectLogbookStats project={project} logbookWeekData={logbookWeekData} selectedUserId={selectedUserId} />
          <ProjectLogbookSelectPeriod dateRange={dateRange} logbookWeekData={logbookWeekData} onChangePeriod={changeDate} />
          {!!selectedUserId && <ProjectLogbookTimeRegistration project={project} logbookWeekData={logbookWeekData} selectedUserId={selectedUserId} />}
          {!!selectedUserId && <ProjectLogbookNotes project={project} logbookWeekData={logbookWeekData} selectedUserId={selectedUserId} />}
          <Box sx={{ display: "flex", alignItems: "flex-end", justifyContent: "space-between", flex: 1, mb: 2 }}>
            {logbookWeekData.closed ? (
              <IconButton onClick={openLogbookUnlockDialog} sx={{ p: 0 }} disabled={!canOpenLogbookPeriod(logbookWeekData)}>
                <LockOutlinedIcon sx={{ color: "secondary.main" }} />
              </IconButton>
            ) : (
              <LockOpenOutlinedIcon sx={{ color: "primary.main" }} />
            )}
            {canCloseLogbookPeriod(logbookWeekData, selectedUserId) && !logbookWeekData.closed && (
              <Button variant="contained" onClick={openLogbookCloseDialog}>
                {t("logbook.submit")}
              </Button>
            )}
          </Box>
        </Box>
      )}
      {!getLogbookUsersData && (
        <Box sx={{ width: "100%", height: "100%", display: "flex", justifyContent: "center", alignItems: "center" }}>
          <CircularProgress size={150} />
        </Box>
      )}
    </Grid>
  );
};
