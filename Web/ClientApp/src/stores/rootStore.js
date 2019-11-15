import UserStore from './userStore';
import SnackbarStore from './snackbarStore';
import ModalStore from './modalStore';

class RootStore {
    constructor() {
        this.snackbarStore = new SnackbarStore(this);
        this.userStore = new UserStore(this);
        this.modalStore = new ModalStore(this);
    }
}

export default RootStore;