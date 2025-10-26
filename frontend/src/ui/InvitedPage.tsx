import type { JSX } from "@emotion/react/jsx-runtime";
import PlaceOutlinedIcon from "@mui/icons-material/PlaceOutlined";
import TagOutlinedIcon from "@mui/icons-material/TagOutlined";
import WorkOutlineIcon from "@mui/icons-material/WorkOutline";
import {
  Alert, Avatar, Box, Button, Card, CardActions, CardContent,
  CircularProgress, Divider, Stack, Typography
} from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { acceptLead, declineLead, fetchInvitedLeads } from "../api/leads";

const money = new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" });
const dateFmt = new Intl.DateTimeFormat(undefined, { month: "long", day: "numeric" });

const MetaItem = ({ icon, text }: { icon: JSX.Element; text: string }) => (
  <Stack direction="row" spacing={0.75} alignItems="center">
    {icon}
    <Typography variant="body2" color="text.secondary">{text}</Typography>
  </Stack>
);

export default function InvitedPage() {
  const qc = useQueryClient();

  const { data, isLoading, isError } = useQuery({
    queryKey: ["invited"],
    queryFn: fetchInvitedLeads,
  });

  const acceptMutation = useMutation({
    mutationFn: acceptLead,
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["invited"] });
      qc.invalidateQueries({ queryKey: ["accepted"] });
    },
  });

  const declineMutation = useMutation({
    mutationFn: declineLead,
    onSuccess: () => qc.invalidateQueries({ queryKey: ["invited"] }),
  });

  if (isLoading)
    return (
      <Box display="flex" justifyContent="center" py={6}>
        <CircularProgress />
      </Box>
    );

  if (isError) return <Alert severity="error">Failed to load invited leads.</Alert>;
  if (!data || data.length === 0)
    return <Alert severity="info">No invited leads right now.</Alert>;

  return (
    <Stack spacing={2}>
      {data.map((l) => {
        const initials = (l.contactFirstName ?? "?").slice(0, 1).toUpperCase();
        const jobId = l.id.slice(0, 6).toUpperCase();

        return (
          <Card key={l.id} variant="outlined" sx={{ borderColor: "grey.300" }}>
            <CardContent sx={{ pb: 1 }}>

              <Stack direction="row" spacing={2} alignItems="center">
                <Avatar sx={{ bgcolor: "primary.main", fontWeight: 700 }}>{initials}</Avatar>
                <Box>
                  <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                    {l.contactFirstName}
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
              </Stack>

              <Typography variant="body2" sx={{ mt: 1.5 }}>
                {l.description}
              </Typography>
            </CardContent>

            <Divider />

            <CardActions
              sx={{
                px: 2,
                py: 1.5,
                display: "flex",
                justifyContent: "space-between",
              }}
            >
              <Stack direction="row" spacing={1.5}>
                <Button
                  variant="contained"
                  color="primary"
                  disableElevation
                  onClick={() => acceptMutation.mutate(l.id)}
                  disabled={acceptMutation.isPending}
                  sx={{ px: 3, fontWeight: 700 }}
                >
                  {acceptMutation.isPending ? "Accepting..." : "Accept"}
                </Button>
                <Button
                  variant="outlined"
                  color="inherit"
                  onClick={() => declineMutation.mutate(l.id)}
                  disabled={declineMutation.isPending}
                  sx={{
                    px: 3,
                    bgcolor: "grey.200",
                    borderColor: "grey.300",
                    "&:hover": { bgcolor: "grey.300" },
                    fontWeight: 700,
                  }}
                >
                  {declineMutation.isPending ? "Declining..." : "Decline"}
                </Button>
                <Typography
                  variant="subtitle2"
                  color="text.secondary"
                  sx={{ display: "flex", alignItems: "center" }}
                >
                  <Box component="span" sx={{ fontWeight: 600, color: "grey", mr: 0.5 }}>
                    {money.format(l.price)}
                  </Box>
                  Lead Invitation
                </Typography>
              </Stack>
            </CardActions>
          </Card>
        );
      })}
    </Stack>
  );
}
