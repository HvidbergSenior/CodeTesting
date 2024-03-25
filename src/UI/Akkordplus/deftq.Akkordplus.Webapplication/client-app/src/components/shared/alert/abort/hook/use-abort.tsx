import { useDialog } from "shared/dialog/use-dialog";
import { AbortDialog } from "../abort";

interface Props {
  closeDialog: () => void;
}
export const useAbortDialog = ({ closeDialog }: Props) => {
  const [openAbortDialog, closeAbortDialog] = useDialog(AbortDialog);

  const handleSubmit = () => {
    closeAbortDialog();
    closeDialog();
  };

  const handleClick = () => {
    openAbortDialog({ onSubmit: () => handleSubmit() });
  };

  return handleClick;
};
