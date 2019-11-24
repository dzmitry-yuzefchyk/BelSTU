import React from 'react';
import { inject, observer } from 'mobx-react';
import Preview from './components/preview';
import Sidebar from './components/sidebar/sidebar';
import { withStyles } from '@material-ui/styles';
import PrivateRoute from '../../components/route/route.private.strict';
import { PROJECTS, PROJECT } from '../../utils/routes';
import ProjectsBoard from './components/project/projects.board';
import ProjectView from './components/project/project.view';
import { useLocation } from 'react-router-dom';
import * as R from 'ramda';
import RecentNotifications from './components/notifications/recent.notifications';

const styles = theme => ({
    root: {
        display: 'flex',
        overflowY: 'auto',
        height: '100%',
        background: theme.palette.background.default
    },
    content: {
        flexGrow: 1,
        padding: theme.spacing(3),
        display: 'flex',
        flexDirection: 'row'
    }
});

const HomePage = (props) => {
    const { rootStore, classes } = props;
    const { userStore } = rootStore;
    const query = new URLSearchParams(useLocation().search);
    const page = query.get('page') || 0;

    if (!userStore.user.isLoggedIn)
        return <Preview />;

    return (
        <div className={classes.root}>
            <Sidebar />
            <div className={classes.content}>
                <RecentNotifications />
                <PrivateRoute path={PROJECTS}>
                    <ProjectsBoard page={page} />
                </PrivateRoute>
                <PrivateRoute path={PROJECT}>
                    <ProjectView />
                </PrivateRoute>
            </div>
        </div>
    );
}

export default R.compose(
    inject('rootStore'),
    withStyles(styles),
    observer
)(HomePage);