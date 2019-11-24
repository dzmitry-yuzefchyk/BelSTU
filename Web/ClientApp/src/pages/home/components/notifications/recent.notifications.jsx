import React from 'react';
import { Card, CardHeader, CardContent, withStyles, CardActions, Button } from '@material-ui/core';
import { inject, observer } from 'mobx-react';
import { withTranslation } from 'react-i18next';
import { withRouter } from 'react-router-dom';
import clsx from 'clsx';
import Notification from './notification';

const styles = theme => ({
    root: {
        maxWidth: 375,
        width: 375,
        transition: theme.transitions.create('width', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen,
        }),
    },
    rootHidden: {
        width: 0,
        transition: theme.transitions.create('width', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
    }
});

@inject('rootStore')
@withTranslation()
@withStyles(styles)
@withRouter
@observer
class RecentNotifications extends React.Component {

    constructor() {
        super();

        this.markAsRead = this.markAsRead.bind(this);
        this.clearAll = this.clearAll.bind(this);
    }

    markAsRead(id) {
        const { notificationStore } = this.rootStore;
        notificationStore.markAsRead(id);
    }

    clearAll() {
        const { notificationStore } = this.rootStore;
        notificationStore.clearAll();
    }

    render() {
        const { t, classes } = this.props;
        const { notificationStore } = this.props.rootStore;
        const isOpen = notificationStore.isNotificationsOpen;

        return (
            <Card elevation={2} className={clsx(classes.root, { [classes.rootHidden]: !isOpen })}>
                <CardHeader
                    title={t('notification.recent')}
                    subheader={t('notification.here you can see what changed')}
                />
                <CardActions>
                    <Button variant='primary' onClick={this.clearAll}>
                        {t('notification.clear')}
                    </Button>
                </CardActions>
                <CardContent>
                    {notificationStore.notifications.map(notification =>
                        <Notification
                            key={notification.id}
                            id={notification.id}
                            subject={notification.subject}
                            description={notification.description}
                            directLink={notification.directLink}
                            markAsRead={this.markAsRead}
                        />
                    )}
                </CardContent>
            </Card>
        )
    }
}

export default RecentNotifications;