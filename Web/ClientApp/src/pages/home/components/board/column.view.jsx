import React from 'react';
import { Paper, Box, withStyles, Typography, Button } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import TaskDetail from './task.detail';
import * as R from 'ramda';

const styles = () => ({
    tasks: {
        flex: '2 1 auto',
        overflowY: 'auto'
    },
    task: {

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
        classes
    } = props;

    const renderAddColumn = () =>
        <Button color='primary'>
            {t('task.create')}
        </Button>;

    return (
        <Paper elevation={2} className={className}>
            <Typography align='center' color='primary' variant='h5'>
                {title}
            </Typography>
            <Box className={classes.tasks}>
                {tasks.map(task =>
                    <TaskDetail
                        className={classes.task}
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
    withTranslation(),
    withStyles(styles)
)(ColumnView);