import { OrderDirection } from "types";
import { formatDateToSortingShortDate, shortDateToDate } from "./formats";

export const sortCompareString = (direction: OrderDirection, a?: string | null, b?: string | null): number => {
  if ((!a && !b) || (a === "" && b === "")) return 0;
  if (!a || a === "") {
    return sortAddDirection(direction, -1);
  }
  if (!b || b === "") {
    return sortAddDirection(direction, 1);
  }
  const res = a > b ? 1 : -1;
  return sortAddDirection(direction, res);
};

export const sortCompareNumber = (direction: OrderDirection, a?: number | null, b?: number | null): number => {
  if ((a === undefined && b === undefined) || (a === null && b === null)) return 0;
  if (a === undefined || a === null) {
    return sortAddDirection(direction, -1);
  }
  if (b === undefined || b === null) {
    return sortAddDirection(direction, 1);
  }
  const res = a > b ? 1 : -1;
  return sortAddDirection(direction, res);
};

export const sortCompareDate = (direction: OrderDirection, a?: Date | null, b?: Date | null): number => {
  if ((a === undefined && b === undefined) || (a === null && b === null)) return 0;
  if (a === undefined || a === null) {
    return sortAddDirection(direction, -1);
  }
  if (b === undefined || b === null) {
    return sortAddDirection(direction, 1);
  }
  var dateA = formatDateToSortingShortDate(a);
  var dateB = formatDateToSortingShortDate(b);
  const res = dateA > dateB ? 1 : -1;
  return sortAddDirection(direction, res);
}

export const sortCompareDateString = (direction: OrderDirection, a?: string | null, b?: string | null): number => {
  if ((a === undefined && b === undefined) || (a === null && b === null) || (a === "" && b === "")) return 0;
  if (a === undefined || a === null || a === "") {
    return sortAddDirection(direction, -1);
  }
  if (b === undefined || b === null || b === "") {
    return sortAddDirection(direction, 1);
  }
  var dateA = formatDateToSortingShortDate(shortDateToDate(a));
  var dateB = formatDateToSortingShortDate(shortDateToDate(b));
  const res = dateA > dateB ? 1 : -1;
  return sortAddDirection(direction, res);
}

export const sortAddDirection = (direction: OrderDirection, value: number): number => {
  return direction === "desc" ? value : value * -1;
}
