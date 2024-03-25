import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import DialogTitleStyled from "components/shared/dialog/dialog-title-styled";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";

export const LogbookInfoDialog = (props: DialogBaseProps) => {
  const { t } = useTranslation();

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitleStyled title={t("logbook.info.title")} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent>
        <Typography variant="subtitle1">{t("logbook.info.absence.title")}</Typography>
        <Typography variant="body2" sx={{ marginBottom: 2 }}>
          {t("logbook.info.absence.description")}
        </Typography>
        <Typography variant="subtitle1" sx={{ display: "flex" }}>
          {t("logbook.info.close.title")} <LockOutlinedIcon fontSize="small" sx={{ color: "primary.main", ml: 0.5 }} />
        </Typography>
        <Typography variant="body2" sx={{ marginBottom: 2 }}>
          {t("logbook.info.close.description")}
        </Typography>
      </DialogContent>
      <DialogActions>
        <Button variant="outlined" onClick={props.onClose}>
          {t("common.close")}
        </Button>
      </DialogActions>
    </Dialog>
  );
};
