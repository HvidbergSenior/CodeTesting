import { useScreenSize, ScreenSizeEnum } from "shared/use-screen-size";
import { DesktopLayout } from "./desktop-layout";
import { MobileLayout } from "./mobile-layout";

export const DefaultLayout = () => {
  const { screenSize } = useScreenSize();
  return screenSize === ScreenSizeEnum.Mobile ? <MobileLayout /> : <DesktopLayout />;
};
