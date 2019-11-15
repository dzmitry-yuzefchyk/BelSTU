import { observable, action, reaction } from 'mobx';
import { POST } from './../utils/axios';
import { POST_SIGN_UP, POST_SIGN_IN, POST_SIGN_OUT, POST_CONFIRM_EMAIL, POST_RESEND_EMAIL } from './../utils/api.routes';
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
            const response = await POST(POST_SIGN_UP, user);
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch(e) {
            this.rootStore.snackbarStore.show(e.response.data || e.toString(), 'error');
            return false;
        }
        return true;
    }

    @action async signIn(user) {
        try {
            const response = await POST(POST_SIGN_IN, user);
            this.user.isLoggedIn = { isLoggedIn: true };
            reaction(() => {
                i18n.changeLanguage(this.user.lang || 'en');
            });
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch(e) {
            this.rootStore.snackbarStore.show(e.response.data || e.toString(), 'error');
        }
    }

    @action async signOut() {
        try {
            const response = await POST(POST_SIGN_OUT);
            this.user = { isLoggedIn: false };
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch(e) {
            this.rootStore.snackbarStore.show(e.response.data || e.toString(), 'error');
        }
    }

    @action async resendConfirmationEmail(email) {
        try {
            const response = await POST(POST_RESEND_EMAIL, `"${email}"`);
            this.rootStore.snackbarStore.show(response.data, 'success');
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