import { observable, action } from 'mobx';
import { POST, GET } from './../utils/axios';
import { POST_SIGN_UP, POST_SIGN_IN, POST_SIGN_OUT, POST_CONFIRM_EMAIL, POST_RESEND_EMAIL, GET_IS_AUTHORIZED } from './../utils/api.routes';
import i18n from './../i18n';

class UserStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable user = {
        isLoggedIn: false,
        lang: 'en'
    };

    @action.bound
    async checkAuth() {
        try {
            const response = await GET(GET_IS_AUTHORIZED);
            this.user.isLoggedIn = response.data;
        } catch (e) {
            this.user.isLoggedIn = false;
        }
    }

    @action.bound
    async signUp(user) {
        try {
            const response = await POST(POST_SIGN_UP, user);
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
            return false;
        }
        return true;
    }

    @action.bound
    async signIn(user) {
        try {
            const response = await POST(POST_SIGN_IN, user);
            this.user.isLoggedIn = { isLoggedIn: true };
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async signOut() {
        try {
            const response = await POST(POST_SIGN_OUT);
            this.user = { isLoggedIn: false };
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async resendConfirmationEmail(email) {
        try {
            const response = await POST(POST_RESEND_EMAIL, `"${email}"`);
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async confirmEmail(email, token) {
        try {
            const response = await POST(POST_CONFIRM_EMAIL, { email, token });
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async updateProfile() {

    }

    @action.bound
    async updateSettings() {

    }

    @action.bound
    async getProfile() {
        i18n.changeLanguage(this.user.lang);
    }

    @action.bound
    async getSettings() {

    }

    @action.bound
    async getNotifications() {

    }
}

export default UserStore;