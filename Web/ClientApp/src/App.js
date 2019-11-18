import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { observer, inject } from 'mobx-react'
import Snackbar from './components/modal/snackbar';
import { withTranslation } from 'react-i18next';
import { Dialog } from '@material-ui/core';
import AnonymousRoute from './components/route/route.anonymous';

import { SIGN_IN, SIGN_UP, CONFIRM_EMAIL, HOME } from './utils/routes';

import SignInPage from './pages/signin/page';
import SignUpPage from './pages/signup/page';
import ConfirmEmailPage from './pages/confirmEmail/page';
import HomePage from './pages/home/page';

@inject('rootStore')
@withTranslation()
@observer
class App extends React.Component {

    constructor() {
        super();

        this.closeSnackbar = this.closeSnackbar.bind(this);
        this.renderSnackbar = this.renderSnackbar.bind(this);
    }

    async componentDidMount() {
        await this.props.rootStore.fetchUserData();
    }

    closeSnackbar() {
        const { snackbarStore } = this.props.rootStore;
        snackbarStore.close();
    }

    closeModal() {
        const { modalStore } = this.props.rootStore;
        modalStore.close();
    }

    renderSnackbar() {
        const { snackbarStore } = this.props.rootStore;

        return (
            <Snackbar
                isOpen={snackbarStore.isOpen}
                onClose={this.closeSnackbar}
                variant={snackbarStore.variant}
                message={snackbarStore.content}
            />
        );
    }

    renderModal() {
        const { modalStore } = this.props.rootStore;

        return (
            <Dialog
                keepMounted
                onClose={this.closeModal}
                open={modalStore.isOpen}
            >
                {modalStore.content}
            </Dialog>
        )
    }

    render() {
        return (
            <React.Fragment>
                <Switch>
                    <AnonymousRoute path={CONFIRM_EMAIL}>
                        <ConfirmEmailPage />
                    </AnonymousRoute>
                    
                    <AnonymousRoute path={SIGN_IN}>
                        <SignInPage />
                    </AnonymousRoute>
                    
                    <AnonymousRoute path={SIGN_UP}>
                        <SignUpPage />
                    </AnonymousRoute>

                    <Route path={HOME}>
                        <HomePage />
                    </Route>
                </Switch>
                {this.renderSnackbar()}
                {this.renderModal()}
            </React.Fragment>
        );
    }
}

export default App;