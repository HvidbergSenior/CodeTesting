import { useEffect, useRef } from "react";
import { useOnlineStatus } from "utils/connectivity/hook/use-online-status";
import { useOfflineStorage } from "utils/offline-storage/use-offline-storage";

export function useUpdateOfflineData() {
  const isOnline = useOnlineStatus();
  const isOnlineRef = useRef(isOnline);
  const { updateOfflineFavorites, updateOfflineSupplements, getOfflineProjectId } = useOfflineStorage();
  const updateOfflineFavoritesRef = useRef(updateOfflineFavorites);
  const updateOfflineSupplementsRef = useRef(updateOfflineSupplements);
  const getOfflineProjectIdRef = useRef(getOfflineProjectId);

  useEffect(() => {
    let intervalId: any;
    const interval5m = 5 * 60 * 1000;
    const interval10s = 10 * 1000;

    const projectId = getOfflineProjectIdRef.current();
    if (projectId) {
      if (isOnlineRef.current) {
        updateOfflineFavoritesRef.current(projectId);
        updateOfflineSupplementsRef.current();
      }
      intervalId = setInterval(
        () => {
          if (isOnlineRef.current) {
            updateOfflineFavoritesRef.current(projectId);
            updateOfflineSupplementsRef.current();
          }
        },
        isOnlineRef.current ? interval5m : interval10s
      );
    }
    return () => {
      clearInterval(intervalId);
    };
  }, [isOnlineRef, updateOfflineFavoritesRef, getOfflineProjectIdRef]);
}
