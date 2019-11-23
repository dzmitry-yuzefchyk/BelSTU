import React from 'react';
import CenteredLayout from '../../../components/layout/centered.layout';
import { Card, CardActions, CardMedia,  Button, CardContent, withStyles, Typography } from '@material-ui/core';
import { withRouter } from 'react-router-dom';
import { withTranslation } from 'react-i18next';
import { SIGN_IN } from '../../../utils/routes';

const styles = () => ({
    card: {
        width: 'auto'
    },
    media: {
        height: '75vh'
    }
});

const Preview = (props) => {
    const { history, t, classes } = props;

    const toSignIn = () => {
        history.push(SIGN_IN);
    }

    return (
        <CenteredLayout>
            <Card elevation={2} className={classes.card}>
                <CardMedia
                    className={classes.media}
                    image='/img/previewBg.jpg'
                    title='Preview'
                />
                <CardContent>
                    <Typography variant='h4'>
                        {t('preview.Hello nice to see you there')}
                    </Typography>
                </CardContent>
                <CardActions>
                    <Button color='primary' onClick={toSignIn}>
                        {t('forms.signIn')}
                    </Button>
                </CardActions>

            </Card>
        </CenteredLayout>
    );
};

export default withStyles(styles)(withRouter(withTranslation()(Preview)));