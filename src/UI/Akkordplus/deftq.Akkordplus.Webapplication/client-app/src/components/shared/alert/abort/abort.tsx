import { Dialog, DialogActions, Typography, Button, DialogContent } from "@mui/material";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import ErrorIcon from "@mui/icons-material/Error";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

interface Props extends DialogBaseProps {
  onSubmit: () => void;
}

export const AbortDialog = (props: Props) => {
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const marginSides = screenSize === ScreenSizeEnum.Mobile ? "0px" : "0px 40px";

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
        <ErrorIcon fontSize="large" sx={{ color: "primary.dark", transform: "scale(1.3)" }} />
        <Typography variant="h5" sx={{ marginTop: 2, marginBottom: 2, textAlign: "center", color: "primary.dark" }}>
          {t("shared.abort.title")}
        </Typography>
        <Typography variant="body2" color={"grey.50"} textAlign="center" sx={{ margin: marginSides }}>
          {t("shared.abort.description")}
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("shared.abort.label")}
        </Button>
        <Button variant="contained" color="primary" onClick={props.onSubmit}>
          {t("common.finish")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
