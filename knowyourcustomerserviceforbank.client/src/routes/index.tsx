import { createFileRoute } from "@tanstack/react-router";
import {
  Box,
  Button,
  Chip,
  Container,
  Paper,
  Stack,
  Link,
  Typography,
} from "@mui/material";

export const Route = createFileRoute("/")({
  component: LandingPage,
});

function LandingPage() {
  const highlights = [
    {
      title: "Fast onboarding",
      text: "Guide customers through each verification step with a calm, clear experience.",
    },
    {
      title: "Risk-ready insights",
      text: "Assess profiles instantly and surface high-risk signals before approvals are finalized.",
    },
    {
      title: "Audit-friendly workflow",
      text: "Keep every action traceable with compliance logs and approval history in one place.",
    },
  ];

  const progressItems = [
    { label: "Customer profile setup", value: "Completed" },
    { label: "Document verification", value: "In review" },
    { label: "Risk assessment", value: "Ready" },
  ];

  return (
    <Box
      className="min-h-screen"
      sx={{
        bgcolor: "background.default",
        color: "text.primary",
        background: (theme) => `radial-gradient(circle at top left, color-mix(in srgb, ${theme.palette.primary.main} 20%, transparent), transparent 35%),linear-gradient(135deg, ${theme.palette.background.default} 0%, ${theme.palette.background.paper} 45%, ${theme.palette.background.default} 100%)`
      }}
    >
      {/* ── Navbar ── */}
      <Container maxWidth="xl">
        <Stack
          direction="row"
          spacing={2}
          className="items-center justify-between px-6 py-6 lg:px-8"
        >
          {/* Logo */}
          <Stack direction="row" spacing={2} className="items-center">
            <Box
              className="flex h-10 w-10 items-center justify-center rounded-full"
              sx={{
                bgcolor: "primary.main",
                borderStyle: "solid",
                borderWidth: 1,

                borderColor: "primary.light",
              }}
            >
              <Box
                component="img"
                src="/favicon.svg"
                alt="KYC-Flow logo"
                className="h-9 w-9 object-contain"
                sx={{ opacity: 5 }}
              />
            </Box>
            <Box>
              <Typography
                variant="h6"
                className="tracking-wide"
                sx={{ color: "text.primary" }}
              >
                KYC-Flow
              </Typography>
              <Typography variant="body2" sx={{ color: "text.secondary" }}>
                Banking onboarding platform
              </Typography>
            </Box>
          </Stack>

          {/* Nav links */}
          <Stack
            direction="row"
            spacing={3}
            className="hidden text-sm md:flex"
            sx={{ color: "text.secondary" }}
          >
            {["features", "workflow", "contact"].map((section) => (
              <Link
                key={section}
                // component="a"
                href={`#${section}`}
                className="capitalize transition-colors"
                sx={{
                  color: "text.secondary",
                  "&:hover": { color: "text.primary" },
                }}
              >
                {section.charAt(0).toUpperCase() + section.slice(1)}
              </Link>
            ))}
          </Stack>
        </Stack>
      </Container>

      {/* ── Hero ── */}
      <Container maxWidth="xl" className="pb-20">
        <Paper
          elevation={4}
          className="grid items-center gap-10 p-8 backdrop-blur-sm xl:grid-cols-[1.1fr_0.9fr] xl:p-12"
          sx={{
            bgcolor: "background.paper",
            borderWidth: 1,
            borderRadius: 2,
            borderStyle: "solid",
            borderColor: "divider",
          }}
        >
          {/* Hero copy */}
          <Box className="max-w-2xl">
            <Chip
              label="Digital customer onboarding made simple"
              variant="outlined"
              color="primary"
              size="small"
            />
            <Typography
              variant="h2"
              className="mt-6 font-semibold tracking-tight"
              sx={{
                color: "text.primary",
                fontSize: { xs: "2.25rem", sm: "3rem", lg: "3.75rem" },
              }}
            >
              Modern KYC workflows for faster, safer account approvals.
            </Typography>
            <Typography
              variant="body1"
              className="mt-5 text-lg leading-8"
              sx={{ color: "text.secondary" }}
            >
              KYC-Flow brings registration, identity verification, risk scoring, and audit
              tracking into one elegant experience for banks and compliance teams.
            </Typography>
            <Stack
              direction={{ xs: "column", sm: "row" }}
              spacing={2}
              className="mt-8"
            >
              <Button
                component="a"
                href="#contact"
                variant="contained"
                color="primary"
                className="px-5 py-3 text-sm font-semibold normal-case"
                sx={{ borderRadius: 1.25 }}
              >
                Start onboarding
              </Button>
              <Button
                component="a"
                href="#features"
                variant="outlined"
                color="inherit"
                sx={{ borderRadius: 1.25 }}
                className="px-5 py-3 text-sm font-semibold normal-case"
              >
                Explore features
              </Button>
            </Stack>
          </Box>

          {/* Progress card */}
          <Paper
            elevation={8}
            className="p-6 hover:scale-105"
            sx={{
              bgcolor: "background.default",
              borderWidth: 1,
              borderStyle: " solid",
              borderRadius: 2,
              borderColor: "divider",
            }}
          >
            <Stack direction="row" className="items-center justify-between">
              <Box>
                <Typography variant="body2" sx={{ color: "text.secondary" }}>
                  Today's progress
                </Typography>
                <Typography
                  variant="h3"
                  className="text-3xl font-semibold"
                  sx={{ color: "text.primary" }}
                >
                  82%
                </Typography>
              </Box>
              <Chip
                label="On track"
                color="success"
                variant="outlined"
                size="small"
              />
            </Stack>

            <Stack spacing={2} className="mt-6">
              {progressItems.map((item) => (
                <Box
                  key={item.label}
                  className="p-4"
                  sx={{
                    backgroundColor: "action.hover",
                    borderWidth: 1,
                    borderColor: "divider",
                    borderRadius: 1.5
                  }}
                >
                  <Stack direction="row" className="items-center justify-between">
                    <Typography variant="body2" sx={{ color: "text.secondary" }}>
                      {item.label}
                    </Typography>
                    <Typography
                      variant="body2"
                      className="font-medium"
                      sx={{ color: "text.primary" }}
                    >
                      {item.value}
                    </Typography>
                  </Stack>
                </Box>
              ))}
            </Stack>
          </Paper>
        </Paper>

        {/* ── Feature cards ── */}
        <Box id="features" className="mt-16 grid gap-6 md:grid-cols-3">
          {highlights.map((item) => (
            <Paper
              key={item.title}
              elevation={2}
              className="p-6 hover:scale-105"
              sx={{
                bgcolor: "background.paper",
                borderWidth: 1,
                borderRadius: 2,
                borderColor: "divider",
              }}
            >
              <Typography variant="h6" sx={{ color: "text.primary" }}>
                {item.title}
              </Typography>
              <Typography
                variant="body2"
                className="mt-3 leading-7"
                sx={{ color: "text.secondary" }}
              >
                {item.text}
              </Typography>
            </Paper>
          ))}
        </Box>

        {/* ── Workflow section ── */}
        <Paper
          id="workflow"
          elevation={2}
          className="mt-16 p-8 lg:p-10"
          sx={{
            bgcolor: "background.paper",
            // border: "1px solid",
            borderWidth: 1,
            borderColor: "divider",
            borderRadius: 2
          }}
        >
          <Stack
            direction={{ xs: "column", lg: "row" }}
            spacing={3}
            className="justify-between"
            sx={{ alignItems: { xs: "flex-start", lg: "flex-end" } }}
          >
            <Box className="max-w-2xl">
              <Typography
                variant="overline"
                className="tracking-widest"
                sx={{ color: "primary.main" }}
              >
                Workflow
              </Typography>
              <Typography variant="h4" className="mt-3" sx={{ color: "text.primary" }}>
                Built for compliance teams and digital banking teams alike.
              </Typography>
              <Typography
                variant="body1"
                className="mt-4 leading-8"
                sx={{ color: "text.secondary" }}
              >
                Collaborate across onboarding, verification, and approval stages without
                losing context or control.
              </Typography>
            </Box>
            <Chip
              label="Secure • Scalable • Compliant"
              color="success"
              variant="outlined"
              size="small"
            />
          </Stack>

          <Box className="mt-8 grid gap-4 md:grid-cols-3">
            {[
              "Register customer profile",
              "Upload and verify documents",
              "Approve or reject with audit trail",
            ].map((step) => (
              <Box
                key={step}
                className="p-4 text-sm hover:scale-105"
                sx={{
                  backgroundColor: "action.hover",
                  borderWidth: 1,
                  borderRadius: 1.5,
                  borderColor: "divider",
                  color: "text.secondary",
                }}
              >
                {step}
              </Box>
            ))}
          </Box>
        </Paper>
      </Container>

      {/* ── Footer ── */}
      <Box
        id="contact"
        sx={{
          borderTop: "1px solid",
          borderColor: "divider",
          bgcolor: "background.default",
        }}
      >
        <Container maxWidth="xl">
          <Stack
            direction={{ xs: "column", md: "row" }}
            className="gap-4 px-6 py-8 text-sm lg:px-8"
            sx={{
              color: "text.secondary",
              alignItems: { md: "center" },
              justifyContent: { md: "space-between" },
            }}
          >
            <Typography variant="body2">
              © 2026 KYC-Flow. Designed for modern financial onboarding.
            </Typography>
            <Box
              component="a"
              href="mailto:hello@kycflow.example"
              className="font-medium no-underline transition-colors"
              sx={{
                color: "primary.main",
                "&:hover": { color: "primary.light" },
              }}
            >
              hello@kycflow.example
            </Box>
          </Stack>
        </Container>
      </Box>
    </Box>
  );
}