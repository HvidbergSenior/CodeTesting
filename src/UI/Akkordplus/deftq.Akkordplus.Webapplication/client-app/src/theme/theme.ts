import { createTheme } from "@mui/material/styles";
import { palette } from "./palette";
import { typography } from "./typography";

declare module "@mui/material/styles" {
  // allow configuration using `createTheme`
  interface ThemeOptions { }
}

declare module "@mui/material/Button" {
  interface ButtonPropsVariantOverrides {
    dismiss: true;
  }
}

export const theme = createTheme({
  palette,
  typography,
  components: {
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          scrollbarColor: "#fff #D5DFEA",
          "&::-webkit-scrollbar, & *::-webkit-scrollbar": {
            backgroundColor: "#fff",
            width: 9,
          },
          "&::-webkit-scrollbar-thumb, & *::-webkit-scrollbar-thumb": {
            borderRadius: 9,
            backgroundColor: "#D5DFEA",
            minHeight: 24,
            border: "3px solid #fff",
          },
          "&::-webkit-scrollbar-thumb:focus, & *::-webkit-scrollbar-thumb:focus": {
            backgroundColor: "#959595",
          },
          "&::-webkit-scrollbar-thumb:active, & *::-webkit-scrollbar-thumb:active": {
            backgroundColor: "#959595",
          },
          "&::-webkit-scrollbar-thumb:hover, & *::-webkit-scrollbar-thumb:hover": {
            backgroundColor: "#959595",
          },
          "&::-webkit-scrollbar-corner, & *::-webkit-scrollbar-corner": {
            backgroundColor: "#fff",
          },
        },
      },
    },
    MuiBreadcrumbs: {
      styleOverrides: {
        separator: {
          fontWeight: "bold",
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: ({ theme }) => ({
          minHeight: theme.spacing(5),
        }),
      },
    },
    MuiDialog: {
      defaultProps: {
        maxWidth: "sm",
        fullWidth: true,
      },
      styleOverrides: {
        root: ({ theme }) => ({
          borderRadius: theme.spacing(1.25),
        }),
      },
    },
    MuiDialogTitle: {
      styleOverrides: {
        root: ({ theme }) => ({
          paddingTop: theme.spacing(2),
          paddingRight: theme.spacing(3),
          paddingBottom: theme.spacing(2),
          paddingLeft: theme.spacing(3),
        }),
      },
    },
    MuiDialogActions: {
      styleOverrides: {
        root: ({ theme }) => ({
          padding: theme.spacing(3),
        }),
      },
    },
    MuiListItemIcon: {
      styleOverrides: {
        root: {
          minWidth: "40px",
        },
      },
    },
    MuiPaper: {
      defaultProps: {
        elevation: 0,
      },
      styleOverrides: {
        root: ({ theme }) => ({
          borderRadius: theme.spacing(1.25),
        }),
      },
    },
  },
});
