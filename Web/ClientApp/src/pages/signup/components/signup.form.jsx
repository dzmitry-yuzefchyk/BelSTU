import React from 'react';
import * as R from 'ramda';
import { inject } from 'mobx-react';
import { registrationFormValidator as validator } from '../../../utils/validators';
import { withStyles, Box, Paper, Button, TextField } from '@material-ui/core';
import jdenticon from 'jdenticon';
import { withTranslation } from 'react-i18next';
import { withRouter } from 'react-router-dom';
import useForm from './../../../components/form/useForm';
import { SIGN_IN, HOME } from '../../../utils/routes';

const styles = () => ({
    form: {
        width: '20rem'
    },
    paper: {
        padding: '1.25rem'
    },
    formInputBox: {
        paddingTop: '0.225rem',
        paddingBottom: '0.225rem'
    },
    formInput: {
        width: '20rem'
    },
    button: {
        height: '2.125rem',
        margin: '0.425rem'
    }
});

const SignUpForm = (props) => {
    const { history, t, classes } = props;
    const { userStore } = props.rootStore;
    const signUp = async () => {
        const icon = jdenticon.toSvg(values.email, 300);
        const svg = new DOMParser().parseFromString(icon, 'image/svg+xml');
        const svgString = new XMLSerializer().serializeToString(svg);
        const decoded = unescape(encodeURIComponent(svgString));
        const base64 = btoa(decoded);
        const imgSource = `data:image/svg+xml;base64,${base64}`;
        const user = {
            email: values.email,
            password: values.password,
            icon: imgSource
        };
        if (await userStore.signUp(user))
            history.push(SIGN_IN);
    };
    const {
        handleChange,
        handleSubmit,
        values,
        validationResult
    } = useForm(signUp, validator);

    const redirectToHome = () => {
        history.push(HOME);
    };

    const redirectToSignUp = () => {
        history.push(SIGN_IN);
    }

    return (
        <React.Fragment>
            <Box className={classes.form}>
                <Paper className={classes.paper}>
                    <Box display='flex' justifyContent='center'>
                        <Button onClick={redirectToHome}>
                            {t('common.appName')}
                        </Button>
                    </Box>

                    <Box className={classes.formInputBox} display='flex' justifyContent='center'>
                        <TextField
                            className={classes.formInput}
                            label={t('forms.email')}
                            name='email'
                            type='text'
                            onChange={handleChange}
                            error={!!validationResult.email}
                            helperText={t(`forms.validation.${validationResult.email || 'valid'}`)}
                        />
                    </Box>

                    <Box className={classes.formInputBox} display='flex' justifyContent='center'>
                        <TextField
                            className={classes.formInput}
                            label={t('forms.password')}
                            name='password'
                            type='password'
                            onChange={handleChange}
                            error={!!validationResult.password}
                            helperText={t(`forms.validation.${validationResult.password || 'valid'}`)}
                        />
                    </Box>

                    <Box className={classes.formInputBox} display='flex' justifyContent='center'>
                        <TextField
                            className={classes.formInput}
                            label={t('forms.passwordConfirmation')}
                            name='passwordConfirmation'
                            type='password'
                            onChange={handleChange}
                            error={!!validationResult.passwordConfirmation}
                            helperText={t(`forms.validation.${validationResult.passwordConfirmation || 'valid'}`)}
                        />
                    </Box>

                    <Box className={classes.formInputBox} display='flex' justifyContent='space-around' flexDirection='row'>
                        <Button className={classes.button} variant='contained' color='primary' onClick={handleSubmit}>
                            {t('forms.signUp')}
                        </Button>

                        <Button className={classes.button} variant='contained' color='secondary' onClick={redirectToSignUp}>
                            {t('forms.signIn')}
                        </Button>
                    </Box>
                </Paper>
            </Box>
        </React.Fragment>
    );
}

export default R.compose(
    inject('rootStore'),
    withRouter,
    withTranslation(),
    withStyles(styles)
)(SignUpForm);
