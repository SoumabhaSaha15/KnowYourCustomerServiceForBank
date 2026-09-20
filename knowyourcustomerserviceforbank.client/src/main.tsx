import './index.css';
import theme from "@/config/theme";
import { StrictMode } from 'react';
import { routeTree } from './routeTree.gen';
import { SnackbarProvider } from 'notistack';
import { createRoot } from 'react-dom/client'
import { QueryClient } from "@tanstack/react-query";
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeProvider } from '@mui/material/styles';
import GlobalStyles from '@mui/material/GlobalStyles';
import { StyledEngineProvider } from '@mui/material/styles';
import { RouterProvider, createRouter } from '@tanstack/react-router';
import { PersistQueryClientProvider } from "@tanstack/react-query-persist-client";
import { createAsyncStoragePersister } from "@tanstack/query-async-storage-persister";

const persister = createAsyncStoragePersister({ storage: window.localStorage });
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 15 * 60 * 1000,
    },
  },
});


const router = createRouter({
  routeTree,
  context: queryClient,
  defaultViewTransition: true,
  defaultPreload: 'intent',
  scrollRestoration: true,
})

// Register things for typesafety
declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}



createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <StyledEngineProvider enableCssLayer>
      <GlobalStyles styles="@layer theme, base, mui, components, utilities;" />
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <PersistQueryClientProvider
          client={queryClient}
          persistOptions={{ persister }}
        >
          <SnackbarProvider maxSnack={3} style={{ borderRadius: 16 }}>
            <RouterProvider router={router} />
          </SnackbarProvider>
        </PersistQueryClientProvider>
      </ThemeProvider>
    </StyledEngineProvider>
  </StrictMode>,
);