import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import Tab from "@mui/material/Tab";
import Tabs from "@mui/material/Tabs";
import Typography from "@mui/material/Typography";
import FolderOutlinedIcon from "@mui/icons-material/FolderOutlined";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import type { GetProjectResponse } from "api/generatedApi";
import type { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";

interface Props {
  folder: ExtendedProjectFolder | undefined;
  project: GetProjectResponse;
  preSelectedPage: number;
  changeTabProps: (tabId: number) => void;
}

export const MobileFolderContentNavigation = (props: Props) => {
  const { folder, project, preSelectedPage } = props;
  const { t } = useTranslation();
  const navigate = useNavigate();
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
    <Box sx={{ display: "flex", flexDirection: "column" }}>
      <Box sx={{ display: "flex", paddingBottom: "10px", backgroundColor: "primary.light", pt: "10px" }} onClick={() => navigate(-1)}>
        <IconButton sx={{ p: 0 }}>
          <KeyboardArrowLeftIcon />
        </IconButton>
        <IconButton sx={{ p: 0, pl: "2px" }}>
          <FolderOutlinedIcon color={folder?.folderExtraWork === "ExtraWork" ? "secondary" : "primary"} />
        </IconButton>
        <Typography variant="h6" color={"primary.main"} sx={{ pt: "4px", pl: "10px" }}>
          {folder?.projectFolderName ? folder?.projectFolderName : project?.title}
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
          backgroundColor: "background",
          width: "100%",
        }}
      >
        <Tab label={t("content.overview.title")} sx={{ height: 40 }} />
        <Tab label={t("content.measurements.title")} sx={{ height: 40 }} />
        <Tab label={t("content.calculation.title")} sx={{ height: 40 }} />
      </Tabs>
      <Divider sx={{ height: "50px", boxShadow: "0 1em 1em -1em rgba(0, 0, 0, .25)", mt: "-50px" }} />
    </Box>
  );
};
