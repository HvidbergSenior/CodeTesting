import { ReactNode } from "react";
import { CardContentStyled, CardStyled, NothingToShowTypographyStyled } from "./card-styling";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

type Props = {
  height: string;
  children: ReactNode;
  showContent: boolean;
  noContentText: string;
  isLoading?: boolean;
  hasChildPadding: boolean;
};

export const ContentOnlyCard = (props: Props) => {
  const { height, children, showContent, noContentText, isLoading, hasChildPadding } = props;

  const { screenSize } = useScreenSize();
  const padAll = screenSize === ScreenSizeEnum.Mobile ? "10px 20px" : "30px 40px";

  return (
    <CardStyled variant="outlined" sx={{ height: height, width: "100%" }}>
      {showContent && !isLoading && hasChildPadding && <CardContentStyled sx={{ padding: padAll }}>{children}</CardContentStyled>}
      {showContent && !isLoading && !hasChildPadding && children}
      {isLoading && showContent && (
        <CardContentStyled sx={{ display: "flex", justifyContent: "center", padding: padAll }}>
          <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
            <CircularProgress size={60} />
          </Box>
        </CardContentStyled>
      )}
      {!showContent && (
        <CardContentStyled sx={{ padding: padAll }}>
          <NothingToShowTypographyStyled variant="body1" color="grey.100">
            {noContentText}
          </NothingToShowTypographyStyled>
        </CardContentStyled>
      )}
    </CardStyled>
  );
};
