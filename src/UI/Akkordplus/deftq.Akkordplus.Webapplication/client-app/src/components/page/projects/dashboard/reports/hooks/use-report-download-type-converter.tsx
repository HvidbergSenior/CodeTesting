import { makeFullPath } from "api/path";
import { useTranslation } from "react-i18next";
import { formatNewDate } from "utils/formats";

export type ReportType = "workitems" | "logbook" | "status";

export const useReportDownloadTypeConverter = () => {
  const { t } = useTranslation();

  const getFileName = (type: ReportType): string => {
    const dateNow = formatNewDate(false);
    var fileName = t("dashboard.reports.unknownReport.file");
    switch (type) {
      case "logbook":
        fileName = "dashboard.reports.logbookReport.file";
        break;
      case "workitems":
        fileName = "dashboard.reports.workItemsReport.file";
        break;
      case "status":
        fileName = "dashboard.reports.statusReport.file";
        break;
    }
    return t(fileName, { date: dateNow });
  };

  const getUrl = (projectId: string, type: ReportType): string => {
    var urlDestination = "workitemsspreadsheet";
    switch (type) {
      case "logbook":
        urlDestination = "logbookspreadsheet";
        break;
      case "workitems":
        urlDestination = "workitemsspreadsheet";
        break;
      case "status":
        urlDestination = "statusreportspreatsheet";
        break;
    }
    return makeFullPath(`/api/projects/${projectId}/reports/${urlDestination}`);
  };

  const getError = (type: ReportType) => {
    switch (type) {
      case "logbook":
        return t("dashboard.reports.logbookReport.downloadError");
      case "workitems":
        return t("dashboard.reports.workItemsReport.downloadError");
      case "status":
        return t("dashboard.reports.logbookReport.downloadError");
    }

    return t("dashboard.reports.unknownReport.downloadError");
  };

  return { getFileName, getUrl, getError };
};
