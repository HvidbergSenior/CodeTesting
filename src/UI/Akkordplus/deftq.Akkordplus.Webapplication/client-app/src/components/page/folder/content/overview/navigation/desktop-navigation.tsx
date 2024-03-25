import { useTranslation } from "react-i18next";
import { useState, useEffect } from "react";
import Box from "@mui/material/Box";
import Tab from "@mui/material/Tab";
import Tabs from "@mui/material/Tabs";
import Typography from "@mui/material/Typography";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import NotificationsNoneIcon from "@mui/icons-material/NotificationsNone";
import type { GetProjectResponse } from "api/generatedApi";
import type { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { isAllRatesAndSupplementsInherited } from "utils/base-rate/detect-rate-status";
import Tooltip from "@mui/material/Tooltip";

interface Props {
  folder: ExtendedProjectFolder | undefined;
  project: GetProjectResponse;
  preSelectedPage: number;
  changeTabProps: (tabId: number) => void;
}

export const DesktopFolderContentNavigation = (props: Props) => {
  const { folder, project, preSelectedPage } = props;
  const { t } = useTranslation();
  const [selectedTab, setSelectedTab] = useState<number>(preSelectedPage);

  useEffect(() => {
    if (selectedTab !== preSelectedPage) {
      setSelectedTab(preSelectedPage);
    }
  }, [preSelectedPage, selectedTab]);

  const handleChange = (event: React.SyntheticEvent, tabId: number) => {
    setSelectedTab(tabId);
    props.changeTabProps(tabId);
  };

  return (
    <Box sx={{ borderRadius: 0, height: "72px", display: "inline-flex" }}>
      <Box sx={{ width: "25%", pl: "50px", pr: "20px", display: "flex", alignItems: "center" }}>
        <FolderOutlinedIcon color={folder?.folderExtraWork === "ExtraWork" ? "secondary" : "primary"} sx={{ marginRight: 1, marginTop: "-4px" }} />
        <Typography
          data-testid="project-overview-tabs-foldername"
          variant="h6"
          color="primary.main"
          sx={{ whiteSpace: "nowrap", overflow: "hidden", textOverflow: "ellipsis" }}
        >
          {folder?.projectFolderName ? folder?.projectFolderName : project.title}
        </Typography>
      </Box>
      <Tabs
        value={selectedTab}
        onChange={handleChange}
        indicatorColor="secondary"
        TabIndicatorProps={{ sx: { height: 5 } }}
        textColor="primary"
        variant="fullWidth"
        sx={{
          backgroundColor: "primary.light",
          width: "75%",
        }}
      >
        <Tab data-testid="project-overview-tabs-overview" label={t("content.overview.title")} sx={{ height: 72 }} />
        <Tab data-testid="project-overview-tabs-mesurements" label={t("content.measurements.title")} sx={{ height: 72 }} />
        {folder?.isRoot ? (
          <Tab data-testid="project-overview-tabs-calculation" label={t("content.calculation.title")} sx={{ height: 72 }} />
        ) : (
          <Tab
            data-testid="project-overview-tabs-calculation"
            icon={
              <Tooltip title={t("folder.baseRate.notification")}>
                <NotificationsNoneIcon color="secondary" sx={{ opacity: isAllRatesAndSupplementsInherited(folder) ? "0" : "1", marginBottom: 0.5 }} />
              </Tooltip>
            }
            iconPosition={"end"}
            label={t("content.calculation.title")}
            sx={{ height: 72 }}
          />
        )}
      </Tabs>
    </Box>
  );
};
