import { ThemeOptions } from "@mui/material";

const primaryMain = "#3D557C";
const primaryLight = "#D5DFEA";
const primaryDark = "#1E3055"

export const costumPalette = {
  primaryLight: primaryLight,
  operationSupplementBg: "#bbf2d478",
  operationSupplementColor: primaryMain,
  supplementBg: "#9be6e68f",
  supplementColor: primaryMain,
  iconGrey: "#555",
  iconBadgeGrey: "#A1AEC4",
  green: "#00847C",
  gray: "#e0e0e0",
  white: "#ffffff",
  offlineBlue: "#485577",
  reportBackground: "#FAFAFA",
  reportDark: "#E8EEF4",
};
export const palette: ThemeOptions["palette"] = {
  // Akkord+ Blue
  primary: {
    main: primaryMain,
    dark: primaryDark,
    light: primaryLight,
    100: "#3E557C",
  },
  // Akkord+ Orange
  secondary: {
    main: "#FF7118",
  },
  // Akkord+ Light Grey
  grey: {
    50: "#666666",
    100: "#C7D1DA",
  },
  background: {
    default: "#FFFFFF",
  },
  error: {
    main: "#F00",
  },
};
