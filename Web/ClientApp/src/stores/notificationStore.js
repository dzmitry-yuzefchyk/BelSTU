import { observable, action, computed } from 'mobx';
import NotificationHub from './../hubs/notificationHub';
import { GET, DELETE } from './../utils/axios';
import { GET_NOTIFICATIONS, DELETE_NOTIFICATIONS } from './../utils/api.routes';

const notificationPath = 'notification';
const initialState = JSON.parse(window.localStorage.getItem(notificationPath));

export default class NotificationStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
        this.hub = new NotificationHub(this);
    }

    @observable notifications = [];
    @observable show = initialState;

    @computed get amount() {
        return this.notifications.length;
    }

    @computed
    get isNotificationsOpen() {
        return this.show;
    }

    set isNotificationsOpen(boolean) {
        this.show = boolean;
        window.localStorage.setItem(notificationPath, boolean);
    }

    @action.bound
    async getNotifcations() {
        try {
            const response = await GET(GET_NOTIFICATIONS);
            this.notifications = response.data;
        } catch(e) { }
    }

    @action.bound
    async clearAll() {
        try {
            if (this.amount === 0) return;

            await DELETE(DELETE_NOTIFICATIONS);
            this.notifications = [];
        } catch(e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    markAsRead(notificationId) {
        this.hub.markAsRead(notificationId);
        this.notifications = this.notifications
            .filter(notification => {
                if (!notification.id === notificationId)
                    return notification;
            });
    }

    @action.bound
    notify(message) {
        this.notifications.push(message);
        this.rootStore.snackbarStore.show('You have 1 new notification', 'info');
    }
}