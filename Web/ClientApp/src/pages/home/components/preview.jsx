import React from 'react';
import CenteredLayout from '../../../components/layout/centered.layout';
import { Paper, Button } from '@material-ui/core';
import { withRouter } from 'react-router-dom';
import { withTranslation } from 'react-i18next';
import { SIGN_IN } from '../../../utils/routes';

const Preview = (props) => {
    const { history, t } = props;

    const toSignIn = () => {
        history.push(SIGN_IN);
    }

    return (
        <CenteredLayout>
            <Paper>
                {t('preview.You can do something, but you need to')}
                <Button color='primary' onClick={toSignIn}>
                    {t('forms.signIn')}
                </Button>
            </Paper>
        </CenteredLayout>
    );
};

export default withRouter(withTranslation()(Preview));