import React, { Suspense } from 'react';
import ReactDOM from 'react-dom';
import { Router } from 'react-router-dom';
import App from './App';
import './styles.css';
import registerServiceWorker from './registerServiceWorker';
import { Provider } from 'mobx-react';
import RootStore from './stores/rootStore';
import history from './history';
import { I18nextProvider } from 'react-i18next';
import i18n from './i18n';
import CircularProgress from './components/progress/circular.progress';
import Theme from './components/theme';
import { ThemeProvider } from '@material-ui/styles';

ReactDOM.render(
    <ThemeProvider theme={Theme}>
        <Suspense fallback={<CircularProgress />}>
            <I18nextProvider i18n={i18n}>
                <Provider rootStore={new RootStore()}>
                    <Router history={history} basename={process.env.REACT_APP_URL}>
                        <App />
                    </Router>
                </Provider>
            </I18nextProvider>
        </Suspense>
    </ThemeProvider>,
    document.querySelector('#root')
);

registerServiceWorker();
