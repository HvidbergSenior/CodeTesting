import CloseIcon from "@mui/icons-material/Close";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Card from "@mui/material/Card";
import CardActions from "@mui/material/CardActions";
import CardContent from "@mui/material/CardContent";
import CardHeader from "@mui/material/CardHeader";
import CircularProgress from "@mui/material/CircularProgress";
import Dialog from "@mui/material/Dialog";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import styled from "@mui/system/styled";
import { GroupedWorkItemsResponse, useGetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedQuery } from "api/generatedApi";
import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import { useToast } from "shared/toast/hooks/use-toast";
import { GroupingWorkitemsTable } from "./grouping-workitems-table";

interface Props extends DialogBaseProps {
  folderId: string;
  projectId: string;
}

export type WorkItemsGroupSortableId = "Text" | "Amount" | "PaymentDkr";

export interface TableHeaderGrouping {
  id: WorkItemsGroupSortableId;
  title: string;
  sortable: boolean;
  testid: string;
}

export const GroupingWorkitemsDialog = ({ onClose, isOpen, folderId, projectId }: Props) => {
  const { t } = useTranslation();
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const toast = useToast();
  const { data: groupedData, error: groupedWorkItemsError } = useGetApiProjectsByProjectIdFoldersAndFolderIdWorkitemsGroupedQuery({
    folderId: folderId,
    projectId: projectId,
    maxHits: 1000,
  });
  const [workItemsGrouped, setWorkItemsGrouped] = useState<GroupedWorkItemsResponse[]>(groupedData?.groupedWorkItems ?? []);

  useEffect(() => {
    setIsLoading(true);
    if (groupedData) {
      if (groupedData?.groupedWorkItems && groupedData?.groupedWorkItems?.length === 0) {
        setIsLoading(false);
      } else {
        setWorkItemsGrouped(groupedData?.groupedWorkItems ?? []);
        setIsLoading(false);
      }
    }

    if (groupedData === null) {
      setIsLoading(false);
    }

    if (groupedWorkItemsError) {
      toast.error(t("content.overview.grouping.widget.error"));
      setIsLoading(false);
    }
  }, [groupedData, groupedWorkItemsError, t, toast]);

  const CardStyled = styled(Card)`
    display: flex;
    flex-direction: column;
  `;

  const CardHeaderStyled = styled(CardHeader)`
    padding: 15px !important;
    height: 45px !important;
    > div {
      display: inherit !important;
      padding-right: 2px !important;
      > button {
        margin-top: -7px;
      }
    }
  `;

  const CardContentStyled = styled(CardContent)`
    padding-top: 0;
    padding-bottom: 0;
    align-content: center;
    flex-grow: 1;
  `;

  const CardActionsStyled = styled(CardActions)`
    justify-content: right;
    padding: 15px !important;
    height: 45px !important;
  `;

  return (
    <Dialog maxWidth="xl" open={isOpen}>
      <Divider variant="fullWidth" sx={{ zIndex: 2, top: "110px", position: "relative", color: "primary.dark" }}></Divider>
      <CardStyled sx={{ height: 886, p: 4 }}>
        <CardHeaderStyled titleTypographyProps={{ variant: "h5", color: "primary.dark" }} title={t("content.overview.grouping.title")} />
        <Typography paddingLeft={"15px"} paddingBottom={"16px"} variant="body2" color={"grey.50"}>
          {t("content.overview.grouping.dialog.description")}
        </Typography>
        <IconButton onClick={onClose} sx={{ position: "absolute", top: 5, right: 5 }}>
          <CloseIcon fontSize="medium" />
        </IconButton>

        {!isLoading && (
          <CardContentStyled>
            <Box paddingTop={2}>
              <GroupingWorkitemsTable groupedWorkitems={workItemsGrouped} scroll={true} sorting={true} />
            </Box>
          </CardContentStyled>
        )}
        {isLoading && (
          <CardContentStyled sx={{ display: "flex", justifyContent: "center" }}>
            <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
              <CircularProgress size={60} />
            </Box>
          </CardContentStyled>
        )}
        <CardActionsStyled>
          <Button data-testid="grouping-dialog-close" sx={{ width: "130px" }} variant="outlined" onClick={onClose}>
            {t("common.ok")}
          </Button>
        </CardActionsStyled>
      </CardStyled>
    </Dialog>
  );
};
