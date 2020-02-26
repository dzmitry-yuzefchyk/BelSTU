import React from 'react';
import * as R from 'ramda';
import useForm from '../../../../components/form/useForm';
import { createProjectFormValidator as validator } from '../../../../utils/validators';
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

const CreateProjectModal = (props) => {
    const { projectStore, modalStore } = props.rootStore;
    const { t, classes, callback } = props;
    const createProject = async () => {
        const project = {
            title: values.title,
            description: values.description || ''
        };
        await projectStore.createProject(project);
        await callback();
        onClose();
    }
    const {
        values,
        validationResult,
        handleChange,
        handleSubmit
    } = useForm(createProject, validator);

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
                        label={t('forms.title')}
                        name='title'
                        type='text'
                        onChange={handleChange}
                        error={!!validationResult.title}
                        helperText={t(`forms.validation.${validationResult.title || 'valid'}`)}
                    />

                    <TextField
                        className={classes.formInput}
                        label={t('forms.description')}
                        name='description'
                        type='text'
                        onChange={handleChange}
                        error={!!validationResult.description}
                        helperText={t(`forms.validation.${validationResult.description || 'valid'}`)}
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
)(CreateProjectModal);