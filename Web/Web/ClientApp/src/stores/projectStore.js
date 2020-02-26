import { observable, action, computed } from 'mobx';
import { GET, DELETE, POST, PUT } from '../utils/axios';
import {
    GET_PROJECTS,
    POST_USER,
    DELETE_USER,
    GET_PROJECT,
    POST_CRETE_PROJECT,
    DELETE_PROJECT,
    GET_PROJECT_SETTINGS,
    GET_PROJECT_ACCESS,
    PUT_PROJECT_SETTINGS,
    PUT_PROJECT_ACCESS
} from '../utils/api.routes';

export default class ProjectStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable projects = [];
    @observable settings = {};
    @observable access = { users: [] };
    @observable project = { boards: [] };
    @observable fetching = true;
    @observable total = 0;

    @computed
    get isEmpty() {
        return this.projects.length === 0;
    }

    @action.bound
    async fetchProject(id) {
        this.fetching = true;
        try {
            const url = `${GET_PROJECT}/${id}`;
            const response = await GET(url);
            this.project = response.data;
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
        this.fetching = false;
    }

    @action.bound
    async fetchProjects(page, size) {
        this.fetching = true;
        try {
            const url = `${GET_PROJECTS}?page=${page}&size=${size}`;
            const response = await GET(url);
            this.projects = response.data.projects;
            this.total = response.data.total;
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
        this.fetching = false;
    }

    @action.bound
    async addToProject(projectUser) {
        try {
            const response = await POST(POST_USER, projectUser);
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
    async removeFromProject(projectUser) {
        try {
            const response = await DELETE(DELETE_USER, projectUser);
            this.access.users = this.access.users.filter(x => x.email !== projectUser.email);
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
    async createProject(project) {
        try {
            const response = await POST(POST_CRETE_PROJECT, project);
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
    async deleteProject(projectId) {
        try {
            const url = `${DELETE_PROJECT}/${projectId}`;
            const response = await DELETE(url);
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
    async fetchSettings(projectId) {
        this.fetching = true;
        try {
            const url = `${GET_PROJECT_SETTINGS}/${projectId}`;
            const response = await GET(url);
            this.settings = response.data;
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
        this.fetching = false;
    }

    @action.bound
    async fetchAccess(projectId, page = 0) {
        this.fetching = true;
        try {
            const url = `${GET_PROJECT_ACCESS}/${projectId}?page=${page}`;
            const response = await GET(url);
            this.access = response.data;
        } catch (e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
        this.fetching = false;
    }

    @action.bound
    async updateSettings(settings) {
        try {
            const response = await PUT(PUT_PROJECT_SETTINGS, settings);
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
    async updateAccess(projectId) {
        try {
            const data = {
                projectId,
                users: this.access.users.toJS().filter(x => !x.isAdmin || !x.changingBlocked)
            };
            const response = await PUT(PUT_PROJECT_ACCESS, data);
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