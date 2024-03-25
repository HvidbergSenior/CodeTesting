import { useTranslation } from "react-i18next";
import Typography from "@mui/material/Typography";
import { formatNumberToAmount, formatNumberToPrice } from "utils/formats";
import { CardWithHeader } from "./card-header";
import { costumPalette } from "theme/palette";
import { CardSize, FontSize, getCardHeight, getCardValuePaddingTop, getFontSize, getUnitFontSize } from "./use-value-card-formatter";

type Props = {
  titleNamespace: string;
  value?: number;
  fontSize: FontSize;
  unitNamespace: string;
  asCurrency?: boolean;
  showBackground: boolean;
  headerActionClickedProps?: () => void;
  cardSize?: CardSize;
  isLoading?: boolean;
  fixedWidth?: string;
};

export const ValueCard = (props: Props) => {
  const { titleNamespace, fixedWidth, value, cardSize, isLoading, fontSize, unitNamespace, asCurrency, showBackground, headerActionClickedProps } =
    props;
  const { t } = useTranslation();
  const nonAction = () => {};

  const fontSizeAdjusted = getFontSize(fontSize);
  const unitFontSizeAdjusted = getUnitFontSize(fontSize);

  return (
    <CardWithHeader
      titleNamespace={titleNamespace}
      height={getCardHeight(cardSize)}
      showContent={true}
      noContentText={""}
      description={undefined}
      showDescription={false}
      backgroundColor={showBackground ? costumPalette.primaryLight : undefined}
      headerActionClickedProps={headerActionClickedProps ?? nonAction}
      showHeaderAction={!!headerActionClickedProps}
      isLoading={isLoading}
      fixedWidth={fixedWidth}
    >
      <Typography pt={getCardValuePaddingTop(cardSize)} variant={fontSize && fontSizeAdjusted}>
        {asCurrency ? formatNumberToPrice(value) : formatNumberToAmount(value)}
      </Typography>
      <Typography variant={fontSize && unitFontSizeAdjusted}>{t(unitNamespace).toLowerCase()}</Typography>
    </CardWithHeader>
  );
};
