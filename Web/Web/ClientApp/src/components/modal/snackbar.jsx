import React from 'react';
import { Snackbar as MUISnackbar, SnackbarContent, IconButton, makeStyles } from '@material-ui/core';
import { observer } from 'mobx-react';
import { green, red, orange, grey } from '@material-ui/core/colors';
import CloseIcon from '@material-ui/icons/Close';
import PropTypes from 'prop-types';

const useStyles = makeStyles({
    success: {
        backgroundColor: green[700],
    },
    error: {
        backgroundColor: red[700],
    },
    info: {
        backgroundColor: grey,
    },
    warning: {
        backgroundColor: orange[700]
    }
});

const Snackbar = observer((props) => {
    const defaultAction = 
        <IconButton
            key='close'
            aria-label='close'
            color='inherit'
            onClick={props.onClose}
            >
            <CloseIcon />
        </IconButton>;
    const actions = [ defaultAction ].concat(props.action);
    const classes = useStyles();
    
    if (!props.message) return null;

    return (
        <MUISnackbar
            anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'center',
            }}
            open={props.isOpen}
            autoHideDuration={props.duration}
            onClose={props.onClose}
        >
            <SnackbarContent
                message={<span id={`message-${props.variant}`}>{props.message}</span>}
                className={classes[props.variant]}
                action={actions}
            />
        </MUISnackbar>
    );
});

Snackbar.defaultProps = {
    variant: 'info',
    duration: 5000,
    autoHide: true,
    isOpen: false
};

Snackbar.propTypes = {
    variant: PropTypes.oneOf(['success', 'error', 'warning', 'info']),
    message: PropTypes.string.isRequired,
    actions: PropTypes.node,
    isOpen: PropTypes.bool,
    duration: PropTypes.number,
    autoHide: PropTypes.bool,
    onClose: PropTypes.func.isRequired
};

export default Snackbar;