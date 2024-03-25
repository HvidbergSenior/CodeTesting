import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { App } from "App";
import * as serviceWorkerRegistration from "./serviceWorkerRegistration";
import "./i18n";
import reportWebVitals from "./reportWebVitals";
import { makePath } from "api/path";
import { AuthenticationResult, Configuration, EventMessage, EventType, PopupRequest, PublicClientApplication } from "@azure/msal-browser";

// Overpass font
import "./assets/fonts/Overpass/Overpass.css";

if (process.env.NODE_ENV === "development" && localStorage.getItem("MockNetwork") === "true") {
  const { worker } = require("./mocks/browser");
  worker.start();
}

async function loadConfig(): Promise<Configuration> {
  return fetch(makePath("/api/config"))
    .then((res) => res.json())
    .then((res) => ({
      auth: {
        clientId: res.azureAdB2C?.clientId ? res.azureAdB2C.clientId : "",
        authority: res.azureAdB2C?.authority ? res.azureAdB2C.authority : "",
        redirectUri: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
        knownAuthorities: res.azureAdB2C?.knownAuthority ? [res.azureAdB2C.knownAuthority] : [],
      },
    }));
}

let configuration: Configuration | null = null;
let pca: PublicClientApplication | null = null;

export const loginRequest = (): PopupRequest => {
  if (configuration) {
    return {
      scopes: [configuration.auth.clientId + " openid"],
    };
  }
  throw Error("Config not loaded");
};

export const getPCA = (): PublicClientApplication => {
  if (pca) {
    return pca;
  }
  throw Error("Config not loaded");
};

loadConfig().then((config) => {
  configuration = config;
  pca = new PublicClientApplication(config);

  const msalInstance = getPCA();
  const accounts = pca.getAllAccounts();
  if (accounts.length > 0) {
    msalInstance.setActiveAccount(accounts[0]);
  }

  msalInstance.addEventCallback((event: EventMessage) => {
    if (event.eventType === EventType.LOGIN_SUCCESS && event.payload) {
      const payload = event.payload as AuthenticationResult;
      const account = payload.account;
      msalInstance.setActiveAccount(account);
    }
  });

  const root = createRoot(document.getElementById("root")!);
  root.render(
    <StrictMode>
      <App />
    </StrictMode>
  );
});

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
// serviceWorkerRegistration.unregister();
serviceWorkerRegistration.register();
// serviceWorkerRegistration.register({
//   onUpdate: (e) => {
//     const {waiting: { postMessage = null} = {} as any, update } = e || {};
//     if (postMessage) {
//       postMessage({ type: 'SKIP_WAITING' });
//     }
//     alert("New opdate er her");
//     update().then(() => {
//       window.location.reload();
//     })
//   }
// });

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
