
//API
const api = 'api';

const accountController = `${api}/Account`;
export const POST_SIGN_UP = `${accountController}/SignUp`;
export const POST_SIGN_IN = `${accountController}/SignIn`;
export const POST_SIGN_OUT = `${accountController}/SignOut`;
export const POST_CONFIRM_EMAIL = `${accountController}/ConfirmEmail`;
export const POST_RESEND_EMAIL = `${accountController}/ResendEmail`;
export const PUT_UPDATE_PROFILE = `${accountController}/UpdateProfile`;
export const PUT_UPDATE_SETTINGS = `${accountController}/UpdateSettings`;
export const GET_PROFILE = `${accountController}/Profile`;
export const GET_SETTINGS = `${accountController}/Settings`;
export const GET_IS_AUTHORIZED = `${accountController}/IsAuthenticated`;

const notificationController = `${api}/Notification`;
export const GET_NOTIFICATIONS = `${notificationController}`;
export const DELETE_NOTIFICATIONS = `${notificationController}`;

const projectController = `${api}/Project`;
export const GET_PROJECT = `${projectController}`;
export const GET_PROJECTS = `${projectController}`;
export const POST_USER = `${projectController}/AddUser`;
export const DELETE_USER = `${projectController}/RemoveUser`;
export const POST_CRETE_PROJECT = `${projectController}`;
export const DELETE_PROJECT = `${projectController}`;
export const GET_PROJECT_SETTINGS = `${projectController}/GeneralSettings`;
export const GET_PROJECT_ACCESS = `${projectController}/AccessSettings`;
export const PUT_PROJECT_SETTINGS = `${projectController}/Settings`;
export const PUT_PROJECT_ACCESS = `${projectController}/AccessSettings`;
export const GET_USERS = `${projectController}/Users`;

const boardController = `${api}/Board`;
export const GET_BOARD = `${boardController}`;
export const POST_CREATE_BOARD = `${boardController}`;
export const POST_CREATE_COLUMN = `${boardController}/Column`;

const taskController = `${api}/Task`;
export const POST_CREATE_TASK = `${taskController}`;
export const GET_TASK = `${taskController}`;
export const POST_MOVE_TASK = `${taskController}/Move`;
export const DOWNLOAD_ATTACHMENT = `${taskController}/Attachment`;

//Hubs
export const BOARD_HUB = 'board';
export const NOTIFICATION_HUB = 'notification';