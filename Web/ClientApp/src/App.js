import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { observer, inject } from 'mobx-react'
import Snackbar from './components/modal/snackbar';

@inject('rootStore')
@observer
class App extends React.Component {

    constructor() {
        super();

        this.closeModal = this.closeModal.bind(this);
        this.renderModal = this.renderModal.bind(this);
    }

    closeModal() {
        const { modalStore } = this.props.rootStore;
        modalStore.closeModal();
    }

    renderModal() {
        const { modalStore } = this.props.rootStore;

        return (
            <Snackbar
                isOpen={modalStore.isModalOpen}
                onClose={this.closeModal}
                variant={modalStore.variant}
                message={modalStore.content}
            />
        );
    }

    render() {
        return (
            <React.Fragment>
                <Switch>
                    <Route exact path='/'/>
                </Switch>
                {this.renderModal()}
            </React.Fragment>
        );
    }
}

export default App;
