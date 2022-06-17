import React, { Component } from 'react';
import { Button, Input, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, Label, FormText } from 'reactstrap';



export class Tests extends Component {
    static displayName = Tests.name;

    constructor(props) {
        super(props);
        this.state = {
            tests: [],
            searchString: "",
        };
        this.getTests = this.getTests.bind(this);
    }

    componentDidMount() {
        this.getTests();
        this.setState({ searchString: "" });
    }

    async getTests() {
        const token = sessionStorage.getItem('tokenKey');
        var response = await fetch("/api/tests/", {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token
            }
        });
        var data = await response.json();
        if (response.ok === true) {
            this.setState({ tests: [] })
            this.setState({ tests: data });
        }
        else {
            console.log("Error: ", response.status);
        }
    }

    render() {
        return (
            <div>
                <ModalImportTest />
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


