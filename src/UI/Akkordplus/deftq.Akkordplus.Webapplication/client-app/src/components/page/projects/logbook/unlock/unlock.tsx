import { Dialog, DialogActions, Typography, Button, DialogContent } from "@mui/material";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import LockOpenIcon from "@mui/icons-material/LockOpen";

interface Props extends DialogBaseProps {
  onSubmit: () => void;
}

export const LogbookUnlock = (props: Props) => {
  const { t } = useTranslation();

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogContent
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          marginTop: 3,
        }}
      >
        <LockOpenIcon />
        <Typography variant="h6" sx={{ marginTop: 1, marginBottom: 2 }}>
          {t("logbook.unlock.title")}
        </Typography>
        <Typography variant="body1" textAlign="center">
          {t("logbook.unlock.description")}
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.cancel")}
        </Button>
        <Button variant="contained" color="primary" onClick={props.onSubmit}>
          {t("logbook.unlock.submit")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
