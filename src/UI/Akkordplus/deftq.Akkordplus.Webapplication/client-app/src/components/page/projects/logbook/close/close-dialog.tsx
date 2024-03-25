import { Dialog, DialogActions, Typography, Button, DialogContent } from "@mui/material";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import ErrorIcon from "@mui/icons-material/Error";

interface Props extends DialogBaseProps {
  onSubmit: () => void;
}

export const LogbookCloseDialog = (props: Props) => {
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
        <ErrorIcon />
        <Typography variant="h5" sx={{ marginTop: 1, marginBottom: 2 }}>
          {t("logbook.close.title")}
        </Typography>
        <Typography variant="body1" textAlign="center">
          {t("logbook.close.description")}
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.cancel")}
        </Button>
        <Button variant="contained" color="primary" onClick={props.onSubmit}>
          {t("common.finish")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
