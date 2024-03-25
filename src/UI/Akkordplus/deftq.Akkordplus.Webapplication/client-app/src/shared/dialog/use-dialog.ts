import { ComponentType, useCallback, useContext, useMemo } from "react";
import { DialogContext } from "./dialog-context";
import type { DialogBaseProps } from "./types";

// TODO: Consider replacing IIFE with yield id generator
const generateModalKey = (() => {
  let count = 0;

  return () => `${count++}`;
})();

export function useDialog<P extends DialogBaseProps>(component: ComponentType<P>) {
  const context = useContext(DialogContext);
  const key = useMemo(generateModalKey, []);

  if (context === null) {
    throw Error("You called useDialog hook outside its context");
  }

  const showDialog = useCallback(
    (componentProps: Omit<P, "isOpen">) => {
      context.showDialog(key, component, componentProps as P); // TODO: Fix cast https://stackoverflow.com/a/59363875
    },
    [component, context, key]
  );

  const hideDialog = useCallback(() => {
    context.hideDialog(key);
  }, [context, key]);

  return [showDialog, hideDialog] as const;
}
