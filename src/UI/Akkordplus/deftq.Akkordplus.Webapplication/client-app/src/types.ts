import type { SerializedError } from "@reduxjs/toolkit";
import type { FetchBaseQueryError } from "@reduxjs/toolkit/dist/query";
import { paths } from "__generated__/api";

export type RTKQueryError = FetchBaseQueryError | SerializedError;
export type OrderDirection = "asc" | "desc";

export type ApiPaths = keyof paths;
