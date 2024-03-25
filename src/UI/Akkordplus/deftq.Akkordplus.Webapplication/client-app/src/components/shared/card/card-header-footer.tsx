import { useTranslation } from "react-i18next";
import EditIcon from "@mui/icons-material/Edit";
import { ReactElement, ReactNode } from "react";
import {
  CardActionsRightStyled,
  CardActionsCenterStyled,
  CardContentStyled,
  CardHeaderStyled,
  CardStyled,
  NothingToShowTypographyStyled,
} from "./card-styling";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Button from "@mui/material/Button";
import { ScreenSizeEnum, useScreenSize } from "shared/use-screen-size";

export type BottomActionTypes = "showMore" | "button" | "none";

type Props = {
  titleNamespace: string;
  height: string;
  headerActionIcon: ReactElement | undefined;
  showHeaderAction: boolean;
  showBottomAction: BottomActionTypes;
  disableBottomButton?: boolean;
  bottomActionText?: string;
  headerActionClickedProps: () => void;
  bottomActionClickedProps: () => void;
  children: ReactNode;
  showContent: boolean;
  noContentText: string;
  isLoading?: boolean;
  description: string | undefined;
  showDescription: boolean;
  hasChildPadding: boolean;
};

export const CardWithHeaderAndFooter = (props: Props) => {
  const {
    titleNamespace,
    height,
    headerActionIcon,
    showBottomAction,
    showHeaderAction,
    children,
    showContent,
    noContentText,
    isLoading,
    description,
    showDescription,
    hasChildPadding,
    bottomActionText,
    disableBottomButton,
  } = props;
  const { t } = useTranslation();
  const { screenSize } = useScreenSize();

  const headerActionMenuClicked = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.stopPropagation();
    props.headerActionClickedProps();
  };

  const bottomActionClicked = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.stopPropagation();
    props.bottomActionClickedProps();
  };

  const padLeft = screenSize === ScreenSizeEnum.Mobile ? "20px" : "40px";
  const padRight = screenSize === ScreenSizeEnum.Mobile ? "20px" : "40px";
  const padTop = screenSize === ScreenSizeEnum.Mobile ? "30px" : "40px";
  const padBottom = screenSize === ScreenSizeEnum.Mobile ? "30px" : "30px";
  const padAll = screenSize === ScreenSizeEnum.Mobile ? "0px 20px 0px 20px" : "0px 40px 0px 40px";

  return (
    <CardStyled variant="outlined" sx={{ height: height, width: "100%" }}>
      {showHeaderAction ? (
        <CardHeaderStyled
          sx={{
            paddingLeft: padLeft,
            paddingRight: padRight,
            pb: padBottom,
            pt: padTop,
          }}
          titleTypographyProps={{ variant: "h6", color: "primary.main" }}
          title={titleNamespace ? t(titleNamespace) : ""}
          action={
            <IconButton onClick={(e) => headerActionMenuClicked(e)}>{headerActionIcon ? headerActionIcon : <EditIcon color="primary" />}</IconButton>
          }
        />
      ) : (
        <CardHeaderStyled
          sx={{
            paddingLeft: padLeft,
            paddingRight: padRight,
            pb: padBottom,
            pt: padTop,
          }}
          titleTypographyProps={{ variant: "h6", color: "primary.main" }}
          title={t(titleNamespace)}
        />
      )}

      {showDescription && (
        <Typography sx={{ pl: padLeft, pr: padRight, color: "grey.50" }} variant="body2">
          {description}
        </Typography>
      )}
      {showContent && !isLoading && hasChildPadding && <CardContentStyled sx={{ padding: padAll }}>{children}</CardContentStyled>}
      {showContent && !isLoading && !hasChildPadding && children}
      {isLoading && showContent && (
        <CardContentStyled sx={{ display: "flex", justifyContent: "center", padding: padAll }}>
          <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
            <CircularProgress size={60} />
          </Box>
        </CardContentStyled>
      )}
      {showContent && showBottomAction === "showMore" && (
        <CardActionsRightStyled>
          <Button color="primary" variant="text" disabled={disableBottomButton} onClick={(e) => bottomActionClicked(e)}>
            {t("common.viewMore")}
          </Button>
        </CardActionsRightStyled>
      )}
      {showContent && showBottomAction === "button" && (
        <CardActionsCenterStyled>
          <Button variant="contained" color="primary" sx={{ width: "70%" }} disabled={disableBottomButton} onClick={(e) => bottomActionClicked(e)}>
            {t(bottomActionText ?? "common.viewMore")}
          </Button>
        </CardActionsCenterStyled>
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
