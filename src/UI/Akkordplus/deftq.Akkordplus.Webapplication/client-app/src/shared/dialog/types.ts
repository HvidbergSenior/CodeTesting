import type { ComponentType } from "react";
import type { Theme } from "@mui/material/styles";
import type { SvgIconProps } from "@mui/material/SvgIcon";

export interface FormDialogProps<T> extends DialogBaseProps {
  onSubmit: (data: T) => Promise<unknown[]>;
}

export interface DialogBaseProps {
  isOpen: boolean;
  onClose?: () => void;
}

export interface Props extends DialogBaseProps {
  title?: string;
  description?: string;
  icon: ComponentType<SvgIconProps>;
  color?: (theme: Theme) => string;
}

export type TDialogState<P extends object> = {
  isOpen: boolean;
  component: ComponentType<P>;
  componentProps: P;
};
