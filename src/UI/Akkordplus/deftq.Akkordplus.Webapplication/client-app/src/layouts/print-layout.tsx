import { Outlet, useLocation } from "react-router-dom";
import { ErrorBoundary } from "shared/error-boundary";

export const PrintLayout = () => {
  const location = useLocation();

  return (
    <ErrorBoundary key={location.pathname}>
      <Outlet />
    </ErrorBoundary>
  );
};
