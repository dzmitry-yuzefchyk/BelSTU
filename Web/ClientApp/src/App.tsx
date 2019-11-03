import React from 'react';
import { Route } from 'react-router-dom';

export default class App extends React.Component {
    public render(): React.ReactNode {
        return (
            <React.Fragment>
                <Route exact path={'/'} />
            </React.Fragment>
        );
    }
}
