import { createTheme } from "@mui/material";

export const theme = createTheme({
  palette: {
    primary: { main: "#F36F21" },
    grey: { 100: "#f5f5f5", 200: "#eeeeee", 300: "#e0e0e0" }
  },
  shape: { borderRadius: 8 },
  typography: { fontSize: 14 },
  components: {
    MuiTabs: {
      styleOverrides: {
        indicator: { backgroundColor: "#F36F21", height: 3, borderRadius: 2 }
      }
    },
    MuiTab: {
      styleOverrides: {
        root: { textTransform: "none", fontWeight: 600, paddingInline: 24, minHeight: 48 }
      }
    },
    MuiCard: { styleOverrides: { root: { borderColor: "#e6e6e6" } } }
  }
});