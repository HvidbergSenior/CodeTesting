import { useMsal } from "@azure/msal-react";
import Paper from "@mui/material/Paper";
import LoginImage from "assets/images/login.jpg";
import { OfflineDialogButtonless } from "components/shared/offline-dialog/offline-dialog-buttonless";
import { loginRequest } from "index";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { LoginCardDesktop } from "./login-card/login-card-desktop";
import { LoginCardMobile } from "./login-card/login-card-mobile";

export const Login = () => {
  const { instance } = useMsal();
  const { screenSize } = useScreenSize();
  const isMobile = screenSize === ScreenSizeEnum.Mobile;
  const isOnline = useOnlineStatus();

  const handleLogin = () => {
    const redirectStartPage = `${window.location}projects`;
    instance.loginRedirect({ redirectStartPage, ...loginRequest() });
  };

  return (
    <Paper
      sx={{
        backgroundImage: `url(${LoginImage})`,
        width: "100%",
        height: "100vh",
        backgroundPosition: "center",
        backgroundSize: "cover",
        borderRadius: 0,
      }}
    >
      <OfflineDialogButtonless isOpen={!isOnline} />
      {!isMobile && <LoginCardDesktop handleLogin={handleLogin} />}
      {isMobile && <LoginCardMobile handleLogin={handleLogin} />}
    </Paper>
  );
};
