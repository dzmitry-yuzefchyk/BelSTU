import { action, observable, computed } from 'mobx';

export default class ModalStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable content = '';

    @computed get isOpen() {
        return !!this.content;
    }

    @action
    show(content) {
        if (this.isSnackbarOpen) return;
        this.content = content;
    }

    @action
    close() {
        this.content = '';
    }
}