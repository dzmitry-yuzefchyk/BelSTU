import UserStore from './userStore';
import SnackbarStore from './snackbarStore';

class RootStore {
    constructor() {
        this.snackbarStore = new SnackbarStore(this);
        this.userStore = new UserStore(this);
    }
}

export default RootStore;