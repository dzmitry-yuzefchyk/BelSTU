import React from 'react';
import { Card, CardActions, CardContent, Button, CardHeader } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import { withRouter } from 'react-router-dom';
import * as R from 'ramda';

const Notification = props => {
    const { t, history, id, subject, description, directLink, markAsRead, className } = props;

    const navigate = () => {
        history.push(directLink);
    }

    const read = () => {
        markAsRead(id);
    }

    return (
        <Card className={className}>
            <CardHeader
                title={subject}
            />
            <CardContent>
                {description}
            </CardContent>
            <CardActions>
                <Button color='primary' onClick={navigate}>
                    {t('notification.goto')}
                </Button>
                <Button color='primary' onClick={read}>
                    {t('notification.mark as read')}
                </Button>
            </CardActions>
        </Card>
    );
}

export default R.compose(
    withTranslation(),
    withRouter
)(Notification);