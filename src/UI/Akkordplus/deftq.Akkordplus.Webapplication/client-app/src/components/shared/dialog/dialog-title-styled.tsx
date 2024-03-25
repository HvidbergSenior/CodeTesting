import CloseIcon from "@mui/icons-material/Close";
import DialogTitle from "@mui/material/DialogTitle";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import type { DialogBaseProps } from "shared/dialog/types";
import Divider from "@mui/material/Divider";

interface Props extends DialogBaseProps {
  title: string | undefined;
  description?: string | undefined;
  handleIconClose?: () => void;
  showDivider?: boolean;
  textPaddingHorizontal?: number;
  bottomPadding?: number;
}
const DialogTitleStyled = ({ title, description, textPaddingHorizontal, bottomPadding, onClose, handleIconClose, showDivider }: Props) => {
  const hide = showDivider ?? true;
  const horizontalTextPadding = textPaddingHorizontal ? textPaddingHorizontal : 3;
  return (
    <DialogTitle component="div" textAlign={"left"} sx={{ pl: 0, pr: 0, pb: bottomPadding ?? 3 }}>
      <Typography variant="h5" color="primary.dark" sx={{ pt: 4, pl: horizontalTextPadding, pr: horizontalTextPadding }}>
        {title}
      </Typography>
      {description && (
        <Typography variant="body2" color="grey.50" sx={{ pt: 1, pl: horizontalTextPadding, pr: horizontalTextPadding }}>
          {description}
        </Typography>
      )}
      <IconButton onClick={handleIconClose ? handleIconClose : onClose} sx={{ position: "absolute", top: 5, right: 5 }}>
        <CloseIcon fontSize="medium" />
      </IconButton>
      {hide && <Divider sx={{ pt: 2 }} />}
    </DialogTitle>
  );
};

export default DialogTitleStyled;
