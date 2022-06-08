import React, { Component } from 'react';
import { Route, Redirect } from 'react-router';
import { Layout } from './components/Layout';
import { Authorisation } from './components/Authorisation';

import './custom.css'
import { Home } from './components/Home';
import { Patients } from './components/Patients';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/Patients' component={Patients} />
                <Route path='/Login' component={Authorisation} />
                <Route path='/Home' component={Home} />
                <Redirect from='/' to='/Patients' />
            </Layout>
        );
    }
}
