import React from 'react';
import { withStyles, Card, CardContent, CardActions, Typography, Button } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import * as R from 'ramda';

const styles = theme => ({
    card: {
        position: 'relative', 
        marginLeft: 10,
        marginRight: 10,
        marginTop: 10,
        marginBottom: 10,
        height: 150,
        minWidth: 175,
        maxHeight: 200,
        overflowY: 'auto',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'space-between'
    },
    actions: {
        bottom: 0 
    }
});

const ProjectDetails = (props) => {
    const { id, title, description, onClick, classes, t } = props;

    const openProject = () => {
        onClick(id);
    }

    return (
        <Card elevation={2} className={classes.card}>
            <CardContent>
                <Typography variant='h5' component='h2'>
                    {title}
                </Typography>
                <Typography>
                    {description}
                </Typography>
            </CardContent>
            <CardActions className={classes.actions}>
                <Button color='primary' onClick={openProject}>
                    {t('project.open')}
                </Button>
            </CardActions>
        </Card>
    );
}

export default R.compose(
    withStyles(styles),
    withTranslation()
)(ProjectDetails);