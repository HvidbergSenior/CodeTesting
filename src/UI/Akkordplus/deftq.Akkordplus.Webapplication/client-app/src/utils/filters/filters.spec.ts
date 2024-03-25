import { list } from "./filters";

describe("list filter", () => {
  it("should return empty array for null", () => {
    const nullArray = null;

    const result = list(nullArray);

    expect(result.length).toBe(0);
  });

  it("should return empty array for undefined", () => {
    const undefinedArray = undefined;

    const result = list(undefinedArray);

    expect(result.length).toBe(0);
  });

  it("should remove empty entries", () => {
    const mixedArray = ["a", null, 2, undefined, false];

    const result = list(mixedArray);

    expect(result.length).toBe(3);
    expect(result[0]).toBe("a");
    expect(result[1]).toBe(2);
    expect(result[2]).toBe(false);
  });
});
