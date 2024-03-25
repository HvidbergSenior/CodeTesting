import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import { Header } from "components/shared/header/header";
import { Outlet, useLocation } from "react-router-dom";
import { ErrorBoundary } from "shared/error-boundary";

export const DesktopLayout = () => {
  const location = useLocation();

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        height: "100vh",
        overflow: "hidden",
      }}
    >
      <Header />
      <Box sx={{ flex: "1", display: "flex", overflow: "hidden" }}>
        <Box
          sx={{
            width: "100%",
            height: "100%",
            overflow: "hidden",
          }}
        >
          <Grid container direction="column" sx={{ height: "100%" }}>
            <ErrorBoundary key={location.pathname}>
              <Outlet />
            </ErrorBoundary>
          </Grid>
        </Box>
      </Box>
    </Box>
  );
};
