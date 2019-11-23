import { observable, action, computed } from 'mobx';
import { POST, GET } from './../utils/axios';
import {
    POST_SIGN_UP,
    POST_SIGN_IN,
    POST_SIGN_OUT,
    POST_CONFIRM_EMAIL,
    POST_RESEND_EMAIL, GET_IS_AUTHORIZED,
    GET_PROFILE,
    GET_SETTINGS
} from './../utils/api.routes';

const navbarPath = 'navbar';

const themes = {
    dark: 0,
    light: 1
};

const anonymousUser = {
    isLoggedIn: false,
    lang: 'en',
    settings: {
        theme: themes.light,
        navbar: Boolean(window.localStorage.getItem(navbarPath))
    }
};

class UserStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable user = anonymousUser;

    @computed
    get darkTheme() {
        return this.user.settings.theme === themes.dark;
    }

    @computed
    get isNavOpen() {
        return this.user.settings.navbar;
    }

    set isNavOpen(boolean) {
        this.user.settings.navbar = boolean;
        window.localStorage.setItem(navbarPath, boolean);
    }

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
            this.user.isLoggedIn = true;
            await this.rootStore.fetchUserData();
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
            await POST(POST_SIGN_OUT);
            this.user = anonymousUser;
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
    async updateProfile(profile) {
        try {
            const response = await POST(POST_CONFIRM_EMAIL, profile);
            this.user.profile = profile;
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
    async updateSettings(settings) {
        try {
            const response = await POST(POST_CONFIRM_EMAIL, settings);
            this.user.settings = settings;
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
    async getProfile() {
        try {
            const response = await GET(GET_PROFILE);
            this.user.profile = response.data;
            //i18n.changeLanguage(this.user.lang);
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async getSettings() {
        try {
            const response = await GET(GET_SETTINGS);
            this.user.settings = response.data;
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

}

export default UserStore;