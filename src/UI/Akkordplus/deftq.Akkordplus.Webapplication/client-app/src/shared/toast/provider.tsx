import { FC, ReactNode, useState } from "react";
import { Toast, ToastStyle } from "./toast";
import { ToastContext, ToastMessage } from "./hooks/use-toast";

export const ToastProvider: FC<{ children: ReactNode } & ToastStyle> = ({ children, ...props }) => {
  const [message, setMessage] = useState<ToastMessage | undefined>();
  const [lastMessage, setLastMessage] = useState<number>(Date.now());

  const removeMessage = () => {
    setMessage(undefined);
  };
  const addNewMessage = (msg: ToastMessage) => {
    const now = Date.now();
    if (!!message && msg.message === message.message && msg.severity === message.severity && now - lastMessage < 5000) {
      return;
    }
    setLastMessage(now);
    setMessage(msg);
  };

  return (
    <ToastContext.Provider
      value={{
        addMessage(message) {
          addNewMessage(message);
        },
      }}
    >
      {children}
      {!!message && <Toast key={message.key} message={message} onExited={() => removeMessage()} {...props} />}
    </ToastContext.Provider>
  );
};
