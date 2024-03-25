import { Dialog, DialogActions, Typography, Button, DialogContent } from "@mui/material";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import ErrorIcon from "@mui/icons-material/Error";
import CloseIcon from "@mui/icons-material/Close";
import IconButton from "@mui/material/IconButton";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

interface Props extends DialogBaseProps {
  onSubmit: () => void;
  title: string;
  description: string;
  submitButtonLabel?: string;
}

export const ConfirmNoOptions = (props: Props) => {
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
        <IconButton onClick={props.onClose} sx={{ position: "absolute", top: 5, right: 5 }}>
          <CloseIcon fontSize="medium" />
        </IconButton>
        <ErrorIcon fontSize="large" sx={{ color: "primary.dark", transform: "scale(1.3)" }} />
        <Typography variant="h5" sx={{ marginTop: 2, marginBottom: 2, textAlign: "center", color: "primary.dark" }}>
          {props.title}
        </Typography>
        <Typography variant="body2" color={"grey.50"} textAlign="center" sx={{ margin: marginSides }}>
          {props.description}
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.ok")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
