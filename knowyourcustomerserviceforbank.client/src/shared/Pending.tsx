import CircularProgress from '@mui/material/CircularProgress';
import Typography from '@mui/material/Typography';

/**
 * Pending / loading page.
 *
 * Colors: theme tokens only.
 *  - spinner:  color="primary"
 *  - track:    currentColor set via the theme's divider CSS variable
 *  - label:    color="text.secondary"
 */
export default function Pending() {
  return (
    <main
      role="status"
      aria-live="polite"
      className="flex min-h-screen flex-col items-center justify-center gap-5 px-6"
    >
      <div className="relative size-14">
        {/* Track */}
        <CircularProgress
          variant="determinate"
          value={100}
          size={56}
          thickness={3}
          color="inherit"
          className="absolute inset-0"
        />
        {/* Spinner */}
        <CircularProgress
          size={56}
          thickness={3}
          color="primary"
          className="absolute inset-0"
          aria-label="Loading"
        />
      </div>

      <Typography color="text.secondary">Loading…</Typography>
    </main>
  );
}