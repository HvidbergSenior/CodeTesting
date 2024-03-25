import { useEffect, useState } from "react";
import Box from "@mui/material/Box";
import Paper from "@mui/material/Paper";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import OutputIcon from "@mui/icons-material/Output";
import { useTranslation } from "react-i18next";
import { GroupedWorkItemsResponse, useGetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedQuery } from "api/generatedApi";
import { CardWithHeaderAndFooter } from "components/shared/card/card-header-footer";
import { useDialog } from "shared/dialog/use-dialog";
import { useToast } from "shared/toast/hooks/use-toast";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { GroupingWorkitemsDialog } from "../dialog/grouping-workitems-dialog";
import GroupingWidgetRow from "./widget-row";

interface Props {
  folderId: string;
  projectId: string;
}

export const GroupingWidget = ({ folderId, projectId }: Props) => {
  const { t } = useTranslation();
  const [isLoading, setIsLoading] = useState(true);
  const [showContent, setShowContent] = useState(true);
  const [openDialog] = useDialog(GroupingWorkitemsDialog);
  const toast = useToast();

  const { data: groupedData, error: groupedWorkItemsError } = useGetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedQuery(
    {
      folderId: folderId,
      projectId: projectId,
      maxHits: 3,
    },
    { refetchOnMountOrArgChange: true }
  );
  const nonActionFunction = () => {};
  const [workItemsGrouped, setWorkItemsGrouped] = useState<GroupedWorkItemsResponse[]>([]);

  useEffect(() => {
    setIsLoading(true);
    setShowContent(true);
    if (groupedData) {
      if (groupedData?.groupedWorkItems && groupedData?.groupedWorkItems?.length === 0) {
        setupEmptyPage();
      } else {
        setWorkItemsGrouped(groupedData?.groupedWorkItems ?? []);
        setIsLoading(false);
        setShowContent(true);
      }
    }
    if (groupedData === null) {
      setupEmptyPage();
    }
    if (groupedWorkItemsError) {
      toast.error(t("content.overview.grouping.widget.error"));
      setupEmptyPage();
    }
  }, [groupedData, groupedWorkItemsError, t, toast]);

  const setupEmptyPage = () => {
    setIsLoading(false);
    setShowContent(false);
  };

  const outputIcon = (
    <Box sx={{ position: "relative" }}>
      <OutputIcon />
    </Box>
  );

  const { screenSize } = useScreenSize();
  const isMobile = screenSize === ScreenSizeEnum.Mobile;

  const openGroupingDialog = () => {
    openDialog({ folderId, projectId });
  };

  return (
    <CardWithHeaderAndFooter
      titleNamespace={t("content.overview.grouping.title")}
      height={"420px"}
      headerActionIcon={outputIcon}
      showHeaderAction={false}
      showBottomAction={!isMobile && showContent && !isLoading ? "showMore" : "none"}
      headerActionClickedProps={nonActionFunction}
      bottomActionClickedProps={openGroupingDialog}
      showContent={showContent}
      noContentText={t("content.overview.grouping.widget.noContentText")}
      isLoading={isLoading}
      description={t("content.overview.grouping.widget.description")}
      showDescription={true}
      hasChildPadding={true}
    >
      <Box paddingTop={2}>
        <TableContainer sx={{ height: "100%", overflowY: "auto" }} component={Paper}>
          <Table>
            <TableHead sx={{ backgroundColor: "primary.light" }}>
              <TableRow>
                <TableCell data-testid={"overview-grouping-widget-title"} key={1} sx={{ color: "primary.main", width: 10 }}>
                  {t("content.overview.grouping.widget.table.title")}
                </TableCell>
                <TableCell
                  data-testid={"overview-grouping-widget-quantity"}
                  key={2}
                  sx={{ color: "primary.main", width: 10, textAlign: "right", pr: 5 }}
                >
                  {t("content.overview.grouping.widget.table.quantity")}
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {workItemsGrouped.map((groupedWorkitem) => (
                <GroupingWidgetRow key={groupedWorkitem.id} groupedWorkItem={groupedWorkitem} />
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Box>
    </CardWithHeaderAndFooter>
  );
};
