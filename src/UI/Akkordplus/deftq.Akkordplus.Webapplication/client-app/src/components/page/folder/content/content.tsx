import { useState, useEffect, useRef } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Grid";
import Fab from "@mui/material/Fab";
import Grid from "@mui/material/Grid";
import TimerOutlinedIcon from "@mui/icons-material/TimerOutlined";
import { FavoritesResponse, GetProjectResponse, useGetApiProjectsByProjectIdFavoritesQuery } from "api/generatedApi";
import { ExtendedProjectFolder } from "pages/project/hooks/use-map-tree-to-flat";
import { FolderCalculation } from "./calculation/calculation";
import { FolderOverview } from "./overview/overview";
import { useCreateWorkItem } from "./measurements/create/hooks/use-create-work-item";
import { FolderContentNavigation } from "./overview/navigation/default";
import { useWorkItemRestrictions } from "shared/user-restrictions/use-workitems-restrictions";
import { FolderWorkitems } from "./measurements/table/default";
import { useToast } from "shared/toast/hooks/use-toast";

interface Props {
  folder: ExtendedProjectFolder;
  project: GetProjectResponse;
  dataFlatlist: ExtendedProjectFolder[];
}

export const FolderContent = (props: Props) => {
  const { folder, project, dataFlatlist } = props;
  const { t } = useTranslation();
  const tRef = useRef(t);
  const toast = useToast();
  const toastRef = useRef(toast);
  const { canCreateWorkitem } = useWorkItemRestrictions(project);
  const [page, setPage] = useState<number>(parseInt(JSON.parse(localStorage.getItem("content_page") ?? "0")));
  const [loadedFavorites, setLoadedFavorites] = useState<FavoritesResponse[]>([]);
  const openCreateWorkItemDialog = useCreateWorkItem({ folder, project, favorites: loadedFavorites });
  const { data: favorites, error: favoritesError } = useGetApiProjectsByProjectIdFavoritesQuery(
    { projectId: project.id ?? "" },
    { refetchOnMountOrArgChange: true }
  );

  const tabsConfig = [
    {
      name: t("content.overview.title"),
      content: <FolderOverview folder={folder} project={project} />,
    },
    {
      name: t("content.measurements.title"),
      content: <FolderWorkitems folder={folder} project={project} dataFlatlist={dataFlatlist} />,
    },
    {
      name: t("content.calculation.title"),
      content: <FolderCalculation folder={folder} project={project} />,
    },
  ];

  useEffect(() => {
    if (favorites) {
      setLoadedFavorites(favorites?.favorites ?? []);
    }
    if (favoritesError) {
      toastRef.current.error(tRef.current("dashboard.favorits.getFavorites.error"));
    }
  }, [favorites, favoritesError, toastRef, tRef]);

  const changeTab = (tabId: number) => {
    localStorage.setItem("content_page", tabId.toString());
    setPage(tabId);
  };

  const openCreateWorkItem = () => {
    openCreateWorkItemDialog();
    setPage(1);
  };

  return (
    <Grid item xs={true} sx={{ height: "100%", overflow: "auto" }}>
      <Box sx={{ backgroundColor: "background", height: "100%", display: "flex", flexDirection: "column", overflow: "hidden" }}>
        <FolderContentNavigation folder={folder} project={project} preSelectedPage={page} changeTabProps={(tabId) => changeTab(tabId)} />
        {tabsConfig.map(
          (tab, index) =>
            page === index && (
              <Box
                sx={{ p: 0, pb: 1, position: "relative", flex: 1, overflowY: "hidden", display: "flex", flexDirection: "column", height: "100%" }}
                key={index}
              >
                <Box sx={{ p: 2, overflow: "auto", height: "100%" }}>
                  {tab.content}
                  {canCreateWorkitem(folder) && (
                    <Fab sx={{ position: "absolute", bottom: "30px", right: "10px", backgroundColor: "primary.dark" }} onClick={openCreateWorkItem}>
                      <TimerOutlinedIcon fontSize="large" sx={{ color: "grey.100", pb: "2px" }} />
                    </Fab>
                  )}
                </Box>
              </Box>
            )
        )}
      </Box>
    </Grid>
  );
};
