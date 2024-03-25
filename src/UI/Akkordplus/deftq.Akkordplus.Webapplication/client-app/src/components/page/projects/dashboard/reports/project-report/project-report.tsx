import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import Box from "@mui/material/Box";
import { GetProjectResponse } from "api/generatedApi";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { ReportCardContentList } from "../card-content-list/card-content-list";

interface Props {
  project: GetProjectResponse;
}

export const ProjectReport = ({ project }: Props) => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const printUrl = `/reports/projects/${project.id}/projectinfo/`;

  const openPrintReport = () => {
    navigate(printUrl);
  };

  return (
    <CardWithHeaderAndFooter
      titleNamespace={t("dashboard.reports.projectReport.title")}
      description={t("dashboard.reports.projectReport.description")}
      height={"400px"}
      headerActionIcon={undefined}
      showHeaderAction={false}
      showBottomAction={"button"}
      headerActionClickedProps={() => {}}
      bottomActionClickedProps={openPrintReport}
      bottomActionText="dashboard.reports.showReport"
      showContent={true}
      noContentText={""}
      showDescription={true}
      hasChildPadding={true}
    >
      <Box sx={{ pt: 2 }}>
        <ReportCardContentList translationProperty="dashboard.reports.projectReport.content" />
      </Box>
    </CardWithHeaderAndFooter>
  );
};
