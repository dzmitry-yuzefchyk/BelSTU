import React from 'react';
import { withStyles, Card, CardContent, CardActions, Typography, Button, boxShadow } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import * as R from 'ramda';

const styles = theme => ({
    card: {
        marginLeft: 10,
        marginRight: 10,
        marginTop: 2,
        marginBottom: 2,
        height: 150,
        maxHeight: 150,
        maxWidth: 300
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
            <CardActions>
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