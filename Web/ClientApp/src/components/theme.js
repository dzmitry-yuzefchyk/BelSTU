import { createMuiTheme } from '@material-ui/core';
import { blue } from '@material-ui/core/colors';

const darkTheme = createMuiTheme({
    palette: {
        type: 'dark',
        primary: {
            light: blue[500],
            main: blue[600],
            dark: blue[700]
        }
    }
});

const lightTheme = createMuiTheme({
    palette: {
        type: 'light',
        primary: {
            light: blue[500],
            main: blue[600],
            dark: blue[700]
        }
    }
});

export default lightTheme;