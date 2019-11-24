import { observable, action, computed } from 'mobx';
import NotificationHub from './../hubs/notificationHub';
import { GET } from './../utils/axios';
import { GET_NOTIFICATIONS } from './../utils/api.routes';

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
    markAsRead(notificationId) {
        this.hub.markAsRead(notificationId);
        this.notifications = this.notifications
            .filter(notification => {
                if (!notification.id === notificationId)
                    return notification;
            });
    }
}