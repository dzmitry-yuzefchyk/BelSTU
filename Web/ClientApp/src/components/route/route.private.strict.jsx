import React from 'react';
import { inject, observer } from 'mobx-react';
import { Route, withRouter } from 'react-router-dom';
import { SIGN_IN } from '../../utils/routes';

@inject('rootStore')
@observer
@withRouter
class PrivateRouteStrict extends React.Component {
    componentDidUpdate() {
        const { userStore, snackbarStore } = this.props.rootStore;
        const { history } = this.props;
        if (!userStore.isLoggedIn) {
            snackbarStore.show('Sign-in to view this page', 'warning');
            history.push(SIGN_IN);
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

export default PrivateRouteStrict;