import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import { styled } from "@mui/material";
import { GetProjectLogBookWeekQueryResponse, GetProjectResponse } from "api/generatedApi";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { ValueCard } from "components/shared/card/value-card";
import { timeToDecimal } from "../hooks/use-format-logbook";
import { ValueCardFooter } from "components/shared/card/value-card-footer";
import { useLogbookRestrictions } from "shared/user-restrictions/use-logbook-restrictions";
import { useEditSalaryAdvance } from "./edit-salary-advance/hooks/use-edit-salary-advance";

interface Props {
  project: GetProjectResponse;
  logbookWeekData: GetProjectLogBookWeekQueryResponse;
  selectedUserId?: string;
}

export const ProjectLogbookStats = (props: Props) => {
  const { project, logbookWeekData, selectedUserId } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const { canViewSalaryAdvance, canEditSalaryAdvance } = useLogbookRestrictions(project);
  const editSalaryAdvance = useEditSalaryAdvance({ project, logbookWeekData, selectedUserId });
  const valueCardFontSize = screenSize === ScreenSizeEnum.Mobile ? "small" : "large";
  const valueCardHeight = screenSize === ScreenSizeEnum.Mobile ? "small" : "medium";

  const BoxContainer = styled(Box)`
    flex: 1;
    min-width: 260px;
  `;

  const getSalaryAdvanceHeader = (): string => {
    if (!logbookWeekData.salaryAdvance?.role || logbookWeekData.salaryAdvance.role === "Undefined") {
      return t("logbook.salaryAdvance.title");
    }
    if (logbookWeekData.salaryAdvance.role === "Apprentice") {
      return t("logbook.salaryAdvance.titleApprentice");
    }
    return t("logbook.salaryAdvance.title");
  };

  const getSalaryAdvanceFooter = (): string => {
    if (!logbookWeekData.salaryAdvance?.start && !logbookWeekData.salaryAdvance?.end) {
      return t("logbook.salaryAdvance.periodNone");
    }
    if (!logbookWeekData.year || !logbookWeekData.week) {
      return t("logbook.salaryAdvance.periodNone");
    }
    if (!logbookWeekData.salaryAdvance?.end) {
      return t("logbook.salaryAdvance.periodFrom", {
        weekFrom: logbookWeekData.salaryAdvance?.start?.week,
        yearFrom: logbookWeekData.salaryAdvance?.start?.year,
      });
    }
    if (!logbookWeekData.salaryAdvance?.start) {
      return t("logbook.salaryAdvance.periodTo", {
        weekTo: logbookWeekData.salaryAdvance?.end?.week,
        yearTo: logbookWeekData.salaryAdvance?.end?.year,
      });
    }
    return t("logbook.salaryAdvance.periodFromTo", {
      weekFrom: logbookWeekData.salaryAdvance?.start?.week,
      yearFrom: logbookWeekData.salaryAdvance?.start?.year,
      weekTo: logbookWeekData.salaryAdvance?.end?.week,
      yearTo: logbookWeekData.salaryAdvance?.end?.year,
    });
  };

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "row",
        gap: 2,
        pb: 2,
        overflowX: "auto",
      }}
    >
      <BoxContainer>
        <ValueCard
          titleNamespace={"logbook.totalTimeWeek"}
          value={timeToDecimal(logbookWeekData?.weekSummation?.hours, logbookWeekData?.weekSummation?.minutes)}
          unitNamespace={"common.time.hours"}
          showBackground={true}
          cardSize={valueCardHeight}
          fontSize={valueCardFontSize}
        />
      </BoxContainer>
      <BoxContainer>
        <ValueCard
          titleNamespace={"logbook.totalTime"}
          value={timeToDecimal(logbookWeekData?.closedWeeksSummation?.hours, logbookWeekData?.closedWeeksSummation?.minutes)}
          unitNamespace={"common.time.hours"}
          showBackground={true}
          cardSize={valueCardHeight}
          fontSize={valueCardFontSize}
        />
      </BoxContainer>
      {canViewSalaryAdvance(selectedUserId) && (
        <BoxContainer>
          <ValueCardFooter
            titleNamespace={getSalaryAdvanceHeader()}
            value={logbookWeekData.salaryAdvance?.amount ?? 0}
            unitNamespace={"logbook.salaryAdvance.pricePrHour"}
            showBackground={true}
            cardSize={valueCardHeight}
            fontSize={valueCardFontSize}
            footer={getSalaryAdvanceFooter()}
            headerActionClickedProps={canEditSalaryAdvance() ? editSalaryAdvance : undefined}
          />
        </BoxContainer>
      )}
    </Box>
  );
};
