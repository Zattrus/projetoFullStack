import type { JSX } from "@emotion/react/jsx-runtime";
import PlaceOutlinedIcon from "@mui/icons-material/PlaceOutlined";
import TagOutlinedIcon from "@mui/icons-material/TagOutlined";
import WorkOutlineIcon from "@mui/icons-material/WorkOutline";
import {
  Alert, Avatar, Box, Card, CardContent, CircularProgress,
  Divider, Stack, Typography
} from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { fetchAcceptedLeads } from "../api/leads";

const money = new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" });
const dateFmt = new Intl.DateTimeFormat(undefined, { month: "long", day: "numeric" });

const MetaItem = ({ icon, text }: { icon: JSX.Element; text: string }) => (
  <Stack direction="row" spacing={0.75} alignItems="center">
    {icon}
    <Typography variant="body2" color="text.secondary">{text}</Typography>
  </Stack>
);

export default function AcceptedPage() {
  const { data, isLoading, isError } = useQuery({
    queryKey: ["accepted"],
    queryFn: fetchAcceptedLeads,
  });

  if (isLoading)
    return (
      <Box display="flex" justifyContent="center" py={6}>
        <CircularProgress />
      </Box>
    );

  if (isError) return <Alert severity="error">Failed to load accepted leads.</Alert>;
  if (!data || data.length === 0)
    return <Alert severity="info">No accepted leads yet.</Alert>;

  return (
    <Stack spacing={2}>
      {data.map((l) => {
        const initials = (l.contactFullName ?? "?").slice(0, 1).toUpperCase();
        const jobId = l.id.slice(0, 6).toUpperCase();

        return (
          <Card key={l.id} variant="outlined" sx={{ borderColor: "grey.300" }}>
            <CardContent sx={{ pb: 1 }}>

              <Stack direction="row" spacing={2} alignItems="center">
                <Avatar sx={{ bgcolor: "primary.main", fontWeight: 700 }}>{initials}</Avatar>
                <Box>
                  <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                    {l.contactFullName}
                  </Typography>
                  <Typography variant="caption" color="text.secondary">
                    {dateFmt.format(new Date(l.createdAt))} @{" "}
                    {new Date(l.createdAt).toLocaleTimeString([], {
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </Typography>
                </Box>
              </Stack>

              <Stack direction="row" spacing={3} alignItems="center" sx={{ mt: 1.25 }}>
                <MetaItem icon={<PlaceOutlinedIcon fontSize="small" />} text={l.suburb} />
                <MetaItem icon={<WorkOutlineIcon fontSize="small" />} text={l.category} />
                <MetaItem icon={<TagOutlinedIcon fontSize="small" />} text={`Job ID: ${jobId}`} />
                <Typography variant="subtitle2" color="text.secondary">
                  <Box component="span" sx={{ fontWeight: 600, color: "grey", mr: 0.5 }}>
                    {money.format(l.price)}
                  </Box>
                  Lead invitation
                </Typography>
              </Stack>

              <Divider sx={{ my: 1.5 }} />

              <Stack direction="row" justifyContent="space-between" alignItems="center">
                <Stack direction="row" spacing={2} alignItems="center">
                  <Typography variant="body2" color="orange">📞 {l.contactPhone ?? "-"}</Typography>
                  <Typography variant="body2" color="orange">✉️ {l.contactEmail ?? "-"}</Typography>
                </Stack>
              </Stack>
            </CardContent>
          </Card>
        );
      })}
    </Stack>
  );
}
