import { AppBar, Box, Container, Paper, Tab, Tabs, Toolbar, Typography } from "@mui/material";
import { useMemo } from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";

export default function App() {
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const tabIndex = useMemo(() => (pathname === "/" ? 0 : pathname.startsWith("/accepted") ? 1 : false), [pathname]);

  return (
    <Box sx={{ minHeight: "100vh", bgcolor: "grey.100" }}>
      <AppBar position="static" elevation={0} color="inherit" sx={{ borderBottom: 1, borderColor: "grey.300" }}>
        <Toolbar sx={{ maxWidth: 1100, mx: "auto", width: "100%" }}>
          <Typography variant="h6" sx={{ fontWeight: 800, mr: 3 }}>Lead Manager</Typography>
          <Tabs
            value={tabIndex}
            onChange={(_, v) => navigate(v === 0 ? "/" : "/accepted")}
            textColor="inherit"
            TabIndicatorProps={{ sx: { bgcolor: "primary.main" } }}
          >
            <Tab label="Invited" />
            <Tab label="Accepted" />
          </Tabs>
        </Toolbar>
      </AppBar>

      <Container sx={{ maxWidth: 1100, px: 2, py: 3 }}>
        <Paper elevation={0} sx={{ p: 2, border: 1, borderColor: "grey.300", bgcolor: "background.paper" }}>
          <Outlet />
        </Paper>
      </Container>
    </Box>
  );
}
