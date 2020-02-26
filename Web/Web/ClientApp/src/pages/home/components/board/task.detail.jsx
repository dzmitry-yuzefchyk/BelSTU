import React from 'react';
import { withTranslation } from 'react-i18next';
import { Card, CardHeader, CardContent, CardActions, Box, withStyles, Button } from '@material-ui/core';
import { withRouter } from 'react-router-dom';
import * as R from 'ramda';
import { inject, observer } from 'mobx-react';
import ViewTaskModal from '../task/view.task.modal';
import clsx from 'clsx';

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
    img: {
        maxWidth: 24,
        maxHeight: 24,
        borderRadius: '100px',
        backgroundColor: theme.palette.primary.contrastText
    },
    dragged: {
        pointerEvents: 'none'
    }
});

const TaskDetail = (props) => {
    const {
        t,
        id,
        title,
        type,
        severity,
        priority,
        assigneeTag,
        assigneeIcon,
        creatorTag,
        creatorIcon,
        className,
        classes,
        rootStore,
        onDragStart,
        columnId,
        disabled
    } = props;
    const { projectId, boardId } = props.match.params;
    const { modalStore } = rootStore;

    const onTaskView = () => {
        modalStore.show(<ViewTaskModal taskId={id} projectId={projectId} />);
    }

    const dragStart = event => {
        onDragStart(event, id, columnId);
    }

    return (
        <Card onDragStart={dragStart} className={clsx(className, { [classes.dragged]: disabled })} draggable>
            <CardHeader
                title={title}
            />
            <CardContent>
                <Box display='flex' flexDirection='column'>
                    <Box>
                        {t('task.assignee')}:
                        <img
                            alt={assigneeTag ? `@${assigneeTag}` : ''}
                            className={classes.img}
                            src={assigneeIcon ? assigneeIcon : ''}
                        />
                    </Box>
                    <Box>
                        {t('task.creator')}:
                        <img
                            alt={creatorTag ? `@${creatorTag}` : ''}
                            className={classes.img}
                            src={creatorIcon ? creatorIcon : ''}
                        />
                    </Box>
                    <Box>
                        {t('task.type')}:
                        {t(`task.${types.find(x => x.value === type).title}`)}
                    </Box>
                    <Box>
                        {t('task.severity')}:
                        {t(`task.${severities.find(x => x.value === severity).title}`)}
                    </Box>
                    <Box>
                        {t('task.priority')}:
                        {t(`task.${priorities.find(x => x.value === priority).title}`)}
                    </Box>
                </Box>
            </CardContent>
            <CardActions>
                <Button color='primary' variant='outlined' onClick={onTaskView}>
                    {t('task.view')}
                </Button>
            </CardActions>
        </Card>
    );
}

export default R.compose(
    inject('rootStore'),
    withRouter,
    withStyles(styles),
    withTranslation(),
    observer
)(TaskDetail);