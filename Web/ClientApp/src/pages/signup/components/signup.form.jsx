import { inject } from 'mobx-react';
import jdenticon from 'jdenticon';
import { useForm } from './../../../components/form/useForm';

const SignUpForm = (props) => {
    const {
        handleChange,
        handleSubmit,
        values,
        validationResult
    } = useForm(signUp, validator);
    const { userStore } = props.rootStore;

    const signIn = async () => {
        const icon = jdenticon.toSvg(values.email, 300);
        const svgString = new XMLSerializer().serializeToString(icon);
        const decoded = unescape(encodeURIComponent(svgString));
        const base64 = btoa(decoded);
        const imgSource = `data:image/svg+xml;base64,${base64}`;
        const user = {
            email: values.email,
            password: values.password,
            icon: imgSource
        };
        await userStore.signUp(user);
    };

    return (
        <div/>
    );
}

export default inject('rootStore')(observer(SignUpForm));