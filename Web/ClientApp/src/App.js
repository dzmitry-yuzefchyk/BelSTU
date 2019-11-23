import React, { Suspense } from 'react';
import { Switch, Route } from 'react-router-dom';
import { observer, inject } from 'mobx-react'
import Snackbar from './components/modal/snackbar';
import { withTranslation } from 'react-i18next';
import { Dialog } from '@material-ui/core';
import AnonymousRoute from './components/route/route.anonymous';
import { I18nextProvider } from 'react-i18next';
import i18n from './i18n';
import CircularProgress from './components/progress/circular.progress';
import { darkTheme, lightTheme } from './components/theme';
import { ThemeProvider } from '@material-ui/styles';

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
        this.closeModal = this.closeModal.bind(this);
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
        const { rootStore } = this.props;

        return (
            <ThemeProvider theme={rootStore.userStore.darkTheme ? darkTheme : lightTheme}>
                <Suspense fallback={<CircularProgress />}>
                    <I18nextProvider i18n={i18n}>
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
                    </I18nextProvider>
                </Suspense>
            </ThemeProvider>
        );
    }
}

export default App;