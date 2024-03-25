import { ComponentType, ReactNode, useCallback, useMemo, useState } from "react";
import { DialogContext } from "./dialog-context";
import type { TDialogState } from "./types";

export const DialogProvider = ({ children }: { children: ReactNode }) => {
  const [modalsConfig, setConfig] = useState<Record<string, TDialogState<any>>>({});

  const hideDialog = useCallback(
    (modalKey: string) => {
      setConfig((prevConfig) => ({
        ...prevConfig,
        [modalKey]: {
          ...prevConfig[modalKey],
          isOpen: false,
        },
      }));
    },
    [setConfig]
  );

  const showDialog = useCallback(
    function <P extends object>(modalKey: string, component: ComponentType<P>, modalData: P) {
      setConfig((prevConfig) => ({
        ...prevConfig,
        [modalKey]: {
          isOpen: true,
          component,
          componentProps: modalData,
        },
      }));
    },
    [setConfig]
  );

  const contextValue = useMemo(
    () => ({
      showDialog,
      hideDialog,
    }),
    [hideDialog, showDialog]
  );

  return (
    <DialogContext.Provider value={contextValue}>
      {children}
      {Object.keys(modalsConfig).map((modalKey) => {
        const { component: Component, isOpen, componentProps } = modalsConfig[modalKey];

        return isOpen && <Component onClose={() => hideDialog(modalKey)} key={modalKey} isOpen={isOpen} {...componentProps} />;
      })}
    </DialogContext.Provider>
  );
};
