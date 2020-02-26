import { action, observable, computed } from 'mobx';
import i18n from '../i18n';

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
        this.content = i18n.t(`snackbar.${content}`);
        this.variant = variant;
    }

    @action
    close() {
        this.content = '';
        this.variant = 'info';
    }
}