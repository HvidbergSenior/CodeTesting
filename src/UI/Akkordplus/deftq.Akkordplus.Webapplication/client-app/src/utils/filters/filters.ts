import { notEmpty } from "utils/predicates/predicates";

export function list<T>(items: Array<T | null | undefined> | null | undefined): Array<T> {
  if (!items) {
    return [];
  }

  return items.filter(notEmpty);
}
