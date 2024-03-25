import { makePath } from "api/path";

export function useCheckConnection() {
  const checkConnection = async (): Promise<boolean> => {
    if (navigator.onLine) {
      return await fetch(makePath("/api/ping"))
        .then((data) => true)
        .catch((error) => false);
    }
    return Promise.resolve(false);
  };

  return { checkConnection };
}
