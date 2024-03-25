import Alert from "@mui/material/Alert";
import Snackbar, { SnackbarProps } from "@mui/material/Snackbar";
import IconButton from "@mui/material/IconButton";
import CloseIcon from "@mui/icons-material/Close";
import { FC } from "react";
import { ToastMessage } from "./hooks/use-toast";

export type ToastStyle = Omit<SnackbarProps, "TransitionProps" | "onClose" | "open" | "children" | "message">;

export type ToastProps = {
  message: ToastMessage;
  onExited: () => void;
} & ToastStyle;

// https://mui.com/material-ui/react-snackbar/#consecutive-snackbars
export const Toast: FC<ToastProps> = ({ message, onExited, autoHideDuration, ...props }) => {
  const closeAction = (
    <IconButton size="small" color="inherit" onClick={onExited}>
      <CloseIcon fontSize="small" />
    </IconButton>
  );

  return (
    <Snackbar
      key={message.key}
      open={true}
      onClose={onExited}
      TransitionProps={{ onExited }}
      anchorOrigin={{ vertical: "bottom", horizontal: "left" }}
      autoHideDuration={autoHideDuration ?? 8000}
      {...props}
    >
      <Alert severity={message.severity} action={closeAction}>
        {message.message}
      </Alert>
    </Snackbar>
  );
};
