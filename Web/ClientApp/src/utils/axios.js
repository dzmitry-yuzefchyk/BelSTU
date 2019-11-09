import axios from 'axios';

export async function GET(url, content = 'application/json') {
    return await axiosRequest('GET', url, null, content);
}

export async function POST(url, data = '', content = 'application/json') {
    return await axiosRequest('POST', url, data, content);
}

export async function PUT(url, data = '', content = 'application/json') {
    return await axiosRequest('PUT', url, data, content);
}

export async function DELETE(url, content = 'application/json') {
    return await axiosRequest('DELETE', url, null, content);
}

async function axiosRequest(method, url, data, contentType) {
    const axiosConfig = {
        headers: {
            'Content-Type': contentType
        },
        method,
        data,
        withCredentials: true
    };

    return await axios(url, axiosConfig);
}