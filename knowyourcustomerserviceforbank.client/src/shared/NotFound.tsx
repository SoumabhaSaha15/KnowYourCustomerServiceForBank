import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import { Link, useRouter } from '@tanstack/react-router';

/**
 * 404 page.
 *
 * Colors: only theme tokens are used.
 *  - MUI props:   color="text.primary" | "text.secondary" | "primary" | "inherit"
 *  - Tailwind:    arbitrary values pointing at MUI's CSS variables, e.g.
 *                 border-(color:--mui-palette-primary-main)
 *                 These switch automatically between the light and dark scheme.
 */
export default function NotFound() {
  const router = useRouter();

  return (
    <main className="flex min-h-screen items-center justify-center px-6 py-16">
      <div className="flex w-full max-w-2xl flex-col gap-8">
        {/* The "0" is drawn as a ring in the primary color */}
        <div
          aria-hidden="true"
          className="flex items-center gap-[0.04em] text-[clamp(7rem,26vw,16rem)] leading-none"
        >
          <Typography
            component="span"
            color="text.primary"
            className="text-[1em] font-semibold leading-none"
          >
            4
          </Typography>
          <span className="inline-block h-[0.7em] w-[0.52em] rounded-full border-[0.11em] border-solid" />
          <Typography
            component="span"
            color="text.primary"
            className="text-[1em] font-semibold leading-none"
          >
            4
          </Typography>
        </div>

        <div className="flex flex-col gap-3">
          <Typography
            component="h1"
            variant="h4"
            color="text.primary"
            className="font-semibold"
          >
            Page not found
          </Typography>
          <Typography
            color="text.secondary"
            className="max-w-md leading-relaxed"
          >
            The page you're looking for doesn't exist or has moved. Check the
            address for typos, or go back to where you were.
          </Typography>
        </div>

        <div className="flex flex-wrap gap-3">
          <Button
            component={Link}
            to="/"
            variant="contained"
            size="large"
            disableElevation
            className="px-6"
          >
            Go to home
          </Button>
          <Button
            variant="outlined"
            size="large"
            color="inherit"
            className="px-6"
            onClick={() => router.history.back()}
          >
            Go back
          </Button>
        </div>
      </div>
    </main>
  );
}