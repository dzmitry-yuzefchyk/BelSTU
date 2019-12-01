import { observable, action, computed } from 'mobx';
import { GET, DELETE, POST } from '../utils/axios';
import { GET_PROJECTS, POST_USER, DELETE_USER, GET_PROJECT, POST_CRETE_PROJECT } from '../utils/api.routes';

const AccessLevel = {
    PROJECT_CREATOR: 0,
    PROJECT_MEMBER: 1
}

const UserAction = {
    CREATE_BOARD: 0,
    UPDATE_BOARD: 1,
    DELETE_BOARD: 2,

    CREATE_TASK: 3,
    UPDATE_TASK: 4,
    DELETE_TASK: 5,

    CREATE_COMMENT: 6,

    UPDATE_PROJECT: 7,
    CHANGE_SECURITY: 8,
    DELETE_PROJECT: 9,
};

export default class ProjectStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable projects = [];
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
        } catch(e) {
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
        } catch(e) {
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
        } catch(e) {
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
            this.rootStore.snackbarStore.show(response.data, 'success');
        } catch(e) {
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
        } catch(e) {
            if (e.response) {
                this.rootStore.snackbarStore.show(e.response.data, 'error');
            } else {
                this.rootStore.snackbarStore.show(e.toString(), 'error');
            }
        }
    }
}