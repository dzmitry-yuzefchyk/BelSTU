import React from 'react';
import * as R from 'ramda';
import useForm from '../../../../components/form/useForm';
import { addUserToProjectValidator as validator } from '../../../../utils/validators';
import { inject } from 'mobx-react';
import { Button, DialogActions, DialogContent, DialogTitle, TextField, Box } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import { withStyles } from '@material-ui/styles';

const styles = () => ({
    formInput: {
        width: 275
    },
    button: {
        height: '2.125rem',
        margin: '0.425rem'
    }
});

const AddUserModal = (props) => {
    const { projectStore, modalStore } = props.rootStore;
    const { t, classes, callback, projectId } = props;
    const addUser = async () => {
        const user = {
            projectId: Number(projectId),
            email: values.email
        };
        await projectStore.addToProject(user);
        await callback();
        onClose();
    }

    const {
        values,
        validationResult,
        handleChange,
        handleSubmit
    } = useForm(addUser, validator);

    const onClose = () => {
        modalStore.close();
    }

    return (
        <React.Fragment>
            <DialogTitle>{t('modal.create project')}</DialogTitle>
            <DialogContent>
                <Box display='flex' flexDirection='column'>
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
)(AddUserModal);