import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import { useTranslation } from "react-i18next";
import type { DialogBaseProps } from "shared/dialog/types";
import DialogTitleStyled from "../dialog/dialog-title-styled";

interface Props extends DialogBaseProps {
  stringValue: string;
  header?: string;
}

export const ShowStringDialog = (props: Props) => {
  const { stringValue, header } = props;
  const { t } = useTranslation();

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitleStyled title={t(header ?? "common.note")} onClose={props.onClose} isOpen={props.isOpen} />
      <DialogContent>
        <Typography variant="body1" color="primary.main" whiteSpace="pre-wrap">
          {stringValue}
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
