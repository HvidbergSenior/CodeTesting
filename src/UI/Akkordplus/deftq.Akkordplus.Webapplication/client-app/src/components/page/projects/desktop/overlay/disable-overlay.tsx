import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid";
import { useTranslation } from "react-i18next";
import TextSnippetOutlinedIcon from "@mui/icons-material/TextSnippetOutlined";
import { useEffect, useLayoutEffect, useState } from "react";
import { RouteSubPage } from "shared/route-types";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";

interface Props {
  selectDraft: (page: RouteSubPage) => void;
}

export const OverlaySideMenu = ({ selectDraft }: Props) => {
  const [widthScaling, setWidthScaling] = useState("20.8%");
  const width = useWindowSize();
  const isOnline = useOnlineStatus();
  const { t } = useTranslation();

  useEffect(() => {
    // the numbers corresponds to when the grid item changes in size, and then percentage is what the overlay must cover to fit the grid item
    const scale = width < 1200 ? "33.4%" : width < 1536 ? "25%" : "20.8%";
    setWidthScaling(scale);
  }, [width]);

  function useWindowSize() {
    const [size, setSize] = useState(0);
    useLayoutEffect(() => {
      function updateSize() {
        setSize(window.innerWidth);
      }
      window.addEventListener("resize", updateSize);
      updateSize();
      return () => window.removeEventListener("resize", updateSize);
    }, []);
    return size;
  }

  return (
    <div>
      {!isOnline && (
        <Grid
          item
          sx={{
            position: "absolute",
            backgroundColor: "#485577",
            zIndex: "10",
            top: 80,
            left: 0,
            height: "calc(100vh - 80px)",
            width: widthScaling,
            opacity: "0.5",
          }}
        ></Grid>
      )}

      {!isOnline && (
        <Box
          onClick={() => selectDraft("draft")}
          sx={{
            display: "flex",
            paddingLeft: "15px",
            alignItems: "center",
            marginBottom: 2,
            cursor: "pointer",
            backgroundColor: "primary.light",
            position: "absolute",
            top: 187,
            left: 0,
            zIndex: "11",
            width: widthScaling,
            height: "40px",
          }}
        >
          <TextSnippetOutlinedIcon color="primary" />
          <Typography variant="h6" color="primary.main" sx={{ paddingLeft: "10px" }}>
            {t("draft.title")}
          </Typography>
        </Box>
      )}
    </div>
  );
};
