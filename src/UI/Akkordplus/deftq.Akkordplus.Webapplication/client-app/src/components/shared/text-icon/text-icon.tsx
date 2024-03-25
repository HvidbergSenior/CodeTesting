import { ReactElement, ReactNode } from "react";
import { useTranslation } from "react-i18next";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import styled from "@mui/system/styled";

export type TextDirection = "Bottom" | "Right";
type Props = {
  text?: string;
  translateText?: string;
  textDirection?: TextDirection;
  children: ReactNode | ReactElement;
};

export const TextIcon = (props: Props) => {
  const { text, translateText, children } = props;
  const { t } = useTranslation();
  const textDirection = !props.textDirection || props.textDirection === "Bottom" ? "column" : "row";
  const textPadding = !props.textDirection || props.textDirection === "Bottom" ? "2px" : "3px";

  const WrapperStyled = styled(Box)`
    display: flex;
    flex-direction: ${textDirection};
    align-items: center;
    justify-content: center;
    width: fit-content;
    min-width: 50px;
    margin: 0;
    padding: 0;
  `;
  const TypographyStyled = styled(Typography)`
    text-transform: uppercase;
    line-height: 10px;
    padding-top: ${textPadding};
  `;

  return (
    <WrapperStyled>
      {children}
      {text && <TypographyStyled variant="caption">{text}</TypographyStyled>}
      {translateText && <TypographyStyled variant="caption">{t(translateText)}</TypographyStyled>}
    </WrapperStyled>
  );
};
