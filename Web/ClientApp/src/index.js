import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { Provider } from 'mobx-react';
import RootStore from './stores/rootStore';

const baseUrl = document
    .getElementsByTagName('base')[0]
    .getAttribute('href');

ReactDOM.render(
    <Provider rootStore={new RootStore()}>
        <BrowserRouter basename={baseUrl}>
            <App />
        </BrowserRouter>
    </Provider>,
    document.querySelector('#root')
);

registerServiceWorker();
