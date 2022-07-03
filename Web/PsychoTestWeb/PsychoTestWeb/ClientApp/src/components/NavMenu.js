import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link, useHistory  } from 'react-router-dom';
import './NavMenu.css';

const LogOut = () => {
    let history = useHistory();

    const goToHome = () => {
        history.push("/");
        sessionStorage.removeItem('tokenKey');
        window.location.reload();
    }

    return (
        <NavItem>
             <NavLink tag={Link} className="text-dark" to="/" onClick={goToHome}>Выход</NavLink>
        </NavItem>
    );
}

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }


    LogOut = () => {
        const { history } = this.props;
        if(history) history.push('/');
        sessionStorage.removeItem('tokenKey');
        window.location.reload();
    }

    render() {
        const { history } = this.props;
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/">PsychoTestWeb</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Пациенты</NavLink>
                                </NavItem>
                                <LogOut />
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}