import { useEffect, useRef } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import CircularProgress from "@mui/material/CircularProgress";
import styled from "@mui/system/styled";
import { PrintSectionProjectInfoHeader } from "./section-header";
import { PrintSectionProjectInfo } from "./section-info";
import { useGetApiProjectsByProjectIdReportsProjectinfoQuery } from "api/generatedApi";
import { PrintSectionProjectTime } from "./section-time";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { PrintSectionProjectPrice } from "./section-price";
import { PrintSectionProjectParticipants } from "./section-participants";
import { PrintSectionProjectGroupedWorkitems } from "./section-grouped-workitems";
import { useToast } from "shared/toast/hooks/use-toast";

interface Props {
  projectId: string;
}

export const PrintProjectInfoReport = (props: Props) => {
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const { data: report, error: reportError } = useGetApiProjectsByProjectIdReportsProjectinfoQuery({ projectId: props.projectId ?? "" });

  useEffect(() => {
    if (reportError) {
      console.log(reportError);
      toastRef.current.error(tRef.current("dashboard.reports.printReports.getReportDataError"));
    }
  }, [reportError, tRef, toastRef]);

  const PrintContainer = styled(Box)({
    display: "flex",
    justifyContent: "center",
    width: "100%",

    "@media print": {
      transform: "scale(0.5)",
    },
  });

  return (
    <PrintContainer>
      {report?.project &&
        report?.rootFolder &&
        report?.extraWorkAgreementsRates &&
        report?.projectSummation &&
        report?.users &&
        report?.groupedWorkitems && (
          <Paper sx={{ width: "1420px", background: "transparent", flexDirection: "column", display: "flex", gap: 5, pb: 10 }}>
            <PrintSectionProjectInfoHeader />
            <PrintSectionProjectInfo project={report.project} />
            <PrintSectionProjectTime
              project={report.project}
              folder={report.rootFolder as ExtendedProjectFolder}
              extraworkagreementsRates={report.extraWorkAgreementsRates}
            />
            <PrintSectionProjectPrice project={report.project} summaryData={report.projectSummation} />
            <PrintSectionProjectParticipants users={report?.users} />
            <PrintSectionProjectGroupedWorkitems groundWorkitems={report.groupedWorkitems} />
          </Paper>
        )}
      {!report && (
        <Box sx={{ display: "flex", width: "100%", justifyContent: "center", alignItems: "center", pt: 40, backgroundColor: "transparent" }}>
          <CircularProgress size={100} />
        </Box>
      )}
    </PrintContainer>
  );
};
