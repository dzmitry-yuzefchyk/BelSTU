import React, { useEffect, useState } from 'react';
import * as R from 'ramda';
import { inject, observer } from 'mobx-react';
import useForm from './../../../../components/form/useForm';
import { withStyles, Box, Paper, Button, TextField, Checkbox, Select, MenuItem, FormControl, InputLabel, FormControlLabel } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import { withRouter } from 'react-router-dom';
import { updateProjectSettingsValidator as validator } from './../../../../utils/validators';
import { HOME } from './../../../../utils/routes';

const AccessLevel = [
    { title: 'PROJECT_CREATOR', value: 0 },
    { title: 'PROJECT_MEMBER', value: 1 }
];

const styles = theme => ({
    form: {
        padding: 20
    },
    paper: {
        width: 400,
        padding: '1.25rem'
    },
    formInputBox: {
        display: 'flex',
        justifyContent: 'center',
        paddingTop: '0.225rem',
        paddingBottom: '0.225rem'
    },
    button: {
        height: '2.125rem',
        margin: '0.425rem'
    }
});

const ProjectSettingsForm = props => {
    const { t, classes, projectId, history } = props;
    const { projectStore } = props.rootStore;
    const { settings } = projectStore;
    const submit = async () => {
        const projectSettings = {
            id: Number(projectId),
            title: values.title,
            description: values.description,
            useAdvancedSecuritySettings: Boolean(values.useAdvancedSecuritySettings),
            accessToChangeBoard: values.accessToChangeBoard,
            accessToChangeProject: values.accessToChangeProject,
            accessToChangeTask: values.accessToChangeTask
        }
        await projectStore.updateSettings(projectSettings);
    };
    const [askToDelete, setAskToDelete] = useState(false);

    const handleDelete = () => {
        setAskToDelete(!askToDelete);
    };

    const deleteProject = async () => {
        await projectStore.deleteProject(projectId);
        history.push(HOME);
    };

    const {
        handleChange,
        handleSubmit,
        values,
        validationResult
    } = useForm(submit, validator);

    useEffect(() => {
        values.title = settings.title;
        values.description = settings.description;
        values.useAdvancedSecuritySettings = settings.useAdvancedSecuritySettings;
        values.accessToChangeBoard = settings.accessToChangeBoard;
        values.accessToChangeProject = settings.accessToChangeProject;
        values.accessToChangeTask = settings.accessToChangeTask;
    }, []);

    return (
        <Box display='flex' flexDirection='row'>
            <Box className={classes.form}>
                <Paper elevation={2} className={classes.paper}>

                    <FormControl className={classes.formInputBox}>
                        <TextField
                            value={values.title || settings.title}
                            className={classes.formInput}
                            label={t('forms.title')}
                            name='title'
                            type='text'
                            onChange={handleChange}
                            error={!!validationResult.title}
                            helperText={t(`forms.validation.${validationResult.title || 'valid'}`)}
                        />
                    </FormControl>

                    <FormControl className={classes.formInputBox}>
                        <TextField
                            value={values.description || settings.description}
                            className={classes.formInput}
                            label={t('forms.description')}
                            name='description'
                            type='text'
                            onChange={handleChange}
                        />
                    </FormControl>

                    <FormControlLabel
                        control={
                            <Checkbox
                                value={values.useAdvancedSecuritySettings || settings.useAdvancedSecuritySettings}
                                name='useAdvancedSecuritySettings'
                                onChange={handleChange}
                            />
                        }
                        label={t('forms.use advanced settings')}
                    />

                    <FormControl className={classes.formInputBox}>
                        <InputLabel>{t('forms.accessToChangeProject')}</InputLabel>
                        <Select
                            value={values.accessToChangeProject || settings.accessToChangeProject}
                            name='accessToChangeProject'
                            onChange={handleChange}
                        >
                            {AccessLevel.map(level =>
                                <MenuItem key={level.value} value={level.value}>
                                    {t(`access.${level.title}`)}
                                </MenuItem>
                            )}
                        </Select>
                    </FormControl>

                    <FormControl className={classes.formInputBox}>
                        <InputLabel>{t('forms.accessToChangeBoard')}</InputLabel>
                        <Select
                            value={values.accessToChangeBoard || settings.accessToChangeBoard}
                            name='accessToChangeBoard'
                            onChange={handleChange}
                        >
                            {AccessLevel.map(level =>
                                <MenuItem key={level.value} value={level.value}>
                                    {t(`access.${level.title}`)}
                                </MenuItem>
                            )}
                        </Select>
                    </FormControl>

                    <FormControl className={classes.formInputBox}>
                        <InputLabel>{t('forms.accessToChangeTask')}</InputLabel>
                        <Select
                            value={values.accessToChangeTask || settings.accessToChangeTask}
                            name='accessToChangeTask'
                            onChange={handleChange}
                        >
                            {AccessLevel.map(level =>
                                <MenuItem key={level.value} value={level.value}>
                                    {t(`access.${level.title}`)}
                                </MenuItem>
                            )}
                        </Select>
                    </FormControl>

                    <Box className={classes.formInputBox} display='flex' justifyContent='space-around' flexDirection='row'>
                        <Button className={classes.button} variant='contained' color='primary' onClick={handleSubmit}>
                            {t('forms.submit')}
                        </Button>
                    </Box>
                </Paper>
            </Box>

            <Box className={classes.form}>
                <Paper elevation={2} className={classes.paper}>

                    <Box className={classes.formInputBox} display='flex' justifyContent='space-around' flexDirection='row'>
                        <Button className={classes.button} variant='contained' color='secondary' onClick={handleDelete}>
                            {t('forms.delete project')}
                        </Button>
                    </Box>

                    {askToDelete
                        ? <Box className={classes.formInputBox} display='flex' justifyContent='space-around' flexDirection='row'>
                            <Button className={classes.button} variant='contained' color='secondary' onClick={deleteProject}>
                                {t('forms.really delete project')}
                            </Button>

                            <Button className={classes.button} variant='contained' color='primary' onClick={handleDelete}>
                                {t('forms.changed my mind')}
                            </Button>
                        </Box>
                        : null
                    }
                </Paper>
            </Box>
        </Box>
    );
}

export default R.compose(
    inject('rootStore'),
    withTranslation(),
    withRouter,
    withStyles(styles),
    observer,
)(ProjectSettingsForm);
