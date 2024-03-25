import { statusCodeToName } from "./statusCodes";

describe("status codes", () => {
  it("should map status codes to names", () => {
    expect(statusCodeToName(401)).toBe("Unauthorized");
    expect(statusCodeToName(403)).toBe("Forbidden");
    expect(statusCodeToName(404)).toBe("Not found");
    expect(statusCodeToName(409)).toBe("Conflict");
    expect(statusCodeToName(429)).toBe("Too Many Requests");
    expect(statusCodeToName(500)).toBe("Internal Server Error");
    expect(statusCodeToName(503)).toBe("Service Unavailable");

    expect(statusCodeToName("")).toBeNull();
  });
});
