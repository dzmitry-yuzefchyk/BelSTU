import { action, observable, computed } from 'mobx';

export default class SnackbarStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable content = '';
    @observable variant = 'info';

    @computed get isOpen() {
        return !!this.content;
    }

    @action
    show(content, variant) {
        if (this.isSnackbarOpen) return;
        this.content = content;
        this.variant = variant;
    }

    @action
    close() {
        this.content = '';
        this.variant = 'info';
    }
}