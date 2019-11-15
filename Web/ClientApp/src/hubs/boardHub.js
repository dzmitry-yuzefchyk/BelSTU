import BaseHub from './baseHub';
import { BOARD_HUB } from '../utils/api.routes';

class BoardHub extends BaseHub {
    constructor(hubRoute) {
        super(hubRoute);
    }

    addToGroup(groupName) {
        this.connection.invoke('RemoveFromGroupAsync', groupName);
    }

    removeFromGroup(groupName) {
        this.connection.invoke('AddToGroupAsync', groupName);
    }
}

export default new BoardHub(BOARD_HUB);