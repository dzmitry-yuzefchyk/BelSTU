import React from 'react';
import { Box, withStyles } from '@material-ui/core';

const styles = () => ({
    root: {
        height: '100%',
        width: '100%'
    }
});

const CenteredLayout = props => (
    <Box
        display='flex'
        flexDirection='column'
        alignItems='center'
        justifyContent='center'
        className={props.classes.root}
    >
        {props.children}
    </Box>
);

export default withStyles(styles)(CenteredLayout);