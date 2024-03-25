import { useTranslation } from "react-i18next";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import { ProjectFolderResponse } from "api/generatedApi";
import type { DialogBaseProps } from "shared/dialog/types";
import { formatTimestamp } from "utils/formats";

interface Props extends DialogBaseProps {
  folder: ProjectFolderResponse | undefined;
}

export const FolderInfo = (props: Props) => {
  const { folder } = props;
  const { t } = useTranslation();

  return (
    <Dialog maxWidth="xs" open={props.isOpen}>
      <DialogTitle
        component="div"
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          marginTop: 1,
        }}
      >
        <InfoOutlinedIcon />
        <Typography variant="h5" color="primary.dark" sx={{ marginTop: 1, marginBottom: 2 }}>
          {folder?.projectFolderName}
        </Typography>
      </DialogTitle>
      <DialogContent>
        <Typography variant="overline">{t("common.note")}</Typography>
        <Typography variant="body1" color="primary.main" sx={{ marginBottom: 2 }}>
          {folder?.projectFolderDescription ? folder.projectFolderDescription : "-"}
        </Typography>
        <Typography variant="overline">{t("form.createdAt")}</Typography>
        <Typography variant="body1" color="primary.main" sx={{ marginBottom: 2 }}>
          {formatTimestamp(folder?.createdTime, true)}
        </Typography>
        <Typography variant="overline">{t("form.lastModified")}</Typography>
        <Typography variant="body1" color="primary.main" sx={{ marginBottom: 2 }}>
          {formatTimestamp(folder?.createdTime, true)}
        </Typography>
        <Typography variant="overline">{t("form.createdBy")}</Typography>
        <Typography variant="body1" color="primary.main" sx={{ marginBottom: 2 }}>
          {folder?.createdBy}
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
