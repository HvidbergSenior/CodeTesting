import type { ApiPaths } from "types";

export const PUBLIC_URL: string = process.env.NODE_ENV === "development" ? "https://localhost:3000/" : "";
export const API_PATH: string = process.env.NODE_ENV === "development" ? "https://localhost:5001/" : "";

function removeTrailingSlash(text: string) {
  return text.replace(/\/+$/, "");
}

export const makePath = (subpath: ApiPaths) => `${removeTrailingSlash(API_PATH)}${subpath}`;
export const makeFullPath = (subpath: string) =>
  process.env.NODE_ENV === "development" ? `${removeTrailingSlash(API_PATH)}${subpath}` : `${removeTrailingSlash(window.location.origin)}${subpath}`;
