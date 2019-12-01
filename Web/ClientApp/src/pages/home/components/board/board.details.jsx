import React from 'react';
import { withStyles, Card, CardContent, CardActions, Typography, Button } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import * as R from 'ramda';

const styles = theme => ({
    card: {
        position: 'relative', 
        marginLeft: 10,
        marginBottom: 10,
        height: 200,
        width: 250,
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'space-between'
    },
    actions: {
        bottom: 0 
    }
});

const BoardDetails = (props) => {
    const { id, title, onClick, classes, t } = props;

    const openBoard = () => {
        onClick(id);
    }

    return (
        <Card elevation={2} className={classes.card}>
            <CardContent>
                <Typography variant='h5' component='h2'>
                    {title}
                </Typography>
            </CardContent>
            <CardActions className={classes.actions}>
                <Button color='primary' onClick={openBoard}>
                    {t('board.open')}
                </Button>
            </CardActions>
        </Card>
    );
}

export default R.compose(
    withStyles(styles),
    withTranslation()
)(BoardDetails);