import React from 'react';
import ReactDOM from 'react-dom';
import { Router } from 'react-router-dom';
import App from './App';
import './styles.css';
import registerServiceWorker from './registerServiceWorker';
import { Provider } from 'mobx-react';
import RootStore from './stores/rootStore';
import history from './history';


ReactDOM.render(
    <Provider rootStore={new RootStore()}>
        <Router history={history} basename={process.env.REACT_APP_URL}>
            <App />
        </Router>
    </Provider>,
    document.querySelector('#root')
);

registerServiceWorker();
