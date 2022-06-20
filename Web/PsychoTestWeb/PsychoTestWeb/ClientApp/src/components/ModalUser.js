import React, { Component } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, Col, Input, Form, FormGroup, Label, FormText } from 'reactstrap';
import { Patients } from './Patients';

export default class ModalUser extends React.Component {
    static displayName = ModalUser.name;
    constructor(props) {
        super(props);
        this.state = {
            modal: false,
            button: "",
            modalHeader: "",
            user: this.props.user,
            isCreate: this.props.isCreate,
            oldName: this.props.user.name,
            oldPasswird: this.props.user.password,
            oldLogin: this.props.user.login,
            oldRole: this.props.user.role
        };
        this.toggle = this.toggle.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
        this.onChangeName = this.onChangeName.bind(this);
        this.onChangePassword = this.onChangePassword.bind(this);
        this.onChangeEmail = this.onChangeEmail.bind(this);
        this.onChangeRole = this.onChangeRole.bind(this);
        this.remove = this.remove.bind(this);
    }

    componentDidMount() {
        if (this.state.isCreate === true)
            this.setState({ button: "Добавить пользователя", modalHeader: "Новый пользователь" });
        else
            this.setState({ button: "Просмотр", modalHeader: "Информация о пользователе" });
    }
    toggle() {
        this.setState({
            modal: !this.state.modal
        });
    }
    onChangeName(e) {
        var tmp = this.state.user;
        tmp.name = e.target.value;
        this.setState({ user: tmp });
    }
    onChangePassword(e) {
        var tmp = this.state.user;
        tmp.password = e.target.value;
        this.setState({ user: tmp });
    }
    onChangeEmail(e) {
        var tmp = this.state.user;
        tmp.login = e.target.value;
        this.setState({ user: tmp });
    }
    onChangeRole(e) {
        var tmp = this.state.user;
        tmp.role = e.target.value;
        this.setState({ user: tmp });
    }

    async onSubmit(event) {
        event.preventDefault();
        const token = sessionStorage.getItem('tokenKey');
        if (this.props.isCreate === false) {
            var response = await fetch("/api/users/" + this.state.user.id, {
                method: "PUT",
                headers: {
                    "Accept": "application/json",
                    "Authorization": "Bearer " + token,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: this.state.user.name,
                    id: this.state.user.id,
                    role: this.state.user.role,
                    password: this.state.user.password,
                    login: this.state.user.login
                })
            });
        }
        else {
            var response = await fetch("/api/users/", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Authorization": "Bearer " + token,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: this.state.user.name,
                    id: this.state.user.id,
                    role: this.state.user.role,
                    password: this.state.user.password,
                    login: this.state.user.login
                })
            });
        }
        if (response.ok !== true) {
            console.log("Error: ", response.status);
        }
        this.props.onClose("/api/users/");
        this.onClose();
    }

    async remove() {
        if (window.confirm("Вы уверены что хотите удалить этого пользователя?")) {
            const token = sessionStorage.getItem('tokenKey');
            var response = await fetch("/api/users/" + this.state.user.id, {
                method: "DELETE",
                headers: {
                    "Accept": "application/json",
                    "Authorization": "Bearer " + token,
                    "Content-Type": "application/json",
                }
            });
            if (response.ok !== true) {
                console.log("Error: ", response.status);
            }
            else {
                this.toggle();
                this.props.onClose("/api/users/");
            }
        }
    }

    onClose() {
        this.toggle();
        this.setState({
            user: {
                name: this.state.oldName,
                password: this.state.oldPassword,
                login: this.state.oldLogin,
                role: this.state.oldRole
            }
        });
    }

    render() {
        return (
            <div>
                <Button color="info" outline={!this.state.isCreate} onClick={this.toggle}>{this.state.button}</Button>
                <Modal size="lg" isOpen={this.state.modal}>
                    <Form onSubmit={this.onSubmit}>
                        <ModalHeader toggle={() => { this.onClose() }}>{this.state.modalHeader}</ModalHeader>
                        <ModalBody>
                            <FormGroup>
                                <Label for="name">Имя пользователя:</Label>
                                <Input type="text" name="name" id="name" placeholder="Введите ФИО" value={this.state.user.name} onChange={this.onChangeName} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="email">Email:</Label>
                                <Input type="email" name="email" id="exampleEmail" placeholder="Введите email" value={this.state.user.login} onChange={this.onChangeEmail} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="password">Пароль:</Label>
                                <Input type="text" name="password" id="password" placeholder="Введите пароль" value={this.state.user.password} onChange={this.onChangePassword} />
                            </FormGroup>
                            <FormGroup>
                                <Label for="role">Роль:</Label>
                                <Input type="select" name="role" id="role" onChange={this.onChangeRole} value={this.state.user.role}>
                                    <option value={'user'}>Пользователь</option>
                                    <option value={'admin'}>Администратор</option>
                                </Input>
                            </FormGroup>
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" onClick={() => this.remove()}>Удалить</Button>
                            <input type="submit" value="Сохранить" className="btn btn-info" />
                        </ModalFooter>
                    </Form>
                </Modal>
            </div>
        );
    }
}
