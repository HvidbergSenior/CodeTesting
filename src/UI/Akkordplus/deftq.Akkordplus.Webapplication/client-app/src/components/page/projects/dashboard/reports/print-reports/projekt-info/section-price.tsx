import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import { useTranslation } from "react-i18next";
import { GetProjectResponse, GetProjectSummationQueryResponse } from "api/generatedApi";
import SummationsDashboardDesktop from "../../../summations/summations-dashboard-desktop";

interface Props {
  project: GetProjectResponse;
  summaryData: GetProjectSummationQueryResponse;
}

export const PrintSectionProjectPrice = ({ project, summaryData }: Props) => {
  const { t } = useTranslation();

  return (
    <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
      <Typography variant="h6" sx={{ pb: 2 }}>
        {t("dashboard.reports.printReports.projectPrice.title")}
      </Typography>
      <SummationsDashboardDesktop project={project} summaryData={summaryData} usePrintlayOut={true} />
    </Box>
  );
};
