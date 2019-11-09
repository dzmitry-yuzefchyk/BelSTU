import { action, observable, computed } from 'mobx';

export default class ModalStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable content = '';
    @observable variant = 'info';

    @computed get isModalOpen() {
        return !!this.content;
    }

    @action
    showModal(content, variant) {
        if (this.isModalOpen) return;
        this.content = content;
        this.variant = variant;
    }

    @action
    closeModal() {
        this.content = '';
        this.variant = 'info';
    }
}