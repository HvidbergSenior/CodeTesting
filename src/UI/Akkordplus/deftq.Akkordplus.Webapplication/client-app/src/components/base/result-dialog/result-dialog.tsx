import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import { Props } from "shared/dialog/types";

export function ResultDialog(props: Props) {
  const Icon = props.icon;

  return (
    <Dialog fullWidth maxWidth="sm" open={props.isOpen} onClose={props.onClose}>
      <DialogContent
        sx={{
          margin: (theme) => theme.spacing(2),
          marginBottom: (theme) => theme.spacing(4),
        }}
      >
        <Grid container flexDirection="column" alignItems="center">
          <Icon
            sx={{
              color: props.color,
              fontSize: (theme) => theme.spacing(10),
              margin: (theme) => theme.spacing(2),
            }}
          />
          {props.title && (
            <Typography variant="h4" textAlign="center" gutterBottom>
              {props.title}
            </Typography>
          )}
          {props.description && (
            <Typography textAlign="center" gutterBottom>
              {props.description}
            </Typography>
          )}
        </Grid>
      </DialogContent>
    </Dialog>
  );
}
