import React from 'react';
import { inject } from 'mobx-react';
import CircularProgress from './../../../components/progress/circular.progress';
import PropTypes from 'prop-types';
import { withRouter } from 'react-router-dom';
import { SIGN_IN } from './../../../utils/routes';

@inject('rootStore')
@withRouter
class ConfirmEmailComponent extends React.Component {
    async componentDidMount() {
        const { email, token, history } = this.props;
        const { userStore } = this.props.rootStore;

        await userStore.confirmEmail(email, token);
        history.push(SIGN_IN);
    }

    render() {
        return (
            <CircularProgress/>
        );
    }
}

ConfirmEmailComponent.propTypes = {
    email: PropTypes.string.isRequired,
    token: PropTypes.string.isRequired
};

export default ConfirmEmailComponent;