import { useState, useEffect } from 'react';

const useForm = (callback, validation) => {
    const [ values, setValues ] = useState({});
    const [ validationResult, setValidationResult ] = useState({});
    const [ isSubmitting, setIsSubmitting ] = useState(false);

    useEffect(() => {
        async function submitForm() {
            if (Object.keys(validationResult).length === 0 && isSubmitting) {
                await callback();
            }
        };
        submitForm();
        setIsSubmitting(false);
    }, [ validationResult, isSubmitting, callback ]);

    const handleSubmit = (event) => {
        if (event) event.preventDefault();

        setValidationResult(validation(values));
        setIsSubmitting(true);
    };

    const handleChange = (event) => {
        event.persist();
        let newState = {};
        if (event.target.name === 'useAdvancedSecuritySettings') {
            newState = {
                ...values,
                [event.target.name]: event.target.checked
            };
        } else {
            newState = {
                ...values,
                [event.target.name]: event.target.value
            };
        }
        setValues(newState);
        setValidationResult(validationResult => ({
            ...validationResult,
            [event.target.name]: validation(newState)[event.target.name]
        }));
    };

    return {
        handleChange,
        handleSubmit,
        values,
        validationResult
    };
};

export default useForm;