import ModalStore from './modalStore';

class RootStore {
    constructor() {
        this.modalStore = new ModalStore(this);
    }
}

export default RootStore;