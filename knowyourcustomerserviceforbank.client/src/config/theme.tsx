import { createTheme } from '@mui/material/styles';

const theme = createTheme(
  {
    spacing: 4,
    colorSchemes: {
      light: {
        palette: {
          primary: {
            main: '#427BFF',
            light: '#6C90FE',
            dark: '#2F5FD1',
            contrastText: '#FFFFFF',
          },
          secondary: {
            main: '#1F1F1F',
            contrastText: '#FFFFFF',
          },
          background: {
            default: '#F7F9FC',
            paper: '#FFFFFF',
          },
          text: {
            primary: '#1F1F1F',
            secondary: '#5F6673',
          },
          divider: '#E5E9F0',
        },
      },
      dark: {
        palette: {
          primary: {
            main: '#6C90FE',
            light: '#9DB7FF',
            dark: '#427BFF',
            contrastText: '#FFFFFF',
          },
          secondary: {
            main: '#D9E0EA',
            contrastText: '#1F1F1F',
          },
          background: {
            default: '#101216',
            paper: '#1F1F1F',
          },
          text: {
            primary: '#F3F5F8',
            secondary: '#A8AFBC',
          },
          divider: '#2C313A',
        },
      },
    },
    typography: {
      fontFamily: '"IBM Plex Serif", serif',
    },
    shape: { borderRadius: 12 },
    components: {
      MuiButton: {
        styleOverrides: {
          root: {
            borderRadius: 12, // Pill-shaped buttons
            textTransform: 'none', // MD3 drops all-caps button text
          },
        },
      },
      MuiList: {
        styleOverrides: {
          root: {
            paddingTop: 0,
            paddingBottom: 0,
          },
        },
      },
      MuiListItem: {
        styleOverrides: {
          root: {
            paddingTop: '2px',
            paddingBottom: '2px',
            paddingLeft: '0',
            paddingRight: '8px',
          },
        },
      },
      MuiListItemButton: {
        styleOverrides: {
          root: {
            borderBottomLeftRadius: 0,
            borderTopLeftRadius: 0,
            borderBottomRightRadius: 50,
            borderTopRightRadius: 50,
            boxShadow: 'none', // MD3 relies on tonal elevation over deep shadows
          },
        },
      },
      MuiCard: {
        styleOverrides: {
          root: {
            borderRadius: 24,
            boxShadow: 'none', // MD3 relies on tonal elevation over deep shadows
          },
        },
      },
      MuiFab: {
        styleOverrides: {
          extended: {
            textTransform: 'none',
          },
        },
      },
      MuiTab: {
        styleOverrides: {
          root: {
            textTransform: 'none',
          },
        },
      },
    }
  }
);
export default theme;