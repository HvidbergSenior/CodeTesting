import { ThemeOptions } from "@mui/material";

declare module "@mui/material/styles" {
  interface TypographyVariants {
    subtle: React.CSSProperties;
  }

  // Allow configuration using `createTheme`
  interface TypographyVariantsOptions {
    subtle?: React.CSSProperties;
  }
}

// Update the Typography's variant prop options
declare module "@mui/material/Typography" {
  interface TypographyPropsVariantOverrides {
    subtle: true;
  }
}

export const typography: ThemeOptions["typography"] = {
  fontFamily: "'Overpass', sans-serif",
  h1: {
    fontSize: "96px",
    fontStyle: "normal",
    fontWeight: 300,
    lineHeight: "112px",
    letterSpacing: "-1.5px",
  },
  h2: {
    fontSize: "60px",
    fontStyle: "normal",
    fontWeight: 300,
    lineHeight: "72px",
    letterSpacing: "-0.5px",
  },
  h3: {
    fontSize: "48px",
    fontStyle: "normal",
    fontWeight: 400,
    lineHeight: "52px",
    letterSpacing: 0,
  },
  h4: {
    fontSize: "34px",
    fontStyle: "normal",
    fontWeight: 300,
    lineHeight: "36px",
    letterSpacing: 0,
  },
  h5: {
    fontSize: "24px",
    fontStyle: "normal",
    fontWeight: 400,
    lineHeight: "24px",
    letterSpacing: "0.18px",
  },
  h6: {
    fontSize: "16px",
    fontStyle: "normal",
    fontWeight: 600,
    lineHeight: "24px",
    letterSpacing: "0.15px",
  },
  subtitle1: {
    fontSize: "16px",
    fontStyle: "normal",
    fontWeight: 400,
    lineHeight: "24px",
    letterSpacing: 0,
  },
  subtitle2: {
    fontSize: "14px",
    fontStyle: "normal",
    fontWeight: 500,
    lineHeight: "24px",
    letterSpacing: "0.1px",
  },
  body1: {
    fontSize: "16px",
    fontStyle: "normal",
    fontWeight: 400,
    lineHeight: "24px",
    letterSpacing: "0.5px",
  },
  body2: {
    fontSize: "14px",
    fontStyle: "normal",
    fontWeight: 400,
    lineHeight: "20px",
    letterSpacing: "0.25px",
  },
  button: {
    fontSize: "14px",
    fontStyle: "normal",
    fontWeight: 500,
    lineHeight: "16px",
    letterSpacing: "1.25px",
    textTransform: "uppercase",
  },
  caption: {
    fontSize: "12px",
    fontStyle: "normal",
    fontWeight: 400,
    lineHeight: "16px",
    letterSpacing: "0.4px",
  },
  overline: {
    fontSize: "10px",
    fontStyle: "normal",
    fontWeight: 400,
    lineHeight: "10px",
    letterSpacing: "1.5px",
    textTransform: "uppercase",
  }
};
