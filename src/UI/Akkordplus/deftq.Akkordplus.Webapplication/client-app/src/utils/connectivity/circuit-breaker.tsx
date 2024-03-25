import React, { useEffect, useState } from "react";
import { useCheckConnection } from "./check-connection";

enum CircuitBreakerState {
  CLOSED = "closed", // We are offline
  OPEN = "open", // We are online
  HALF_OPEN = "halfOpen", // We are trying to reestablish connection
}

interface CircuitBreakerProps {
  children: React.ReactNode;
  initialDelay: number;
  errorThreshold: number;
}

export const CircuitBreakerContext = React.createContext(true);

export const CircuitBreaker: React.FC<CircuitBreakerProps> = ({ children, initialDelay, errorThreshold }: CircuitBreakerProps) => {
  const [state, setState] = useState<CircuitBreakerState>(CircuitBreakerState.CLOSED);
  const [errorCount, setErrorCount] = useState<number>(0);
  const [onlineStatus, setOnlineStatus] = useState<boolean>(true);
  const [delay, setDelay] = useState<number>(initialDelay);
  const { checkConnection } = useCheckConnection();
  const [locked, setLocked] = useState<boolean>(false);

  const checkStatus = async () => {
    if (locked) {
      return;
    }
    setLocked(true);

    try {
      const online = await checkConnection();
      if (!online) {
        if (errorCount === 0) {
          // Decrease interval time after the first unsuccessful connection attempt
          setDelay(1000);
        }
        setErrorCount((count) => count + 1);
      } else {
        if (errorCount > 0) {
          // Reset interval time after a successful reconnection attempt
          setDelay(initialDelay);
        }
        setErrorCount(0);
      }
      setOnlineStatus(online);
    } catch {
      console.error("Error fetching heartbeat from backend");
    } finally {
      setLocked(false);
    }
  };

  useEffect(() => {
    const interval = setInterval(() => {
      checkStatus();
    }, delay);

    return () => {
      clearInterval(interval);
    };
  }, [delay, errorCount]);

  useEffect(() => {
    let timeoutIdResetConnection: ReturnType<typeof setTimeout>;

    if (state === CircuitBreakerState.HALF_OPEN) {
      // if the circuit breaker is in a half-open state, check if the connection is working
      if (onlineStatus) {
        // if the connection is working, set the state to CLOSED and reset the error count
        setState(CircuitBreakerState.CLOSED);
        setErrorCount(0);
      } else {
        // if the connection is not working, set the state to OPEN
        setState(CircuitBreakerState.OPEN);
      }
    } else if (state === CircuitBreakerState.CLOSED) {
      // if the circuit breaker is in a closed state, set a timeout to check the connection
      if (errorCount >= errorThreshold) {
        // if the error count exceeds the threshold, set the state to OPEN
        setState(CircuitBreakerState.OPEN);
        setDelay(5000);
      }
    } else if (state === CircuitBreakerState.OPEN) {
      // if the circuit breaker is in an open state, set a timeout to try to reset the circuit breaker
      timeoutIdResetConnection = setTimeout(() => {
        setState(CircuitBreakerState.HALF_OPEN);
      }, initialDelay);
    }

    return () => {
      clearTimeout(timeoutIdResetConnection);
    };
  }, [state, initialDelay, errorCount, errorThreshold, onlineStatus, delay]);

  return <CircuitBreakerContext.Provider value={state === CircuitBreakerState.CLOSED}>{children}</CircuitBreakerContext.Provider>;
};
