import { useState, useEffect } from "react";

export enum ScreenSizeEnum {
  LargeDesktop,
  Desktop,
  Mobile,
}

export const useScreenSize = () => {
  const useViewportBreakPointToDesktop = 600;
  const useViewportBreakPointToLargeDesktop = 1200;

  const getSize = (): ScreenSizeEnum => {
    if (window.innerWidth > useViewportBreakPointToLargeDesktop) {
      return ScreenSizeEnum.LargeDesktop;
    }
    if (window.innerWidth > useViewportBreakPointToDesktop) {
      return ScreenSizeEnum.Desktop;
    }
    return ScreenSizeEnum.Mobile;
  };

  const [screenSize, setScreenSize] = useState(getSize());

  useEffect(() => {
    const handleWindowResize = () => setScreenSize(getSize());
    window.addEventListener("resize", handleWindowResize);
    return () => window.removeEventListener("resize", handleWindowResize);
  });

  return { screenSize };
};
