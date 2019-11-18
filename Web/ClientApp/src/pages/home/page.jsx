import React from 'react';
import { inject, observer } from 'mobx-react';
import Preview from './components/preview';
import Sidebar from '../components/sidebar';
import { withStyles } from '@material-ui/styles';

const styles = theme => ({
    root: {
        display: 'flex',
    },
    content: {
        flexGrow: 1,
        padding: theme.spacing(3),
    }
});

@inject('rootStore')
@observer
@withStyles(styles)
class HomePage extends React.Component {
    render() {
        const { rootStore, classes } = this.props;
        const { userStore } = rootStore;

        if (!userStore.user.isLoggedIn)
            return <Preview />;

        return (
            <div className={classes.root}>
                <Sidebar />
                <div className={classes.content}> Heeee</div>
            </div>
        );
    }
}

export default HomePage;