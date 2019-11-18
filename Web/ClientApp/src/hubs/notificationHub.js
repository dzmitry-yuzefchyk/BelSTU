import BaseHub from './baseHub';
import { NOTIFICATION_HUB } from '../utils/api.routes';

class NotificationHub extends BaseHub {
    constructor(notificationStore) {
        super(NOTIFICATION_HUB);

        this.notificationStore = notificationStore;
        this.connection.on('notify', message => this.notify(message));
    }

    notify(message) {
        console.log(message);
        this.notificationStore.notifications.push(message);
    }

    markAsRead(messageId) {
        this.connection.invoke('MarkAsRead', messageId);
    }
}

export default NotificationHub;