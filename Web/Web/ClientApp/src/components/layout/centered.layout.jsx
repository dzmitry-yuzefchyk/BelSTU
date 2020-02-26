import React from 'react';
import { Box, withStyles } from '@material-ui/core';
import clsx from 'clsx';
import PropTypes from 'prop-types';

const styles = () => ({
    root: {
        height: '100%',
        width: '100%'
    },
    withBackground: {
        background: 'url("/img/formBg.jpg")'
    }
});

const CenteredLayout = props => {
    const { useBg, classes, children } = props;

    return (
        <Box
            display='flex'
            flexDirection='column'
            alignItems='center'
            justifyContent='center'
            className={clsx(classes.root, { [classes.withBackground]: useBg })}
        >
            {children}
        </Box>
    )
};

CenteredLayout.propTypes = {
    useBg: PropTypes.bool
};

export default withStyles(styles)(CenteredLayout);