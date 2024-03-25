import type { ComponentType } from "react";
import { createContext } from "react";
import type { DialogBaseProps } from "./types";

export const DialogContext = createContext<{
  showDialog: <P extends DialogBaseProps>(key: string, component: ComponentType<P>, componentProps: P) => void;
  hideDialog: (key: string) => void;
} | null>(null);
