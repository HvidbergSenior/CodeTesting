import { useContext } from "react";
import { CircuitBreakerContext } from "../circuit-breaker";

export const useOnlineStatus = () => {
  const store = useContext(CircuitBreakerContext);
  return store;
};
