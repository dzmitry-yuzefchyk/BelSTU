
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

//Hubs
export const BOARD_HUB = '/board';
export const NOTIFICATION_HUB = '/notification';