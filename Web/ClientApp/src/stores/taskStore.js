import { action, observable } from 'mobx';
import { GET, DELETE, POST } from '../utils/axios';
import { POST_CREATE_TASK } from '../utils/api.routes';


export default class TaskStore {
    constructor(rootStore) {
        this.rootStore = rootStore;
    }

    @action.bound
    async createTask(task) {
        
    }
}