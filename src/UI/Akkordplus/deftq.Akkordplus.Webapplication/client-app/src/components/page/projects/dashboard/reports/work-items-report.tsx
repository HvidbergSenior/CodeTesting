import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import { GetProjectResponse } from "api/generatedApi";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { ReportCardContentList } from "./card-content-list/card-content-list";
import { useDownloadReport } from "./hooks/use-download-report";

interface Props {
  project: GetProjectResponse;
}

export const WorkItemsReport = ({ project }: Props) => {
  const { t } = useTranslation();
  const downloadReport = useDownloadReport();

  const getReport = () => {
    downloadReport(project, "workitems");
  };

  return (
    <CardWithHeaderAndFooter
      titleNamespace={t("dashboard.reports.workItemsReport.title")}
      description={t("dashboard.reports.workItemsReport.description")}
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
    >
      <Box sx={{ pt: 2 }}>
        <ReportCardContentList translationProperty="dashboard.reports.workItemsReport.content" />
      </Box>
    </CardWithHeaderAndFooter>
  );
};
