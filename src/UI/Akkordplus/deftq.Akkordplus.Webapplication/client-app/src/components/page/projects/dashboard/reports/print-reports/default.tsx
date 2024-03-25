import { useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import { RouteReportSubPage } from "shared/route-types";
import { DefaultOneStepBackNavigation } from "components/shared/one-step-back-navigation/default";
import { PrintProjectInfoReport } from "./projekt-info/print-project-info";
import { costumPalette } from "theme/palette";

interface Props {
  subPage: RouteReportSubPage;
}

export const DefaultProjectPrintReportPage = (props: Props) => {
  const { t } = useTranslation();
  const { projectId } = useParams<"projectId">();

  return (
    <Box sx={{ display: "flex", flexDirection: "column", flexGrow: 1, backgroundColor: costumPalette.reportBackground }}>
      <DefaultOneStepBackNavigation from={t("dashboard.links.reports")} here={t("dashboard.reports.projectReport.title")} />
      {props.subPage === "projekt-info" && <PrintProjectInfoReport projectId={projectId ?? ""} />}
    </Box>
  );
};
