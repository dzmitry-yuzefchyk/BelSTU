import React from 'react';
import { inject, observer } from 'mobx-react';
import Preview from './components/preview';
import Sidebar from './components/sidebar';
import { withStyles } from '@material-ui/styles';
import PrivateRoute from '../../components/route/route.private.strict';
import { PROJECTS } from '../../utils/routes';
import ProjectsBoard from './components/projects.board';
import { useLocation } from 'react-router-dom';
import * as R from 'ramda';

const styles = theme => ({
    root: {
        display: 'flex',
        height: '100%',
        background: theme.palette.background.default
    },
    content: {
        flexGrow: 1,
        padding: theme.spacing(3),
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
                <PrivateRoute path={PROJECTS}>
                    <ProjectsBoard page={page} />
                </PrivateRoute>
                <PrivateRoute path='/fff'>
                    <div> 2</div>
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