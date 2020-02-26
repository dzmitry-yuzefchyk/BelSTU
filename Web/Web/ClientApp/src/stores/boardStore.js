import { observable, computed, action } from 'mobx';
import { GET, DELETE, POST } from '../utils/axios';
import { GET_BOARD, POST_CREATE_BOARD, POST_CREATE_COLUMN } from '../utils/api.routes';

export default class BoardStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @observable board = { columns: [] };
    @observable fetching = true;

    @action.bound
    async fetchBoard(boardId, projectId, searchBy = '', assignedToMe = false, orderBy = -1) {
        this.fetching = true;
        try {
            const url = `${GET_BOARD}/${boardId}?projectId=${projectId}&searchBy=${searchBy}&assignedToMe=${assignedToMe}&orderBy=${orderBy}`;
            const response = await GET(url);
            this.board = response.data;
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
    async createBoard(board) {
        try {
            const response = await POST(POST_CREATE_BOARD, board);
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
    async createColumn(column) {
        try {
            const response = await POST(POST_CREATE_COLUMN, column);
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