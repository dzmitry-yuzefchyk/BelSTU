import React from 'react';
import { inject, observer } from 'mobx-react';
import { Route, Redirect } from 'react-router-dom';
import { HOME } from './../../utils/routes';

@inject('rootStore')
@observer
class AnonymousRoute extends React.Component {
    componentDidMount() {
        const { userStore, modalStore } = props.rootStore;
        if (userStore.isUserLoggedIn) modalStore.showModal('You already signed-in', 'warning');
    }

    render() {
        const { rootStore: { userStore }, path } = props;

        return (
            <Route path={path}>
                {!userStore.isUserLoggedIn
                    ? props.children
                    : <Redirect to={HOME}/>}
            </Route>
        );
    }
}