import { createTheme } from '@mui/material/styles';

const brandPrimary = '#6591FE';
const brandDark = '#23272B';

const darkTheme = createTheme(
  {
    colorSchemes: {
      light: {
        palette: {
          primary: {
            main: brandPrimary,
            light: '#8EAEFF',
            dark: '#3F6FD6',
            contrastText: '#FFFFFF',
          },
          secondary: {
            main: brandDark,
            contrastText: '#FFFFFF',
          },
          background: {
            default: '#F5F7FB', // Soft tinted background
            paper: '#FFFFFF',   // Crisp white for cards/surfaces
          },
          text: {
            primary: brandDark,
            secondary: '#606770',
          },
        },
      },
      dark: {
        palette: {
          primary: {
            main: brandPrimary,
            light: '#A0BCFF',
            dark: '#3F6FD6',
            contrastText: brandDark,
          },
          secondary: {
            main: '#D5DFEB',
            contrastText: brandDark,
          },
          background: {
            default: '#121417', // Deep charcoal base
            paper: brandDark,   // #23272B (Elevated surfaces match your brand outline)
          },
          text: {
            primary: '#F0F3F8',
            secondary: '#A0A7B4',
          },
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
export default darkTheme;