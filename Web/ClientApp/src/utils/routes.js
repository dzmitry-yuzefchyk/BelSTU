export const HOME = '/';

//ACCOUNT
export const ACCOUNT = '/account';
export const PROFILE = `${ACCOUNT}/profile`;
export const SETTINGS = `${ACCOUNT}/settings`;
export const SETTINGS_GENERAL = `${SETTINGS}`;
export const SETTINGS_PROFILE = `${SETTINGS}/profile`;
export const NOTIFICATIONS = '/notifications';

//AUTHENTICATION
export const SIGN_IN = '/sign_in';
export const SIGN_UP = '/sign_up';
export const RESET_PASSWORD = '/forgot_password';
export const CONFIRM_EMAIL = '/confirm_email/:email';

//PROJECTS
export const PROJECTS = '/projects';
export const PROJECT = '/project/:id';
export const PROJECT_SETTINGS = '/project/:id/settings';
export const BOARD = '/project/:projectId/board/:id';
export const TASK = '/project/:projectId/board/:boardId/task/:id';