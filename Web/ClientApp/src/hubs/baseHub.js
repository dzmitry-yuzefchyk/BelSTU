import signalr from '@aspnet/signalr';

export default class BaseHub {
    constructor(hubRoute) {
        const host = process.env.REACT_APP_URL;
        this.hubRoute = hubRoute;
        this.connection = new signalr.HubConnectionBuilder
            .withUrl(`${host}/${hubRoute}`)
            .build();

        this.start = this.start.bind(this);
        this.connection.onclose(this.start);
    }

    async start() {
        try
        {
            await this.connection.start();
        } catch(e) {
            if (process.env.NODE_ENV == 'development')
            {
                throw(`Can\'t start a hub on ${this.hubRoute}`)
            }
        }
    }

    addToGroup(groupName) {
        this.connection.invoke('RemoveFromGroupAsync', groupName);
    }

    removeFromGroup(groupName) {
        this.connection.invoke('AddToGroupAsync', groupName);
    }
}