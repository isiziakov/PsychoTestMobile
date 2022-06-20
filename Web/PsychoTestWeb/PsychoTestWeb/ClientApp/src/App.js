import React, { Component } from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Authorisation } from './components/Authorisation';
import './custom.css'
import { Home } from './components/Home';
import { Patients } from './components/Patients';
import { Tests } from './components/Tests';
import Patient from './components/Patient';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Patients} />
                <Route path='/Login' component={Authorisation} />
                <Route path='/Home' component={Home} />
                <Route path='/Tests' component={Tests} />
                <Route path='/Patient/:id' component={Patient} />
            </Layout>
        );
    }
}
