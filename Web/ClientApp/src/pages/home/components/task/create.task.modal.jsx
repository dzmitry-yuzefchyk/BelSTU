import React, { useEffect } from 'react';
import * as R from 'ramda';
import useFile from '../../../../components/form/useFileForm';
import { createTaskValidator as validator } from '../../../../utils/validators';
import { inject, observer } from 'mobx-react';
import {
    Button,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
    Box,
    TextareaAutosize,
    Select,
    FormControl,
    InputLabel,
    MenuItem
} from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import { withStyles } from '@material-ui/styles';
import { DropzoneArea } from 'material-ui-dropzone';

const types = [
    { title: 'Bug', value: 1 },
    { title: 'Feature', value: 2 },
    { title: 'Improvement', value: 3 }
]

const severities = [
    { title: 'Minor', value: 1 },
    { title: 'Major', value: 2 },
    { title: 'Critical', value: 3 },
    { title: 'Blocker', value: 4 }
];

const priorities = [
    { title: 'Minor', value: 1 },
    { title: 'Major', value: 2 },
    { title: 'Critical', value: 3 },
    { title: 'Blocker', value: 4 }
];

const styles = () => ({
    formInput: {
        width: 275
    },
    button: {
        height: '2.125rem',
        margin: '0.425rem'
    }
});

const CreateTaskModal = (props) => {
    const { taskStore, modalStore } = props.rootStore;
    const { t, classes, callback, projectId, columnId } = props;

    const createTask = async () => {
        const task = new FormData();
        task.set('ColumnId', Number(columnId));
        task.set('ProjectId', Number(projectId));
        task.set('Title', values.title);
        task.set('Content', values.content || '');
        task.set('Priority', values.priority || 0);
        task.set('Severity', values.severity || 0);
        task.set('Type', values.type || 0);
        task.set('AssigneeEmail', values.assigneeEmail);
        for (let file of files) {
            task.append('Attachments', file);
        }
        await taskStore.createTask(task);
        await callback();
        onClose();
    };

    useEffect(() => {
        async function fetchUsers() {
            await taskStore.fetchUsers(Number(projectId));
        }
        fetchUsers();
    }, []);

    const {
        values,
        validationResult,
        handleChange,
        handleSubmit,
        files,
        handleFilesChange
    } = useFile(createTask, validator);

    const onClose = () => {
        modalStore.close();
    }

    return (
        <React.Fragment>
            <DialogTitle>{t('modal.create task')}</DialogTitle>
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

                    <TextareaAutosize
                        className={classes.formInput}
                        placeholder={t('forms.content')}
                        name='content'
                        type='text'
                        onChange={handleChange}
                    />

                    <FormControl className={classes.formInput}>
                        <InputLabel>{t('forms.type')}</InputLabel>
                        <Select
                            name='type'
                            onChange={handleChange}
                            error={!!validationResult.type}
                        >
                            {types.map(type =>
                                <MenuItem key={type.value} value={type.value}>
                                    {t(`task.${type.title}`)}
                                </MenuItem>
                            )}
                        </Select>
                    </FormControl>

                    <FormControl className={classes.formInput}>
                        <InputLabel>{t('forms.severity')}</InputLabel>
                        <Select
                            name='severity'
                            onChange={handleChange}
                            error={!!validationResult.severity}
                        >
                            {severities.map(severity =>
                                <MenuItem key={severity.value} value={severity.value}>
                                    {t(`task.${severity.title}`)}
                                </MenuItem>
                            )}
                        </Select>
                    </FormControl>

                    <FormControl className={classes.formInput}>
                        <InputLabel>{t('forms.priority')}</InputLabel>
                        <Select
                            name='priority'
                            onChange={handleChange}
                            error={!!validationResult.priority}
                        >
                            {priorities.map(priority =>
                                <MenuItem key={priority.value} value={priority.value}>
                                    {t(`task.${priority.title}`)}
                                </MenuItem>
                            )}
                        </Select>
                    </FormControl>


                    <FormControl className={classes.formInput}>
                        <InputLabel>{t('forms.assignee')}</InputLabel>
                        <Select
                            name='assigneeEmail'
                            onChange={handleChange}
                        >
                            {taskStore.users.map(assignee =>
                                <MenuItem key={assignee} value={assignee}>
                                    {assignee}
                                </MenuItem>
                            )}
                        </Select>
                    </FormControl>

                    <DropzoneArea
                        name='files'
                        filesLimit={2}
                        maxFileSize={30000000}
                        onChange={handleFilesChange}
                        acceptedFiles={['image/jpeg', 'image/png', 'image/bmp', 'application/pdf']}
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
    withTranslation(),
    observer
)(CreateTaskModal);