import { useTranslation } from "react-i18next";
import { ReactNode } from "react";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Typography from "@mui/material/Typography";
import { CardContentStyled, CardHeaderStyled, CardStyled, NothingToShowTypographyStyled } from "./card-styling";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";
import { costumPalette } from "theme/palette";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";
import { auto } from "@popperjs/core";

type Props = {
  titleNamespace: string;
  height: string;
  children: ReactNode;
  showContent: boolean;
  noContentText: string;
  isLoading?: boolean;
  description: string | undefined;
  showDescription: boolean;
  backgroundColor?: string | null;
  showHeaderAction: boolean;
  headerActionClickedProps: () => void;
  hasBorder?: boolean;
  fixedWidth?: string;
};

export const CardWithHeader = (props: Props) => {
  const {
    titleNamespace,
    height,
    showHeaderAction,
    headerActionClickedProps,
    children,
    backgroundColor,
    showContent,
    noContentText,
    isLoading,
    description,
    showDescription,
    fixedWidth,
  } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();

  const padLeft = screenSize === ScreenSizeEnum.Mobile ? "20px" : "40px";
  const padRight = screenSize === ScreenSizeEnum.Mobile ? "20px" : "40px";
  const padTop = screenSize === ScreenSizeEnum.Mobile ? "20px" : "30px";
  const padBottom = screenSize === ScreenSizeEnum.Mobile ? "20px" : "20px";
  const padAll = screenSize === ScreenSizeEnum.Mobile ? "0px 20px 0px 20px" : "0px 40px 0px 40px";
  const headerActionMenuClicked = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.stopPropagation();
    headerActionClickedProps();
  };

  return (
    <CardStyled
      variant="outlined"
      sx={{
        height: height,
        width: fixedWidth ? fixedWidth : auto,
        border: "1px solid rgba(0, 0, 0, 0.12)",
        backgroundColor: backgroundColor ? `${backgroundColor} !important` : costumPalette.white,
        WebkitPrintColorAdjust: "exact",
      }}
    >
      <CardHeaderStyled
        sx={{
          paddingLeft: padLeft,
          paddingRight: padRight,
          pb: padBottom,
          pt: padTop,
        }}
        titleTypographyProps={{ variant: "overline", color: "primary.main" }}
        title={t(titleNamespace)}
        action={
          showHeaderAction ? (
            <IconButton onClick={(e) => headerActionMenuClicked(e)}>
              <EditIcon color="primary" />
            </IconButton>
          ) : (
            <Box></Box>
          )
        }
      />

      {showDescription && (
        <Typography sx={{ pl: "15px", color: "grey.50" }} variant="body2">
          {description}
        </Typography>
      )}
      {showContent && !isLoading && <CardContentStyled sx={{ padding: padAll, color: costumPalette.green }}>{children}</CardContentStyled>}

      {isLoading && showContent && (
        <CardContentStyled sx={{ display: "flex", justifyContent: "center", padding: "0px" }}>
          <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", pb: 2.5 }}>
            <CircularProgress size={60} />
          </Box>
        </CardContentStyled>
      )}
      {!showContent && (
        <CardContentStyled>
          <NothingToShowTypographyStyled variant="body1" color="grey.100">
            {noContentText}
          </NothingToShowTypographyStyled>
        </CardContentStyled>
      )}
    </CardStyled>
  );
};
