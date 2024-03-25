import type { FetchBaseQueryError } from "@reduxjs/toolkit/dist/query";
import type { ErrorDetail } from "api/generatedApi";
import type { RTKQueryError } from "types";

export function isFetchBaseQueryError(error: RTKQueryError | undefined | unknown): error is FetchBaseQueryError {
  return (error as FetchBaseQueryError).data !== undefined;
}

export function isProblemDetails(error: ErrorDetail | undefined | unknown): error is ErrorDetail {
  if (error === undefined) {
    return false;
  }

  if (typeof error !== "object") {
    return false;
  }

  return (error as ErrorDetail).code !== undefined;
}
