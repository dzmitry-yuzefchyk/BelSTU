import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { observer, inject } from 'mobx-react'
import Snackbar from './components/modal/snackbar';
import { withTranslation } from 'react-i18next';

import { SIGN_IN } from './utils/routes';

import SignInPage from './pages/signin/page';

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

    render() {
        return (
            <React.Fragment>
                <Switch>
                    <Route path={SIGN_IN}>
                        <SignInPage />
                    </Route>
                    <Route exact path='/' />
                </Switch>
                {this.renderSnackbar()}
            </React.Fragment>
        );
    }
}

export default App;