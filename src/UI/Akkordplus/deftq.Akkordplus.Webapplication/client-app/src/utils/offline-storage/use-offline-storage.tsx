import { useTranslation } from "react-i18next";
import { api } from "api/enhancedEndpoints";
import {
  FavoritesResponse,
  FoundMaterial,
  FoundOperation,
  MaterialMountingResponse,
  SupplementResponse,
  WorkItemType,
  MaterialSupplementRequest,
} from "api/generatedApi";
import { useToast } from "shared/toast/hooks/use-toast";
import { useRef } from "react";
import { ExtendedMaterialSupplementOperationRequest } from "components/page/folder/content/measurements/create/hooks/use-map-work-item";

export interface OfflineFavorit {
  favorit: FavoritesResponse;
  mountings: MaterialMountingResponse[];
}

export interface DraftWorkItem {
  workItemType: WorkItemType;
  workItemAmount: number;
  material?: FoundMaterial;
  operation?: FoundOperation;
  supplements?: MaterialSupplementRequest[];
  supplementTexts?: string[];
  workItemMountingCode?: number;
  workItemMountingText?: string;
  mountingCode?: MaterialMountingResponse;
  supplementOperations?: ExtendedMaterialSupplementOperationRequest[];
  draftId: string;
  note?: string;
}

export function useOfflineStorage() {
  const favoritePrefix = "offline-favorites_";
  const draftPrefix = "offline-drafts_";
  const supplementsParam = "offline-supplements";
  const [getFavoritesTrigger] = api.useLazyGetApiProjectsByProjectIdFavoritesQuery();
  const [getMaterialTrigger] = api.useLazyGetApiCatalogMaterialsByMaterialIdQuery();
  const [getSupplementsTrigger] = api.useLazyGetApiCatalogSupplementsQuery();
  const toast = useToast();
  const toastRef = useRef(toast);
  const { t } = useTranslation();

  const getProjectParam = (): string => {
    return "projectId";
  };

  const getFavoriteParam = (id: string) => {
    return `${favoritePrefix}${id}`;
  };

  const getDraftParam = (id: string): string => {
    return `${draftPrefix}${id}`;
  };

  const getOnlineFavoriteMountings = async (materialId: string): Promise<MaterialMountingResponse[]> => {
    if (!materialId) {
      return [];
    }
    try {
      const mountingRes = await getMaterialTrigger({ materialId }).unwrap();
      return mountingRes.mountings ?? [];
    } catch (error) {
      console.error(error);
      toastRef.current.error(t("project.offline.favorites.getFavoritesError"));
      return [];
    }
  };

  const getMappedOnlineFavorites = async (projectId: string): Promise<OfflineFavorit[]> => {
    if (!projectId) {
      return [];
    }
    try {
      const favoritesRepsonse = await getFavoritesTrigger({ projectId }).unwrap();
      const favorites: OfflineFavorit[] = [];
      if (!favoritesRepsonse?.favorites) {
        return [];
      }
      // For and not forEach, forEach can not handle async calls
      for (var i = 0; i < favoritesRepsonse.favorites.length; i++) {
        const favorit = favoritesRepsonse.favorites[i];
        let mountings: MaterialMountingResponse[] = [];

        if (!favorit) {
          continue;
        }
        if (favorit.catalogType === "Material" && favorit.catalogId) {
          mountings = await getOnlineFavoriteMountings(favorit.catalogId);
        }
        favorites.push({ favorit, mountings });
      }
      return favorites;
    } catch (error) {
      console.error(error);
      toastRef.current.error(t("project.offline.favorites.getFavoritesError"));
      return [];
    }
  };

  const getOfflineProjectId = (): string => {
    return localStorage.getItem(getProjectParam()) ?? "";
  };

  const setOfflineProjectId = (projectId: string) => {
    if (getOfflineProjectId() === projectId) {
      return;
    }
    localStorage.setItem(getProjectParam(), projectId);
  };

  const updateOfflineFavorites = async (projectId: string) => {
    const favorites = await getMappedOnlineFavorites(projectId);
    localStorage.setItem(getFavoriteParam(projectId), JSON.stringify(favorites));

    for (var a in localStorage) {
      if (a.includes(favoritePrefix) && a !== getFavoriteParam(projectId)) {
        localStorage.removeItem(a);
      }
    }
  };

  const updateOfflineSupplements = async () => {
    try {
      const supplements = await getSupplementsTrigger().unwrap();
      localStorage.setItem(supplementsParam, JSON.stringify(supplements.supplements));
    } catch (error) {
      console.error(error);
      toastRef.current.error(t("project.offline.supplements.getSupplementsError"));
    }
  };

  const getOfflineFavorites = (projectId: string): FavoritesResponse[] => {
    const json = localStorage.getItem(getFavoriteParam(projectId));
    if (!json) {
      return [];
    }
    try {
      const list: OfflineFavorit[] = JSON.parse(json);
      return list.map((item) => item.favorit);
    } catch (error) {
      console.error("getOfflineFavorites", error);
      return [];
    }
  };

  const getOfflineMaterialMountings = (projectId: string, catalogId: string): MaterialMountingResponse[] => {
    const json = localStorage.getItem(getFavoriteParam(projectId));
    if (!json) {
      return [];
    }
    try {
      const list: OfflineFavorit[] = JSON.parse(json);
      const foundFavorite = list.find((item) => item.favorit.catalogId === catalogId);
      return foundFavorite?.mountings ?? [];
    } catch (error) {
      console.error("getOfflineMaterialMountings", error);
      return [];
    }
  };

  const getOfflineSupplements = (): SupplementResponse[] => {
    const json = localStorage.getItem(supplementsParam);
    if (!json) {
      return [];
    }
    try {
      const list: SupplementResponse[] = JSON.parse(json);
      return list;
    } catch (error) {
      console.error("getOfflineSupplements", error);
      return [];
    }
  };

  const addOfflineDraftWorkItem = (projectId: string, item: DraftWorkItem) => {
    const list = getOfflineWorkitemDrafts(projectId);
    list.push(item);
    localStorage.setItem(getDraftParam(projectId), JSON.stringify(list));
  };

  const removeOfflineDraftWorkItems = (projectId: string, itemIds: string[]): boolean => {
    const list: DraftWorkItem[] = getOfflineWorkitemDrafts(projectId);
    if (list.length <= 0) {
      return false;
    }
    const filteredList = list.filter((item) => !itemIds.includes(item.draftId));
    localStorage.setItem(getDraftParam(projectId), JSON.stringify(filteredList));
    return true;
  };

  const getOfflineWorkitemDrafts = (projectId: string): DraftWorkItem[] => {
    const json = localStorage.getItem(getDraftParam(projectId));
    if (!json) {
      return [];
    }
    try {
      const list: DraftWorkItem[] = JSON.parse(json);
      return list;
    } catch (error) {
      console.error("getOfflineWorkitemDrafts", error);
      return [];
    }
  };

  return {
    getOfflineProjectId,
    setOfflineProjectId,
    updateOfflineFavorites,
    updateOfflineSupplements,
    getOfflineFavorites,
    getOfflineMaterialMountings,
    getOfflineSupplements,
    addOfflineDraftWorkItem,
    removeOfflineDraftWorkItems,
    getOfflineWorkitemDrafts,
  };
}
