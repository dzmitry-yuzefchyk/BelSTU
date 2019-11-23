import React from 'react';
import { inject, observer } from 'mobx-react';
import Preview from './components/preview';
import Sidebar from './components/sidebar';
import { withStyles } from '@material-ui/styles';
import PrivateRoute from '../../components/route/route.private';
import { PROJECTS } from '../../utils/routes';
import ProjectsBoard from './components/projects.board';

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

@inject('rootStore')
@withStyles(styles)
@observer
class HomePage extends React.Component {
    render() {
        const { rootStore, classes } = this.props;
        const { userStore } = rootStore;

        if (!userStore.user.isLoggedIn)
            return <Preview />;

        return (
            <div className={classes.root}>
                <Sidebar />
                <div className={classes.content}>
                    <PrivateRoute path={PROJECTS}>
                        <ProjectsBoard />
                    </PrivateRoute>
                    <PrivateRoute path='/fff'>
                        <div> 2</div>
                    </PrivateRoute>
                </div>
            </div>
        );
    }
}

export default HomePage;