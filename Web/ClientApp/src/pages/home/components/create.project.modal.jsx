import React from 'react';
import * as R from 'ramda';
import useForm from './../../../components/form/useForm';
import { resendEmailFormValidator as validator } from './../../../utils/validators';
import { inject } from 'mobx-react';
import { Button, DialogActions, DialogContent, DialogTitle, TextField } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import { withStyles } from '@material-ui/styles';

const styles = () => ({
    formInput: {
        width: '20rem'
    },
    button: {
        height: '2.125rem',
        margin: '0.425rem'
    }
});

const CreateProjectModal = (props) => {
    const { userStore, modalStore } = props.rootStore;
    const { t, classes } = props;
    const {
        values,
        validationResult,
        handleChange,
        handleSubmit
    } = useForm(resendEmail, validator);

    const onClose = () => {
        modalStore.close();
    }

    const resendEmail = async () => {
        await userStore.resendConfirmationEmail(values.email);
        onClose();
    }

    return (
        <React.Fragment>
            <DialogTitle>{t('modal.resend email')}</DialogTitle>
            <DialogContent>
                <TextField
                    className={classes.formInput}
                    label={t('forms.email')}
                    name='email'
                    type='text'
                    onChange={handleChange}
                    error={!!validationResult.email}
                    helperText={t(`forms.validation.${validationResult.email || 'valid'}`)}
                />
            </DialogContent>
            <DialogActions>
                <Button className={classes.button} onClick={handleSubmit} color='primary'>
                    {t('modal.confirm')}
                </Button>

                <Button className={classes.button} onClick={onClose} color='primary'>
                    {t('modal.close')}
                </Button>
            </DialogActions>
        </React.Fragment>
    );
};

export default R.compose(
    inject('rootStore'),
    withStyles(styles),
    withTranslation()
)(CreateProjectModal);