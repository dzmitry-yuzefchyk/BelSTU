import React, { useState } from 'react';
import { Paper, Box, withStyles, Typography, Button } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import TaskDetail from './task.detail';
import { inject, observer } from 'mobx-react';
import * as R from 'ramda';
import CreateTaskModal from '../task/create.task.modal';

const styles = () => ({
    tasks: {
        flex: '2 1 auto',
        overflowY: 'auto'
    },
    root: {
        margin: 10
    },
    taskHolder: {
        background: 'blue',
        opacity: '0.4',
        height: 100,
        width: '100%',
        pointerEvents: 'none'
    }
});

const ColumnView = (props) => {
    const {
        id,
        position,
        title,
        tasks,
        canCreateTask,
        className,
        t,
        classes,
        rootStore,
        projectId,
        callback,
        boardId
    } = props;
    const { modalStore, boardStore, taskStore } = rootStore;
    const [taskOver, setTaskOver] = useState(false);

    const openAddTaskModal = () => {
        modalStore.show(<CreateTaskModal columnId={id} projectId={projectId} callback={callback} />);
    };

    const renderAddColumn = () =>
        <Button color='primary' onClick={openAddTaskModal}>
            {t('task.create')}
        </Button>;

    function onTaskDragEnd() {
        setTaskOver(false);
    }

    function onTaskDragStart(event, taskId, columnId) {
        const data = {
            taskId,
            columnId
        };
        event.dataTransfer.setData('text/plain', JSON.stringify(data));
    }

    function onTaskOver(event) {
        event.preventDefault();
    }

    async function onTaskDrop(event) {
        event.preventDefault();
        const data = JSON.parse(event.dataTransfer.getData('text/plain'));
        if (id !== data.columnId) {
            await taskStore.moveTask(projectId, data.taskId, id);
            setTaskOver(false);
            await boardStore.fetchBoard(boardId, projectId);
        }
        setTaskOver(false);
    }

    function onTaskEnter() {
        setTaskOver(true);
    }

    function onTaskLeave() {
        setTaskOver(false);
    }

    return (
        <Paper
            elevation={2}
            className={className}
            onDrop={onTaskDrop}
            onDragOver={onTaskOver}
            onDragEnter={onTaskEnter}
            onDragLeave={onTaskLeave}
        >
            <Typography align='center' color='primary' variant='h5'>
                {title}
            </Typography>
            {taskOver
                ? <Box className={classes.taskHolder}></Box>
                : null
            }

            <Box className={classes.tasks}>
                {tasks.map(task =>
                    <TaskDetail
                        disabled={taskOver}
                        onDragStart={onTaskDragStart}
                        onDragEnd={onTaskDragEnd}
                        columnId={id}
                        className={classes.root}
                        key={task.id}
                        id={task.id}
                        title={task.title}
                        type={task.type}
                        severity={task.severity}
                        priority={task.priority}
                        assigneeTag={task.assigneeTag}
                        assigneeIcon={task.assigneeIcon}
                        creatorTag={task.creatorTag}
                        creatorIcon={task.creatorIcon}
                    />
                )}
            </Box>
            {canCreateTask
                ? renderAddColumn()
                : null
            }
        </Paper>
    );
}

export default R.compose(
    inject('rootStore'),
    withTranslation(),
    withStyles(styles),
    observer
)(ColumnView);