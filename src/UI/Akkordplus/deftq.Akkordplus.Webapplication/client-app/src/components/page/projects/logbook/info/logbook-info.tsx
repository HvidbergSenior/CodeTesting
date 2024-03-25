import IconButton from "@mui/material/IconButton";
import InfoIcon from "@mui/icons-material/Info";
import { useDialog } from "shared/dialog/use-dialog";
import { LogbookInfoDialog } from "./info-dialog";

export const ProjectLogbookInfo = () => {
  const [openLogbookInfoDialog] = useDialog(LogbookInfoDialog);
  const handleOpenLogbookInfoDialog = () => {
    openLogbookInfoDialog({});
  };

  return (
    <IconButton onClick={handleOpenLogbookInfoDialog}>
      <InfoIcon sx={{ color: "primary.main" }} />
    </IconButton>
  );
};
