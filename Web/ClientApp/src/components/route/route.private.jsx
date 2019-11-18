import React from 'react';
import { inject, observer } from 'mobx-react';
import { Route } from 'react-router-dom';
import Preview from '../../pages/home/components/preview';

@inject('rootStore')
@observer
class PrivateRoute extends React.Component {
    render() {
        const { rootStore, path, children } = this.props;
        const { userStore } = rootStore;

        return (
            <Route path={path}>
                {userStore.user.isLoggedIn
                    ? children
                    : <Preview />
                }
            </Route>
        );
    }
}

export default PrivateRoute;