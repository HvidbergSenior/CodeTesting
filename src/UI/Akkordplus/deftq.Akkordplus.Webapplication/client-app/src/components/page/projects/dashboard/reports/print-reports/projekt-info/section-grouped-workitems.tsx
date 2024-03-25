import { useTranslation } from "react-i18next";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import styled from "@mui/system/styled";
import { GroupedWorkItemsResponse } from "api/generatedApi";
import { GroupingWorkitemsTable } from "components/page/folder/content/overview/overview-grouping/dialog/grouping-workitems-table";

interface Props {
  groundWorkitems: GroupedWorkItemsResponse[];
}

export const PrintSectionProjectGroupedWorkitems = ({ groundWorkitems }: Props) => {
  const { t } = useTranslation();

  const PrintContainer = styled(Box)({
    "@media print": {
      breakInside: "avoid",
    },
  });
  return (
    <PrintContainer sx={{ display: "flex", flexDirection: "column", gap: 2, backgroundColor: "white", p: 5, borderRadius: 2 }}>
      <Typography variant="h6" sx={{ pb: 2 }}>
        {t("dashboard.reports.printReports.projectGoupedWorkitems.title")}
      </Typography>
      <GroupingWorkitemsTable groupedWorkitems={groundWorkitems} scroll={false} sorting={false} />
    </PrintContainer>
  );
};
