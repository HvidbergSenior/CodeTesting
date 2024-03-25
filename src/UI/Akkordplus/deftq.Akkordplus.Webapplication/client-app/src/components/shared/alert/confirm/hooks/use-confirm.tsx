import { useDialog } from "shared/dialog/use-dialog";
import { ConfirmToggle } from "../confirm";

interface Props {
  submit: () => void;
  title: string;
  description: string;
  submitButtonLabel?: string;
}
export const useConfirm = ({ submit, title, description, submitButtonLabel }: Props) => {
  const [openConfirmToggleDialog, closeConfirmToggleDialog] = useDialog(ConfirmToggle);

  const handleSubmit = () => {
    submit();
    closeConfirmToggleDialog();
  };

  const handleClick = () => {
    openConfirmToggleDialog({ onSubmit: handleSubmit, title, description, submitButtonLabel });
  };

  return handleClick;
};
