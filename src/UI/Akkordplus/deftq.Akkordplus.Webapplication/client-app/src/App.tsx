import CssBaseline from "@mui/material/CssBaseline";
import { ThemeProvider } from "@mui/material/styles";
import { Provider } from "react-redux";
import { BrowserRouter } from "react-router-dom";
import { AppRoutes } from "shared/app-routes";
import { DialogProvider } from "shared/dialog/dialog-provider";
import { store } from "store/store";
import { theme } from "theme/theme";
import { getPCA } from "index";
import { MsalProvider } from "@azure/msal-react";
import { ToastProvider } from "shared/toast/provider";
import { AuthenticatedTemplate, UnauthenticatedTemplate } from "@azure/msal-react";
import { Login } from "pages/Login";
import { CircuitBreaker } from "utils/connectivity/circuit-breaker";

export const App = () => {
  return (
    <Provider store={store}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <BrowserRouter>
          <CircuitBreaker errorThreshold={5} initialDelay={5000}>
            <ToastProvider>
              <DialogProvider>
                <MsalProvider instance={getPCA()}>
                  <AuthenticatedTemplate>
                    <AppRoutes />
                  </AuthenticatedTemplate>
                  <UnauthenticatedTemplate>
                    <Login />
                  </UnauthenticatedTemplate>
                </MsalProvider>
              </DialogProvider>
            </ToastProvider>
          </CircuitBreaker>
        </BrowserRouter>
      </ThemeProvider>
    </Provider>
  );
};
