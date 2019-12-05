import React, { useEffect } from 'react';
import * as R from 'ramda';
import { inject, observer } from 'mobx-react';
import {
    Button,
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

const styles = theme => ({
    formInput: {
        width: 275
    },
    button: {
        height: '2.125rem',
        margin: '0.425rem'
    },
    img: {
        maxWidth: 24,
        maxHeight: 24,
        borderRadius: '100px',
        backgroundColor: theme.palette.primary.contrastText
    },
});

const ViewTaskModal = (props) => {
    const { taskStore, modalStore } = props.rootStore;
    const { t, classes, projectId, taskId } = props;

    useEffect(() => {
        async function fetchTask() {
            await taskStore.fetchTask(Number(projectId), Number(taskId));
        }
        fetchTask();
    }, []);

    const onDownload = attachmentId => async event => {
        await taskStore.downloadAttachment(Number(projectId), Number(attachmentId));
    };

    return (
        <React.Fragment>
            <DialogTitle>{taskStore.task.title}</DialogTitle>
            <DialogContent>
                <Box display='flex' flexDirection='column'>
                    <Box>
                        {taskStore.task.content}
                    </Box>

                    <Box>
                        {t('task.assignee')}:
                        <img
                            alt={taskStore.task.assigneeTag ? `@${taskStore.task.assigneeTag}` : ''}
                            className={classes.img}
                            src={taskStore.task.assigneeIcon ? taskStore.task.assigneeIcon : ''}
                        />
                    </Box>
                    <Box>
                        {t('task.creator')}:
                        <img
                            alt={taskStore.task.creatorTag ? `@${taskStore.task.creatorTag}` : ''}
                            className={classes.img}
                            src={taskStore.task.creatorIcon ? taskStore.task.creatorIcon : ''}
                        />
                    </Box>

                    <FormControl className={classes.formInput}>
                        <InputLabel>{t('forms.type')}</InputLabel>
                        <Select
                            disabled={true}
                            value={taskStore.task.type}
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
                            disabled={true}
                            value={taskStore.task.severity}
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
                            disabled={true}
                            value={taskStore.task.priority}
                        >
                            {priorities.map(priority =>
                                <MenuItem key={priority.value} value={priority.value}>
                                    {t(`task.${priority.title}`)}
                                </MenuItem>
                            )}
                        </Select>
                    </FormControl>

                    <Box display='flex' flexDirection='column'>
                        {taskStore.task.attachments.map(attachment =>
                            <Button onClick={onDownload(attachment.id)}>
                                {attachment.name}
                            </Button>
                        )}
                    </Box>
                </Box>
            </DialogContent>
        </React.Fragment>
    );
};

export default R.compose(
    inject('rootStore'),
    withStyles(styles),
    withTranslation(),
    observer
)(ViewTaskModal);