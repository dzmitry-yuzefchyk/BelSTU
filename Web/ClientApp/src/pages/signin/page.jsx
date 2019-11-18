import React from 'react';
import SignInForm from './components/signin.form';
import CenteredLayout from '../../components/layout/centered.layout';
import { observer } from 'mobx-react';

const SignInPage = () => (
    <CenteredLayout useBg>
        <SignInForm />
    </CenteredLayout >
);

export default observer(SignInPage);