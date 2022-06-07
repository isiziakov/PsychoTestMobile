import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { Redirect } from 'react-router';
import { Authorisation } from './Authorisation';

export class Layout extends Component {
    static displayName = Layout.name;

    constructor(props) {
        super(props);
        this.state = {
            isLoggedIn: ""
        };
    }

    componentWillMount() {
        this.getData();
    }

    async getData() {
        const token = sessionStorage.getItem('tokenKey');
        const response = await fetch("/api/values/getlogin", {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token  // передача токена в заголовке
            }
        });
        if (response.ok === true) {
            {
                this.setState({ isLoggedIn: true }, () => {
                    this.forceUpdate();
                });
            }
        }
        else {
            console.log("Status: ", response.status);
            this.setState({ isLoggedIn: false });
        }
    };

    render() {
        if (this.state.isLoggedIn === false) {
            return (
                <div>
                    < Authorisation />
                </div>
            );
        }
        if (this.state.isLoggedIn === true) {
            return (
                <div>
                    <NavMenu />
                    <Container>
                        {this.props.children}
                    </Container>
                </div>
            );
        }
        if (this.state.isLoggedIn === "")
            return (<h2>Loading...</h2>);
  }
}

