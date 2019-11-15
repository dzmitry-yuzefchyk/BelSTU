import React from 'react';
import * as R from 'ramda';
import { inject } from 'mobx-react';
import useForm from './../../../components/form/useForm';
import { withStyles, Box, Paper, Button, TextField, Checkbox } from '@material-ui/core';
import { withRouter } from 'react-router-dom';
import { HOME, SIGN_UP } from './../../../utils/routes';
import { withTranslation } from 'react-i18next';
import { loginFormValidator as validator } from './../../../utils/validators';

const styles = theme => ({
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

const SignInForm = (props) => {
    const { history, t, classes } = props;
    const { userStore } = props.rootStore;
    const signIn = async () => {
        const user = {
            email: values.email,
            password: values.password,
            rememberMe: true

        };
        await userStore.signIn(user);
    };

    const {
        handleChange,
        handleSubmit,
        values,
        validationResult
    } = useForm(signIn, validator);

    const redirectToHome = () => {
        history.push(HOME);
    };

    const redirectToSignUp = () => {
        history.push(SIGN_UP);
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
                        <Checkbox
                            name='rememberMe'
                            onChange={handleChange}
                        />
                    </Box>

                    <Box className={classes.formInputBox} display='flex' justifyContent='space-around' flexDirection='row'>
                        <Button className={classes.button} variant='contained' color='primary' onClick={handleSubmit}>
                            {t('forms.signIn')}
                        </Button>

                        <Button className={classes.button} variant='contained' color='secondary' onClick={redirectToSignUp}>
                            {t('forms.signUp')}
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
)(SignInForm);
