import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { DesktopOneStepBackNavigation } from "./desktop-navigation";
import { MobileOneStepBackNavigation } from "./mobile-navigation";
import Box from "@mui/material/Box";

interface Props {
  from: string;
  here: string;
}

export const DefaultOneStepBackNavigation = (props: Props) => {
  const { from, here } = props;
  const { screenSize } = useScreenSize();

  return (
    <Box displayPrint="none">
      {screenSize === ScreenSizeEnum.Mobile ? <MobileOneStepBackNavigation from={from} /> : <DesktopOneStepBackNavigation from={from} here={here} />}
    </Box>
  );
};
