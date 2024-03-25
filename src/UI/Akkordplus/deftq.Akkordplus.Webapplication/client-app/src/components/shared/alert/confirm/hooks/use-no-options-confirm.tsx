import { useDialog } from "shared/dialog/use-dialog";
import { ConfirmNoOptions } from "../no-options-confirm";

interface Props {
  title: string;
  description: string;
}
export const useNoOptionsConfirm = ({ title, description }: Props) => {
  const [openConfirmToggleDialog, closeConfirmToggleDialog] = useDialog(ConfirmNoOptions);

  const handleSubmit = () => {
    closeConfirmToggleDialog();
  };

  const handleClick = () => {
    openConfirmToggleDialog({ onSubmit: handleSubmit, title, description });
  };

  return handleClick;
};
