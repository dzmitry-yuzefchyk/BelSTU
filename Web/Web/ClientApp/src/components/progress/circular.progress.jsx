import React from 'react';
import CenteredLayout from './../layout/centered.layout';
import { CircularProgress as MUICircularProgress } from '@material-ui/core';

const CircularProgress = () => (
    <CenteredLayout>
        <MUICircularProgress/>
    </CenteredLayout>
);

export default CircularProgress;