import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import { Header } from "components/shared/header/header";
import { MobileBottomNavigation } from "components/shared/mobile-bottom-navigation/mobile-bottom-navigation";
import { Outlet, useLocation } from "react-router-dom";
import { ErrorBoundary } from "shared/error-boundary";

export const MobileLayout = () => {
  const location = useLocation();

  return (
    <Box sx={{ display: "flex", flexDirection: "column", height: "100vh", overflow: "hidden" }}>
      <Header />
      <Box sx={{ flex: "1", display: "flex", overflowY: "auto" }}>
        <Box sx={{ width: "100%", height: "100%", padding: 0, paddingBottom: 5, overflow: "hidden" }}>
          <Grid container direction="column" height="100%">
            <ErrorBoundary key={location.pathname}>
              <Outlet />
            </ErrorBoundary>
          </Grid>
          <MobileBottomNavigation />
        </Box>
      </Box>
    </Box>
  );
};
