import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { observer, inject } from 'mobx-react'
import Snackbar from './components/modal/snackbar';
import { withTranslation } from 'react-i18next';

import { SIGN_IN, SIGN_UP } from './utils/routes';

import SignInPage from './pages/signin/page';
import SignUpPage from './pages/signup/page';
import { Dialog } from '@material-ui/core';

@inject('rootStore')
@withTranslation()
@observer
class App extends React.Component {

    constructor() {
        super();

        this.closeSnackbar = this.closeSnackbar.bind(this);
        this.renderSnackbar = this.renderSnackbar.bind(this);
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
                    <Route path={SIGN_IN}>
                        <SignInPage />
                    </Route>
                    <Route path={SIGN_UP}>
                        <SignUpPage />
                    </Route>
                    <Route exact path='/' />
                </Switch>
                {this.renderSnackbar()}
                {this.renderModal()}
            </React.Fragment>
        );
    }
}

export default App;