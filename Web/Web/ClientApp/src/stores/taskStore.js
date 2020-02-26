import { action, observable } from 'mobx';
import { GET, DELETE, POST } from '../utils/axios';
import { POST_CREATE_TASK, GET_USERS, GET_TASK, POST_MOVE_TASK, DOWNLOAD_ATTACHMENT } from '../utils/api.routes';


export default class TaskStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable users = [];
    @observable task = { attachments: [] };

    @action.bound
    async fetchUsers(projectId) {
        try {
            const url = `${GET_USERS}/${projectId}`;
            const response = await GET(url);
            this.users = response.data;
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async fetchTask(projectId, taskId) {
        try {
            const url = `${GET_TASK}/${taskId}?projectId=${projectId}`;
            const response = await GET(url);
            this.task = response.data;
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async downloadAttachment(projectId, attachmentId) {
        try {
            const url = `${DOWNLOAD_ATTACHMENT}/${attachmentId}?projectId=${projectId}`;
            window.open(url);
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async moveTask(projectId, taskId, columnId) {
        try {
            const url = `${POST_MOVE_TASK}?taskId=${taskId}&projectId=${projectId}&columnId=${columnId}`;
            await POST(url);
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }

    @action.bound
    async createTask(task) {
        try {
            const response = await POST(POST_CREATE_TASK, task);
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }
}