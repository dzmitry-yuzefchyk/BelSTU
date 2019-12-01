import React from 'react';
import { withTranslation } from 'react-i18next';
import { Paper } from '@material-ui/core';
import * as R from 'ramda';

const TaskDetail = (props) => {
    const {
        id,
        title,
        type,
        severity,
        priority,
        assigneeTag,
        assigneeIcon,
        creatorTag,
        creatorIcon,
        className
    } = props;

    return (
        <Paper className={className}>
            Task
        </Paper>
    );
}

export default R.compose(
    withTranslation()
)(TaskDetail);