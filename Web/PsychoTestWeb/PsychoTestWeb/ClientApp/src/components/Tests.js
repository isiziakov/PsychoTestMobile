import React, { Component } from 'react';
import { Button, Input, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, Label, FormText, Row, Col, InputGroup, InputGroupAddon } from 'reactstrap';
import { CustomPagination } from './Pagination';


export class Tests extends Component {
    static displayName = Tests.name;

    constructor(props) {
        super(props);
        this.state = {
            tests: []
        };
        this.getTests = this.getTests.bind(this);
    }

    componentDidMount() {
        this.getTests("/api/tests/view");
    }

    async getTests(url) {
        const token = sessionStorage.getItem('tokenKey');
        var response = await fetch(url, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token
            }
        });
        var data = await response.json();
        if (response.ok === true) {
            this.setState({ tests: data });
        }
        else {
            console.log("Error: ", response.status);
        }
    }

    render() {
        return (
            <div>
                <br />
                <Row>
                    <Col xs="8"><h2>Список тестов:</h2></Col>
                    <Col xs="auto"><ModalImportTest getTests={this.getTests} /></Col>
                </Row>
                <hr />
                <div>
                    {
                        this.state.tests.map((test) => {
                            return <Test test={test} key={test.id} getTests={this.getTests} />
                        })
                    }
                </div>
            </div>
        );
    }
}

class Test extends React.Component {
    static displayName = Test.name;
    constructor(props) {
        super(props);
        this.state = {};
        this.remove = this.remove.bind(this);
    }

    async remove() {
        if (window.confirm("Вы уверены что хотите удалить этот тест?")) {
            const token = sessionStorage.getItem('tokenKey');
            var response = await fetch("/api/tests/" + this.props.test.id, {
                method: "DELETE",
                headers: {
                    "Authorization": "Bearer " + token
                }
            });
            if (response.ok !== true) {
                console.log("Error: ", response.status);
            }
            else {
                this.props.getTests("/api/tests/view");
            }
        }
    }

    render() {
        return (
            <div>
                <Row>
                    <Col xs="8">{this.props.test.name}</Col>
                    <Col xs="auto"><Button color='danger' outline onClick={this.remove}>Удалить</Button></Col>
                </Row>
                <br />
            </div>
        );
    }
}


class ModalImportTest extends React.Component {
    static displayName = ModalImportTest.name;
    constructor(props) {
        super(props);
        this.state = {
            modal: false,
            files: []
        };

        this.toggle = this.toggle.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
        this.uploadFile = this.uploadFile.bind(this);
    }
    componentDidMount() {
    }
    toggle() {
        this.setState({
            modal: !this.state.modal
        });
    }

    async onSubmit(e) {
        e.preventDefault();
        var save = 0;
        for (var i = 0; i < this.state.files.length; i++) {
            var formData = new FormData();
            formData.append('value', this.state.files[i]);

            const token = sessionStorage.getItem('tokenKey');
            var response = await fetch("/api/tests/", {
                method: "POST",
                headers: {
                    "Authorization": "Bearer " + token
                },
                body: formData
            });
            if (response.ok !== true) {
                console.log("Error: ", response.status);
            }
            else save++;
        }
        if (save === this.state.files.length) {
            this.toggle();
            this.props.getTests("/api/tests/view");
        }
    }

    uploadFile(e) {
        this.setState({ files: [] });
        this.setState({ files: e.target.files });
    }

    render() {
        return (
            <div>
                <Button color="info" onClick={this.toggle}>Импортировать</Button>
                <Modal isOpen={this.state.modal}>
                    <Form onSubmit={(e) => { this.onSubmit(e) }} encType="multipart/form-data">
                        <ModalHeader toggle={this.toggle}>Импорт нового теста</ModalHeader>
                        <ModalBody>
                            <FormGroup>
                                <Label for="file">Файл:</Label>
                                <Input type="file" name="file" accept=".xml" id="file" multiple onChange={this.uploadFile} />
                                <FormText color="muted">
                                    Прикрепите один или несколько файлов с тестом в формате xml.
                                </FormText>
                            </FormGroup>
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" onClick={this.toggle}>Отмена</Button>
                            <input type="submit" value="Сохранить" className="btn btn-info" />
                        </ModalFooter>
                    </Form>
                </Modal>
            </div>
        );
    }
}


