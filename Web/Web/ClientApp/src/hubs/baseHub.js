import * as signalR from '@aspnet/signalr';

export default class BaseHub {
    constructor(hubRoute) {
        const api = process.env.API;
        const transport = signalR.HttpTransportType.WebSockets;
        const options = {
            transport,
            logMessageContent: false,
            logger: signalR.LogLevel.Trace
        };

        this.hubRoute = hubRoute;
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(`https://localhost:44300/${hubRoute}`, options)
            .build();

        this.start();

        this.start = this.start.bind(this);
        this.connection.onclose(this.start);
    }

    start() {
        try {
            this.connection.start();
        } catch (e) {
            if (process.env.NODE_ENV === 'development') {
                throw (`Can\'t start a hub on ${this.hubRoute}`)
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