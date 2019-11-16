import React from 'react';
import { useParams, useLocation } from 'react-router-dom'
import ConfirmEmailComponent from './components/confirm.email';

const ConfirmEmailPage = () => {
    const { email } = useParams();
    const query = new URLSearchParams(useLocation().search);
    const token = query.get('token');

    return <ConfirmEmailComponent email={email} token={token}/>
};

export default ConfirmEmailPage;