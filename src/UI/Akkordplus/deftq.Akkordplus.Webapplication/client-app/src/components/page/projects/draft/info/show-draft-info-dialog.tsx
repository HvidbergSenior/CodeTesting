import { useTranslation } from "react-i18next";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import CloseIcon from "@mui/icons-material/Close";
import { DraftWorkItem } from "utils/offline-storage/use-offline-storage";
import { DialogBaseProps } from "shared/dialog/types";
import { DraftSummary } from "./draft-summary";

interface Props extends DialogBaseProps {
  draft: DraftWorkItem;
}

export function ShowDraftInfoDialog(props: Props) {
  const { draft } = props;
  const { t } = useTranslation();

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitle component="div" textAlign={"center"}>
        <Typography variant="h5" color="primary.dark">
          {t("project.offline.info.title")}
        </Typography>
        <IconButton onClick={props.onClose} sx={{ position: "absolute", top: 0, right: 0 }}>
          <CloseIcon />
        </IconButton>
      </DialogTitle>
      <DialogContent data-testid="show-draft-info-content" sx={{ padding: 0 }}>
        <DraftSummary draft={draft} />
      </DialogContent>
      <DialogActions>
        <Button data-testid="show-draft-info-action-btn-close" variant="contained" onClick={props.onClose}>
          {t("common.close")}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
