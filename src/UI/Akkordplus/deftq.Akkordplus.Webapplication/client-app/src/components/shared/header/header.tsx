import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Tooltip from "@mui/material/Tooltip";
import SensorsIcon from "@mui/icons-material/Sensors";
import SensorsOffIcon from "@mui/icons-material/SensorsOff";
import { ProjectSelectMenu } from "components/base/project-menu/project-menu";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { costumPalette } from "theme/palette";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";

export const Header = () => {
  const { screenSize } = useScreenSize();
  const isOnline = useOnlineStatus();
  const { t } = useTranslation();
  const headerHeight = screenSize === ScreenSizeEnum.Mobile ? "50px" : "80px";

  return (
    <AppBar
      position="static"
      sx={{
        borderRadius: 0,
        backgroundColor: "primary.100",
        boxShadow: "none",
        height: headerHeight,
      }}
    >
      <Toolbar
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          flex: 1,
        }}
      >
        <Box
          sx={{
            flex: 1,
            display: "flex",
            justifyContent: "left",
          }}
        >
          <Box
            component="img"
            sx={{
              width: screenSize === ScreenSizeEnum.Mobile ? "calc(160px * .6)" : "140px",
              height: screenSize === ScreenSizeEnum.Mobile ? "calc(33px * .6)" : "28px",
            }}
            alt="Logo"
            src="/logo.png"
          />
        </Box>
        <Box display={"flex"} alignItems={"center"}>
          {isOnline ? (
            <Tooltip title={t("project.offline.onlineSignal")}>
              <SensorsIcon sx={{ color: costumPalette.white, marginRight: "30px" }} fontSize={"large"} />
            </Tooltip>
          ) : (
            <Tooltip title={t("project.offline.offlineSignal")}>
              <SensorsOffIcon sx={{ color: costumPalette.white, marginRight: "30px" }} fontSize={"large"} />
            </Tooltip>
          )}
          <ProjectSelectMenu />
        </Box>
      </Toolbar>
    </AppBar>
  );
};
