import { observable, action, computed } from 'mobx';
import NotificationHub from './../hubs/notificationHub';
import { GET } from './../utils/axios';
import { GET_NOTIFICATIONS } from './../utils/api.routes';

export default class NotificationStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
        this.hub = new NotificationHub(this);
    }

    @observable notifications = [];

    @computed get amount() {
        return this.notifications.length;
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