import { useCallback, useState } from "react";

export const useOrder = <T extends string>(initialOrderBy: T) => {
  const [direction, setDirection] = useState<"asc" | "desc">("asc");
  const [orderBy, setOrderBy] = useState<T>(initialOrderBy);

  const onClick = useCallback(
    (orderByKey: T) => (e: any) => {
      setOrderBy(orderByKey);
      if (orderByKey === orderBy) {
        setDirection((currentDirection) => (currentDirection === "asc" ? "desc" : "asc"));
      }
    },
    [orderBy]
  );

  const getLabelProps = useCallback(
    (key: T) => ({
      active: key === orderBy,
      direction,
      onClick: onClick(key),
    }),
    [onClick, orderBy, direction]
  );

  return {
    direction,
    orderBy,
    getLabelProps,
  };
};
