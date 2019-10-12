import React from 'react';
import { Route } from 'react-router-dom';

export default class App extends React.Component {
    render() {
        return (
            <React.Fragment>
                <Route exact path='/' />
            </React.Fragment>
        );
    }
}
