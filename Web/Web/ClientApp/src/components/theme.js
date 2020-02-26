import { createMuiTheme } from '@material-ui/core';
import { blue, grey } from '@material-ui/core/colors';

export const darkTheme = createMuiTheme({
    palette: {
        type: 'dark',
        primary: {
            light: blue[600],
            main: blue[700],
            dark: blue[800],
            contrastText: grey[800]
        }
    }
});

export const lightTheme = createMuiTheme({
    palette: {
        type: 'light',
        primary: {
            light: blue[500],
            main: blue[600],
            dark: blue[700]
        }
    }
});