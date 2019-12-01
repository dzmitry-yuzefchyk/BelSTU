// eslint-disable-next-line no-useless-escape
const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

export function loginFormValidator(values) {
    let result = {};

    if (!values.email) {
        result.email = 'Email address is required';
    } else if (!emailRegex.test(values.email)) {
        result.email = 'Email address is invalid';
    }

    if (!values.password) {
        result.password = 'Password is required';
    }

    return result;
}

export function registrationFormValidator(values) {
    let result = {};

    if (!values.email) {
        result.email = 'Email address is required';
    } else if (!emailRegex.test(values.email)) {
        result.email = 'Email address is invalid';
    }

    if (!values.password) {
        result.password = 'Password is required';
    } else if (values.password.length < 6) {
        result.password = 'Password must be at least 6 symbols long';
    } else if (values.password !== values.passwordConfirmation) {
        result.passwordConfirmation = 'Password confirmation is not the same';
    }

    return result;
}

export function resendEmailFormValidator(values) {
    let result = {};

    if (!values.email) {
        result.email = 'Email address is required';
    } else if (!emailRegex.test(values.email)) {
        result.email = 'Email address is invalid';
    }

    return result;
}

export function createProjectFormValidator(values) {
    let result = {};

    if (!values.title) {
        result.title = 'Title is required';
    }

    return result;
}

export function createBoardFormValidator(values) {
    let result = {};

    if (!values.title) {
        result.title = 'Title is required';
    }

    return result;
}