import { useTranslation } from "react-i18next";
import Typography from "@mui/material/Typography";
import { formatNumberToAmount, formatNumberToPrice } from "utils/formats";
import { CardWithHeader } from "./card-header";
import { costumPalette } from "theme/palette";
import { CardSize, FontSize, getCardHeight, getCardValuePaddingTop, getFontSize } from "./use-value-card-formatter";

type Props = {
  titleNamespace: string;
  value?: number;
  fontSize: FontSize;
  unitNamespace: string;
  asCurrency?: boolean;
  showBackground: boolean;
  headerActionClickedProps?: () => void;
  cardSize: CardSize;
  isLoading?: boolean;
  fixedWidth?: string;
  footer: string;
};

export const ValueCardFooter = (props: Props) => {
  const {
    titleNamespace,
    fixedWidth,
    value,
    cardSize,
    isLoading,
    fontSize,
    unitNamespace,
    asCurrency,
    showBackground,
    footer,
    headerActionClickedProps,
  } = props;
  const { t } = useTranslation();
  const nonAction = () => {};

  const fontSizeAdjusted = getFontSize(fontSize);

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
        {asCurrency ? formatNumberToPrice(value) : formatNumberToAmount(value)} {t(unitNamespace).toLowerCase()}
      </Typography>
      <Typography variant="overline" color="primary.main" fontStyle="italic" sx={{ pt: 1 }}>
        {footer}
      </Typography>
    </CardWithHeader>
  );
};
