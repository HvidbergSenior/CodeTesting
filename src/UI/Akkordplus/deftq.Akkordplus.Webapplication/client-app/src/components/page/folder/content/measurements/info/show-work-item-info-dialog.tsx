import { useTranslation } from "react-i18next";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import CloseIcon from "@mui/icons-material/Close";
import type { GetProjectResponse, ProjectFolderResponse, WorkItemResponse } from "api/generatedApi";
import type { DialogBaseProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { WorkItemSummary } from "../create/create-work-item-dialog/step-info/work-item-summary";
import { useEditWorkItem } from "../edit/hooks/use-edit-work-item";
import { DividerStyled } from "./show-work-item-info-dialog-styles";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";

interface Props extends DialogBaseProps {
  project: GetProjectResponse;
  folder: ProjectFolderResponse;
  workItem: WorkItemResponse;
}

export function ShowWorkItemInfoDialog(props: Props) {
  const { project, folder, workItem } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const openEditDialog = useEditWorkItem({ project, folder, workItem });
  const { canEditWorkitem } = useWorkItemRestrictions(project);

  const editWorkItem = () => {
    if (props.onClose) {
      props.onClose();
    }
    openEditDialog();
  };
  return (
    <Dialog
      fullWidth
      maxWidth="sm"
      PaperProps={{
        sx: {
          height: screenSize === ScreenSizeEnum.Mobile ? "100%" : "750px", // mobile is ignored
          margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
          width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100%-64px)",
        },
      }}
      open={props.isOpen}
    >
      <DialogTitle component="div" textAlign={"center"}>
        <Typography variant="h5" color="primary.dark">
          {t("content.measurements.info.headerView")}
        </Typography>
        <IconButton onClick={props.onClose} sx={{ position: "absolute", top: 0, right: 0 }}>
          <CloseIcon />
        </IconButton>
        <DividerStyled sx={{ borderColor: "grey.100" }} />
      </DialogTitle>
      <DialogContent data-testid="is-the-the-container" sx={{ padding: 0 }}>
        {!workItem && (
          <Box sx={{ mt: 5, textAlign: "center" }}>
            <CircularProgress size={100} />
          </Box>
        )}
        {workItem && <WorkItemSummary workItem={workItem} />}
      </DialogContent>
      <DialogActions>
        {canEditWorkitem(folder) && (
          <Button data-testid="workitem.info-action-btn-edit" variant="contained" onClick={editWorkItem}>
            {t("common.edit")}
          </Button>
        )}
        <Button data-testid="workitem.info-action-btn-next" variant="contained" onClick={props.onClose}>
          {t("common.close")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
