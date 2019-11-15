import { observable, action, reaction } from 'mobx';
import { POST } from './../utils/axios';
import { POST_SIGN_UP, POST_SIGN_IN, POST_SIGN_OUT } from './../utils/api.routes';
import i18n from './../i18n';

class UserStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable user = {
        isLoggedIn: false,
        lang: 'en'
    };

    @action async signUp(user) {
        try {
            await POST(POST_SIGN_UP, user);
        } catch(e) {
            this.rootStore.snackbarStore.show(e.response.data || e.toString(), 'error');
        }
    }

    @action async signIn(user) {
        try {
            await POST(POST_SIGN_IN, user);
            this.user.isLoggedIn = { isLoggedIn: true };
            reaction(() => {
                i18n.changeLanguage(this.user.lang || 'en');
            });
        } catch(e) {
            this.rootStore.snackbarStore.show(e.response.data || e.toString(), 'error');
        }
    }

    @action async signOut() {
        try {
            await POST(POST_SIGN_OUT);
            this.user = { isLoggedIn: false };
        } catch(e) {
            this.rootStore.snackbarStore.show(e.response.data || e.toString(), 'error');
        }
    }

    @action async updateProfile() {

    }

    @action async updateSettings() {

    }

    @action async getProfile() {

    }

    @action async getSettings() {

    }
}

export default UserStore;