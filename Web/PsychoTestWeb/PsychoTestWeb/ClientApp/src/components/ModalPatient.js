//ModalComponent.js
import React, { Component } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, Col, Input, Form, FormGroup, Label, FormText } from 'reactstrap';
import { Patients } from './Patients';

export default class ModalPatient extends React.Component {
    static displayName = ModalPatient.name;
    constructor(props) {
        super(props);
        this.state = {
            modal: false,
            isCreate: props.isCreate,
            patient: props.patient,
            name: props.patient.name,
            prescribedTests: [],
            tests: [],
            availableTests: [],
            results: [],
            addedTest: 0,
            isPrescribedTests: "",
            isResults: "",
            allRequestIsDone: 0,
            button: ""
        };

        this.toggle = this.toggle.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
        this.onChangeName = this.onChangeName.bind(this);
        this.onTestsChange = this.onTestsChange.bind(this);
        this.onCommentChange = this.onCommentChange.bind(this);
        this.onChangeCheckbox = this.onChangeCheckbox.bind(this);
        this.remove = this.remove.bind(this);
    }
    componentDidMount() {
        if (this.state.isCreate === true)
            this.setState({ button: "Добавить пациента" });
        else
            this.setState({ button: "Просмотр" });
        this.getTests();
        this.getResults();
    }
    toggle() {
        this.setState({
            modal: !this.state.modal
        });
    }
    onChangeName(e) {
        this.setState({ name: e.target.value });
    }
    onTestsChange(e) {
        this.setState({ addedTest: e.target.value });
    }
    onCommentChange(e, resultId) {
        var tmp = this.state.results;
        tmp.map((result) => {
            if (result.id === resultId)
                result.comment = e.target.value;
        });
        this.setState({ results: tmp });
    }
    addTest() {
        var tmp = this.state.prescribedTests;
        this.state.tests.map((test) => {
            if (test.id === this.state.addedTest) {
                tmp.push({ name: test.name, id: test.id, isChecked: true });
                this.setState({ prescribedTests: tmp });
                tmp = this.state.availableTests;
                for (var i = 0; i < this.state.availableTests.length; i++) {
                    if (this.state.availableTests[i].id === this.state.addedTest) {
                        if (i + 1 !== this.state.availableTests.length)
                            this.setState({ addedTest: this.state.availableTests[i + 1].id });
                        else
                            this.setState({ addedTest: 0 });
                        tmp.splice(i, 1);
                        this.setState({ isPrescribedTests: "" });
                    }
                }
                this.setState({ availableTests: tmp });
            }
        });
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
            this.setState({ tests: data });
            var a = this.state.allRequestIsDone + 1;
            this.setState({ allRequestIsDone: a });
            this.allRequestIsDone();
        }
        else {
            console.log("Error: ", response.status);
        }
    }

    async getResults() {
        const token = sessionStorage.getItem('tokenKey');
        if (this.state.patient.id !== "") {
            var response = await fetch("/api/Results/patientsResults/" + this.state.patient.id, {
                method: "GET",
                headers: {
                    "Accept": "application/json",
                    "Authorization": "Bearer " + token
                }
            });
            var data = await response.json();
            if (response.ok === true) {
                this.setState({ results: data });
                if (data.length === 0)
                    this.setState({ isResults: "Ни один тест еще не пройден!" });
                var a = this.state.allRequestIsDone + 1;
                this.setState({ allRequestIsDone: a });
                this.allRequestIsDone();
            }
            else {
                console.log("Error: ", response.status);
            }
        }
        else {
            this.setState({ isResults: "Ни один тест еще не пройден!", results: [] });
            var a = this.state.allRequestIsDone + 1;
            this.setState({ allRequestIsDone: a });
            this.allRequestIsDone();
        }
    }

    allRequestIsDone() {
        if (this.state.allRequestIsDone == 2) {
            this.addPrescribedTests();
            this.addNameTestsForResults();
        }
    }

    addPrescribedTests() {
        this.setState({ availableTests: [], prescribedTests: [] }, () => {
            if (this.state.patient.tests !== null && this.state.patient.tests !== undefined) {
                this.state.patient.tests.map((testId) => {
                    this.state.tests.map((test) => {
                        if (testId === test.id) {
                            var tmp = this.state.prescribedTests;
                            tmp.push({ name: test.name, id: test.id, isChecked: true });
                            this.setState({ prescribedTests: tmp });
                        }
                        else {
                            var tmp = this.state.availableTests;
                            tmp.push({ name: test.name, id: test.id });
                            this.setState({ availableTests: tmp });
                        }
                    });
                });
                if (this.state.patient.tests.length === 0) {
                    this.setState({ availableTests: this.state.tests, isPrescribedTests: "Тестов пока нет!" });
                }
            }
            else
                this.setState({ availableTests: this.state.tests, isPrescribedTests: "Тестов пока нет!" });
        });
    }

    addNameTestsForResults() {
        var tmp = this.state.results;
        for (var i = 0; i < tmp.length; i++)
            this.state.tests.map((test) => {
                if (tmp[i].test === test.id)
                    tmp[i].name = test.name;
            });
        this.setState({ results: tmp });
    }

    onChangeCheckbox(testId, value) {
        var tmp = this.state.prescribedTests;
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].id === testId) {
                tmp[i].isChecked = value;
            }
        }
        this.setState({ prescribedTests: tmp });
    }

    async onSubmit(event) {
        event.preventDefault();
        const token = sessionStorage.getItem('tokenKey');
        var tests = [];
        this.state.prescribedTests.map((test) => {
            if (test.isChecked === true)
                tests.push(test.id);
        });

        if (this.state.isCreate === false) {
            var response = await fetch("/api/patients/" + this.state.patient.id, {
                method: "PUT",
                headers: {
                    "Accept": "application/json",
                    "Authorization": "Bearer " + token,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: this.state.name,
                    id: this.state.patient.id,
                    tests: tests
                })
            });
        }
        else {
            var response = await fetch("/api/patients/", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Authorization": "Bearer " + token,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: this.state.name,
                    tests: tests
                })
            });
        }
        if (response.ok !== true) {
            console.log("Error: ", response.status);
        }
        this.state.results.map((result) => {
            this.putResults(result);
        });
        this.setState({ name: "", availableTests: this.state.tests, prescribedTests: [], isPrescribedTests: "Тестов пока нет!" });
        this.props.onClose("/api/patients/");
        this.toggle();
    }

    async putResults(result) {
        const token = sessionStorage.getItem('tokenKey');
        var response = await fetch("/api/results/" + result.id, {
            method: "PUT",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                id: result.id,
                patient: result.patient,
                test: result.test,
                result: result.result,
                comment: result.comment
            })
        });
        if (response.ok !== true) {
            console.log("Error: ", response.status);
        }
    }

    async remove() {
        if (window.confirm("Вы уверены что хотите удалить этого пациента?")) {
            const token = sessionStorage.getItem('tokenKey');
            var response = await fetch("/api/patients/" + this.state.patient.id, {
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
                this.props.onClose("/api/patients/");
            }
        }
    }

    onClose() {
        this.toggle();
        var a = this.state.allRequestIsDone - 1;
        this.setState({ allRequestIsDone: a });
        this.getResults();
        this.setState({ name: this.state.patient.name, results: [] });
    }

    render() {
        return (
            <div>
                <Button color="info" onClick={this.toggle}>{this.state.button}</Button>
                <Modal size="lg" isOpen={this.state.modal}>
                    <Form onSubmit={this.onSubmit}>
                        <ModalHeader toggle={() => { this.onClose() }}>Информация о пациенте:</ModalHeader>
                        <ModalBody>
                            <FormGroup>
                                <Label for="name">Имя:</Label>
                                <Input id="name" required value={this.state.name} onChange={this.onChangeName} placeholder="ФИО" />
                            </FormGroup>

                            <FormGroup>
                                <Label for="newTest">Назначить тест</Label>
                                <Row>
                                    <Col xs="9">
                                        <Input type="select" name="select" defaultValue={'0'} onChange={this.onTestsChange} id="newTest">
                                            <option value="0" disabled>Выберите тест</option>
                                            {
                                                this.state.availableTests.map((test) => {
                                                    return (
                                                        <option key={test.id} value={test.id}>{test.name}</option>
                                                    );
                                                })
                                            }
                                        </Input>
                                    </Col>
                                    <Col xs="auto">
                                        <Button color="info" outline onClick={() => this.addTest()}>Назначить</Button>
                                    </Col>
                                </Row>
                            </FormGroup>

                            <FormGroup>
                                <h5>Назначенные тесты:</h5>
                                <p>{this.state.isPrescribedTests}</p>
                                <div>
                                    {
                                        this.state.prescribedTests.map((test) => {
                                            return (
                                                <FormGroup check key={test.id}>
                                                    <Label check>
                                                        <Input type="checkbox" value={test.id} checked={test.isChecked} onChange={(e) => { this.onChangeCheckbox(test.id, e.target.checked) }} />{test.name}</Label>
                                                </FormGroup>
                                            );
                                        })
                                    }
                                </div>
                            </FormGroup>

                            <h5>Пройденные тесты:</h5>
                            <p>{this.state.isResults}</p>
                            {
                                this.state.results.map((result) => {
                                    return (
                                        <FormGroup key={result.id}>
                                            <strong>{result.name}</strong>
                                            <br />
                                            <>Результат: {result.result}</>
                                            <br />
                                            <Label for="comment">Комментарий:</Label>
                                            <Input type="textarea" name="text" id="comment" value={result.comment} onChange={(e) => { this.onCommentChange(e, result.id) }} />
                                        </FormGroup>
                                    );
                                    <br />
                                })
                            }

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
