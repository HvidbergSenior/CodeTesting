import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import { GetProjectResponse } from "api/generatedApi";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { ReportCardContentList } from "./card-content-list/card-content-list";
import { useDownloadReport } from "./hooks/use-download-report";
import { useDashboardRestrictions } from "shared/user-restrictions/use-dashboard-restrictions";

interface Props {
  project: GetProjectResponse;
}

export const StatusReport = ({ project }: Props) => {
  const { t } = useTranslation();
  const downloadReport = useDownloadReport();
  const { canDownloadStatusReport } = useDashboardRestrictions(project);

  const getReport = () => {
    downloadReport(project, "status");
  };

  return (
    <CardWithHeaderAndFooter
      titleNamespace={t("dashboard.reports.statusReport.title")}
      description={t("dashboard.reports.statusReport.description")}
      height={"400px"}
      headerActionIcon={undefined}
      showHeaderAction={false}
      showBottomAction={"button"}
      headerActionClickedProps={() => {}}
      bottomActionClickedProps={getReport}
      bottomActionText="dashboard.reports.exportReport"
      showContent={true}
      noContentText={""}
      showDescription={true}
      hasChildPadding={true}
      disableBottomButton={!canDownloadStatusReport()}
    >
      <Box sx={{ pt: 2 }}>
        <ReportCardContentList translationProperty="dashboard.reports.statusReport.content" />
      </Box>
    </CardWithHeaderAndFooter>
  );
};
