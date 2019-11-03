import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { Provider } from 'mobx-react';

const baseUrl: string | null = document
    .getElementsByTagName('base')[0]
    .getAttribute('href');

ReactDOM.render(
    <Provider>
        <BrowserRouter basename={baseUrl || undefined}>
            <App />
        </BrowserRouter>
    </Provider>,
    document.querySelector('#root')
);

registerServiceWorker();
