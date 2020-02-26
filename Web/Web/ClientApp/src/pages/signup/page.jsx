import React from 'react';
import SignUpForm from './components/signup.form';
import CenteredLayout from '../../components/layout/centered.layout';
import { observer } from 'mobx-react';

const SignUpPage = () => (
    <CenteredLayout useBg>
        <SignUpForm />
    </CenteredLayout>
);

export default observer(SignUpPage);