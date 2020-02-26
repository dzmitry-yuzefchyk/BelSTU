import React from 'react';
import { inject, observer } from 'mobx-react';
import Preview from './components/preview';
import Sidebar from './components/sidebar/sidebar';
import { withStyles } from '@material-ui/styles';
import PrivateRoute from '../../components/route/route.private.strict';
import { PROJECTS, PROJECT, BOARD } from '../../utils/routes';
import ProjectsBoard from './components/project/projects.board';
import ProjectView from './components/project/project.view';
import { useLocation } from 'react-router-dom';
import * as R from 'ramda';
import RecentNotifications from './components/notifications/recent.notifications';
import BoardView from './components/board/board.view';

const styles = theme => ({
    root: {
        display: 'flex',
        overflowY: 'auto',
        height: '100%',
        background: theme.palette.background.default
    },
    content: {
        flexGrow: 1,
        display: 'flex',
        flexDirection: 'row',
        overflowX: 'hidden'
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
                <PrivateRoute exact path={BOARD}>
                    <BoardView />
                </PrivateRoute>
                <PrivateRoute exact path={PROJECT}>
                    <ProjectView />
                </PrivateRoute>
                <PrivateRoute path={PROJECTS}>
                    <ProjectsBoard page={page} />
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