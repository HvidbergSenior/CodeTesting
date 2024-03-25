import { GetProjectResponse } from "api/generatedApi";
import { useDownloadFile } from "components/shared/download-file/use-download-file";
import { useToast } from "shared/toast/hooks/use-toast";
import { ReportType, useReportDownloadTypeConverter } from "./use-report-download-type-converter";

export function useDownloadReport() {
  const toast = useToast();
  const downloadFile = useDownloadFile();
  const { getFileName, getUrl, getError } = useReportDownloadTypeConverter();

  const clickDownloadReport = async (project: GetProjectResponse, type: ReportType) => {
    try {
      downloadFile(getUrl(project.id ?? "", type), getFileName(type));
    } catch {
      toast.error(getError(type));
    }
  };

  return clickDownloadReport;
}
