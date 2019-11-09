import React from 'react';
import { inject, observer } from 'mobx-react';
import { Route, Redirect } from 'react-router-dom';
import { SIGN_IN } from './../../utils/routes';

@inject('rootStore')
@observer
class PrivateRoute extends React.Component {
    componentDidMount() {
        const { userStore, modalStore } = props.rootStore;
        if (!userStore.isUserLoggedIn) modalStore.showModal('Sign-in to view this page', 'warning');
    }

    render() {
        const { rootStore: { userStore }, path } = props;

        return (
            <Route path={path}>
                {userStore.isUserLoggedIn
                    ? props.children
                    : <Redirect to={SIGN_IN}/>}
            </Route>
        );
    }
}

export default PrivateRoute;