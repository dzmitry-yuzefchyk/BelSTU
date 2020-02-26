import { useState } from 'react';
import useForm from './useForm';

const useFileForm = (callback, validation) => {
    const [ files, setFiles ] = useState([]);
    const {
        values,
        validationResult,
        handleChange,
        handleSubmit
    } = useForm(callback, validation);

    const handleFilesChange = (files) => {
        setFiles(files);
    };

    return {
        values,
        validationResult,
        handleChange,
        handleSubmit,
        files,
        handleFilesChange
    };
};

export default useFileForm;