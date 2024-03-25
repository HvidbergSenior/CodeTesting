export type FontSize = "large" | "small" | "xsmall";
export type CardSize = "small" | "large" | "medium";

export const getCardHeight = (cardSize?: CardSize): string => {
  const size = cardSize ?? "medium";
  switch (size) {
    case "small":
      return "120px";
    case "large":
      return "200px";
    case "medium":
    default:
      return "150px";
  }
};

export const getCardValuePaddingTop = (cardSize?: CardSize): number => {
  const size = cardSize ?? "medium";
  switch (size) {
    case "small":
      return 0;
    case "large":
      return 3;
    case "medium":
    default:
      return 1;
  }
};

export const getFontSize = (fontSize: FontSize) => {
  switch (fontSize) {
    case "large":
      return "h3";
    case "small":
      return "h4";
    case "xsmall":
      return "h5";
  }
};

export const getUnitFontSize = (fontSize: FontSize) => {
  switch (fontSize) {
    case "large":
      return "h5";
    case "small":
      return "h6";
    case "xsmall":
      return "h6";
  }
};
