import { getAccessToken } from "api/baseApi";

export type reportType = "workitems" | "logbook";

export function useDownloadFile() {
  const clickDownloadFile = async (url: string, fileName: string) => {
    const token = await getAccessToken();

    fetch(url, {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }).then((response) => {
      if (!response.ok) {
        console.error(response);
        throw new Error();
      }
      response.blob().then((blob) => {
        let url = window.URL.createObjectURL(blob);
        let a = document.createElement("a");
        a.href = url;
        a.download = fileName;
        a.click();
      });
    });
  };

  return clickDownloadFile;
}
