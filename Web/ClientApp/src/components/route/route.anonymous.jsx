import React from 'react';
import { inject, observer } from 'mobx-react';
import { Route, withRouter } from 'react-router-dom';
import { HOME } from './../../utils/routes';

@inject('rootStore')
@observer
@withRouter
class AnonymousRoute extends React.Component {
    componentDidUpdate() {
        const { userStore, snackbarStore } = this.props.rootStore;
        const { history } = this.props;

        if (userStore.user.isLoggedIn) {
            snackbarStore.show('You already signed-in', 'warning');
            history.push(HOME);
        }
    }

    render() {
        const { path, children } = this.props;
        return (
            <Route path={path}>
                {children}
            </Route>
        );
    }
}

export default AnonymousRoute;