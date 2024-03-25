import { notEmpty } from "./predicates";

describe("predicate function", () => {
  it("notEmpty should return true for values", () => {
    const value = {};

    const result = notEmpty(value);

    expect(result).toBe(true);
  });

  it("notEmpty should return false for null", () => {
    const value = null;

    const result = notEmpty(value);

    expect(result).toBe(false);
  });

  it("notEmpty should return false for undefined", () => {
    const value = undefined;

    const result = notEmpty(value);

    expect(result).toBe(false);
  });
});
