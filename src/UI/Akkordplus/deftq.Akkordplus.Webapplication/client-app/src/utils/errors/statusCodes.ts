// https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
export function statusCodeToName(statusCode: number | string) {
  switch (statusCode) {
    case 401:
      return "Unauthorized";
    case 403:
      return "Forbidden";
    case 404:
      return "Not found";
    case 409:
      return "Conflict";
    case 429:
      return "Too Many Requests";
    case 500:
      return "Internal Server Error";
    case 503:
      return "Service Unavailable";
  }

  return null;
}
