import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

export function usePaperProps() {
  const { screenSize } = useScreenSize();

  const paperProps = {
    sx: {
      minHeight: screenSize === ScreenSizeEnum.Mobile ? "70%" : "550px", // mobile is ignored
      margin: screenSize === ScreenSizeEnum.Mobile ? "32px 7px" : "32px",
      width: screenSize === ScreenSizeEnum.Mobile ? "100%" : "calc(100%-164px)",
    },
  };

  return { paperProps };
}
