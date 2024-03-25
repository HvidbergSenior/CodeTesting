import SensorsOffIcon from "@mui/icons-material/SensorsOff";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

export const OfflineDialog = (props: DialogBaseProps) => {
  const { onClose, isOpen } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();
  const marginSides = screenSize === ScreenSizeEnum.Mobile ? "0px" : "0px 40px";
  return (
    <Dialog maxWidth="xs" open={isOpen} PaperProps={{ sx: { width: "500px", height: "350px" } }}>
      <DialogContent
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        <SensorsOffIcon fontSize="large" sx={{ color: "primary.dark", transform: "scale(1.3)" }} />
        <Typography variant="h5" sx={{ marginTop: 2, marginBottom: 2, textAlign: "center", color: "primary.dark" }}>
          {t("project.offline.title")}
        </Typography>
        <Typography variant="body2" color={"grey.50"} textAlign="center" sx={{ margin: marginSides }}>
          {t("project.offline.description")}
        </Typography>
        <Button sx={{ width: "200px", marginTop: 4 }} variant="contained" onClick={onClose}>
          {t("project.offline.goToDraft")}
        </Button>
      </DialogContent>
    </Dialog>
  );
};
