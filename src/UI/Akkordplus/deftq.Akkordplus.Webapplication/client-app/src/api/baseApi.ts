import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { API_PATH } from "api/path";
import { loginRequest } from "index";
import { getPCA } from "index";

export const getAccessToken = async () => {
  const msalInstance = getPCA();
  const account = msalInstance.getActiveAccount();
  if (!account) {
    throw Error("No active account! Verify a user has been signed in and setActiveAccount has been called.");
  }
  const getToken = await msalInstance.acquireTokenSilent({
    ...loginRequest(),
    account: account,
  });

  return getToken.accessToken;
};

export const baseApi = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: API_PATH,
    prepareHeaders: async (headers) => {
      const token = await getAccessToken();
      headers.set("Authorization", `Bearer ${token}`);
      return headers;
    },
  }),
  endpoints: () => ({}),
});
