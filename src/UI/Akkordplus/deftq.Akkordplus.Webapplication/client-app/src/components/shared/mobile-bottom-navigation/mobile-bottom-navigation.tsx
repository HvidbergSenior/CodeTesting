import { Box, Typography } from "@mui/material";
import { NavLink, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

import HomeOutlinedIcon from "@mui/icons-material/HomeOutlined";
import WorkOutlineOutlinedIcon from "@mui/icons-material/WorkOutlineOutlined";
import CalendarMonthOutlinedIcon from "@mui/icons-material/CalendarMonthOutlined";
import { ReactNode } from "react";
import TextSnippetOutlinedIcon from "@mui/icons-material/TextSnippetOutlined";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { costumPalette } from "theme/palette";

type Link = {
  id: number;
  path: string;
  text: string;
  icon: ReactNode;
};

export const MobileBottomNavigation = () => {
  const { t } = useTranslation();
  const { projectId } = useParams<"projectId">();
  const isOnline = useOnlineStatus();

  let links: Link[] = [];

  if (projectId) {
    links = [
      {
        id: 1,
        path: projectId + "/dashboard",
        text: t("mobile.navigation.home"),
        icon: <HomeOutlinedIcon />,
      },
      {
        id: 2,
        path: projectId,
        text: t("mobile.navigation.piecework"),
        icon: <WorkOutlineOutlinedIcon />,
      },
      {
        id: 3,
        path: projectId + "/logbook",
        text: t("mobile.navigation.logbook"),
        icon: <CalendarMonthOutlinedIcon />,
      },
      {
        id: 4,
        path: projectId + "/draft",
        text: t("mobile.navigation.draft"),
        icon: <TextSnippetOutlinedIcon />,
      },
    ];
  }

  return (
    <Box
      sx={{
        position: "fixed",
        top: "auto",
        bottom: 0,
        left: 0,
        width: "100%",
        height: "60px",
        display: "flex",
        justifyContent: "space-around",
        backgroundColor: "primary.dark",
        pt: "10px",
      }}
    >
      {projectId &&
        links.map((link) => {
          return isOnline ? (
            <NavLink key={link.id} to={link.path} style={{ textDecoration: "none" }}>
              <Box
                sx={{
                  display: "flex",
                  flexDirection: "column",
                  alignItems: "center",
                  color: costumPalette.white,
                  opacity: "1",
                }}
              >
                {link.icon}
                <Typography variant="caption">{link.text}</Typography>
              </Box>
            </NavLink>
          ) : (
            <Box
              key={link.id}
              sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                color: costumPalette.white,
                opacity: link.id === 4 ? "1" : "0.5",
              }}
            >
              {link.icon}
              <Typography variant="caption">{link.text}</Typography>
            </Box>
          );
        })}
    </Box>
  );
};
