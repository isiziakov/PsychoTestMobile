import React, { Component } from 'react';
import { Button, Row, Col, Input, Form, FormGroup, Label, Alert, InputGroup, InputGroupAddon } from 'reactstrap';

export default class Patient extends React.Component {
    static displayName = Patient.name;
    constructor(props) {
        super(props);
        this.state = {
            patientId: props.match.params.id,
            patient: [],
            name: "",
            allTests: [],
            prescribedTests: [],
            availableTests: [],
            patientResults: [],
            addedTest: 0,
            arePrescribedTests: "",
            areResults: "",
            isPatient: "",
            alertVisible: false
        };

        this.onSubmit = this.onSubmit.bind(this);
        this.onChangeName = this.onChangeName.bind(this);
        this.onTestChange = this.onTestChange.bind(this);
        this.onCommentChange = this.onCommentChange.bind(this);
        this.onChangeCheckbox = this.onChangeCheckbox.bind(this);
        this.remove = this.remove.bind(this);
        this.onChangeAlert = this.onChangeAlert.bind(this);
    }

    componentDidMount() {
        this.getPatient(this.state.id);
    }
    async getPatient() {
        const token = sessionStorage.getItem('tokenKey');
        var response = await fetch("/api/patients/" + this.state.patientId, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token
            }
        });
        var data = await response.json();
        if (response.ok === true) {
            this.setState({ patient: data, patientResults: data.results, name: data.name }, () => {
                this.getTests();
            });
        }
        else {
            this.setState({ isPatient: "Данного пациента не существует!" });
            console.log("Error: ", response.status);
        }
    }
    onChangeName(e) {
        this.setState({ name: e.target.value });
    }
    onTestChange(e) {
        this.setState({ addedTest: e.target.value });
    }
    onCommentChange(e, resultIndex) {
        var tmp = this.state.patientResults;
        tmp[resultIndex].comment = e.target.value;
        this.setState({ patient: tmp });
    }
    onChangeAlert(value) {
        this.setState({ alertVisible: value });
    }
    async getTests() {
        const token = sessionStorage.getItem('tokenKey');
        var response = await fetch("/api/tests/view", {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token
            }
        });
        var data = await response.json();
        if (response.ok === true) {
            this.setState({ allTests: data }, () => {
                this.addPrescribedTests();
                this.addNameTestsForResults();
            });
        }
        else {
            console.log("Error: ", response.status);
        }
    }

    addTest() {
        var tmp = this.state.prescribedTests;
        this.state.allTests.map((test) => {
            if (test.id === this.state.addedTest) {
                tmp.push({ name: test.name, id: test.id, isChecked: true });
                this.setState({ prescribedTests: tmp });
                tmp = this.state.availableTests;
                for (var i = 0; i < this.state.availableTests.length; i++) {
                    if (this.state.availableTests[i].id === this.state.addedTest) {
                        if (i + 1 !== this.state.availableTests.length)
                            this.setState({ addedTest: this.state.availableTests[i + 1].id });
                        else if (this.state.availableTests.length === 2)
                            this.setState({ addedTest: this.state.availableTests[0].id });
                        else
                            this.setState({ addedTest: 0 });
                        tmp.splice(i, 1);
                        this.setState({ arePrescribedTests: "" });
                    }
                }
                this.setState({ availableTests: tmp });
            }
        });
    }

    addPrescribedTests() {
        this.setState({ availableTests: [], prescribedTests: [] }, () => {
            if (this.state.patient.tests !== null && this.state.patient.tests !== undefined && this.state.patient.tests.length !== 0) {
                this.state.allTests.map((test) => {
                    var flag = false;
                    this.state.patient.tests.map((prescribedTest) => {
                        if (prescribedTest === test.id) {
                            var tmp = this.state.prescribedTests;
                            tmp.push({ name: test.name, id: test.id, isChecked: true });
                            this.setState({ prescribedTests: tmp });
                            flag = true;
                        }
                    });
                    if (flag === false) {
                        var tmp = this.state.availableTests;
                        tmp.push({ name: test.name, id: test.id });
                        this.setState({ availableTests: tmp });
                    }
                });
            }
            else
                this.setState({ availableTests: this.state.allTests, arePrescribedTests: "Тестов пока нет!" });
        });
    }

    addNameTestsForResults() {
        var tmp = this.state.patientResults;
        if (this.state.patientResults !== null && this.state.patientResults !== undefined && this.state.patientResults.length !== 0) {
            this.setState({ areResults: "" });
            for (var i = 0; i < tmp.length; i++)
                this.state.allTests.map((test) => {
                    if (tmp[i].test === test.id)
                        tmp[i].name = test.name;
                });
            this.setState({ patientResults: tmp });
        }
        else
            this.setState({ areResults: "Ни один тест еще не пройден!" });
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

        var response = await fetch("/api/patients/" + this.state.patientId, {
            method: "PUT",
            headers: {
                "Authorization": "Bearer " + token,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                name: this.state.name,
                id: this.state.patientId,
                tests: tests,
                results: this.state.patientResults
            })
        });

        if (response.ok !== true) {
            console.log("Error: ", response.status);
        }
        else
            this.onChangeAlert(true);
    }

    async remove() {
        if (window.confirm("Вы уверены что хотите удалить этого пациента?")) {
            const token = sessionStorage.getItem('tokenKey');
            var response = await fetch("/api/patients/" + this.state.patient.id, {
                method: "DELETE",
                headers: {
                    "Authorization": "Bearer " + token,
                }
            });
            if (response.ok !== true) {
                console.log("Error: ", response.status);
            }
            else {
                this.props.history.push('/');
            }
        }
    }

    render() {
        if (this.state.isPatient === "")
            return (
                <div>
                    <Alert color="success" isOpen={this.state.alertVisible} toggle={() => { this.onChangeAlert(false) }} fade={false}>
                        Изменения успешно сохранены!
                    </Alert >
                    <Form onSubmit={this.onSubmit}>
                        <h4>Личная информация</h4>
                        <br />
                        <FormGroup>
                            <Row>
                                <Col xs="2"><Label for="name">Имя:</Label></Col>
                                <Col xs="8"><Input id="name" required value={this.state.name} onChange={this.onChangeName} placeholder="ФИО" /></Col>
                            </Row>
                        </FormGroup>

                        <Url patient={this.state.patient} />

                        <hr /><br />
                        <h4>Назначенные теcты</h4>
                        <br />
                        <FormGroup>
                            <Row>
                                <Col xs="2"><Label for="newTest">Назначить тест:</Label></Col>
                                <Col xs="8">
                                    <Input type="select" name="select" defaultValue={'0'} onChange={this.onTestChange} id="newTest">
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
                            <p>Назначенные теcты:</p>
                            <p>{this.state.arePrescribedTests}</p>
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

                        <hr /><br />
                        <h4>Пройденные тесты</h4>
                        <br />
                        <p>{this.state.areResults}</p>
                        {
                            this.state.patientResults.map((result, index) => {
                                return (
                                    <FormGroup key={index}>
                                        <strong>{result.name}</strong>
                                        <br />
                                        <>Результат: {result.result}</>
                                        <br />
                                        <Label for="comment">Комментарий:</Label>
                                        <Input type="textarea" name="text" id="comment" value={result.comment} onChange={(e) => { this.onCommentChange(e, index) }} />
                                    </FormGroup>
                                );
                                <br />
                            })
                        }
                        <br />
                        <FormGroup>
                            <div className="row">
                                <Button className="col-5" color="danger" onClick={() => this.remove()}>Удалить</Button>
                                <div className="col-2"></div>
                                <input type="submit" value="Сохранить" className="btn btn-info col-5" />
                            </div>
                        </FormGroup>
                    </Form>
                    <br /><br />
                </div>
            );
        else return (<h1>{this.state.isPatient}</h1>);
    }
}

class Url extends Component {
    constructor(props) {
        super(props);
        this.state = {
            url: ""
        };
        this.generateUrl = this.generateUrl.bind(this);
    }
    componentDidMount() {
        this.getUrl();
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevProps.patient !== this.props.patient) {
            this.getUrl();
        }
    }

    async generateUrl() {
        const token = sessionStorage.getItem('tokenKey');
        var response = await fetch("api/link/generateUrl/" + this.props.patient.id, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token
            }
        });
        var data = await response.json();
        if (response.ok === true) {
            this.setState({ url: data });
        }
        else {
            console.log("Error: ", response.status);
        }
    }

    async getUrl() {
        const token = sessionStorage.getItem('tokenKey');
        var response = await fetch("api/link/getUrl/" + this.props.patient.id, {
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authorization": "Bearer " + token
            }
        });
        var data = await response.json();
        if (response.ok === true) {
            this.setState({ url: data });
        }
        else {
            console.log("Error: ", response.status);
        }
    }

    render() {
        return (
            <div>
                <FormGroup>
                    <Row>
                        <Col xs="2"><Label for="url">Ссылка для привязки:</Label></Col>
                        <Col xs="8">
                            <InputGroup>
                                <Input readOnly id="url" value={this.state.url} />
                                <InputGroupAddon addonType="append">
                                    <Button color="secondary" outline onClick={() => { navigator.clipboard.writeText(this.state.url) }}>Копировать</Button>
                                </InputGroupAddon>
                            </InputGroup>
                        </Col>
                        <Col><Button color='info' outline onClick={() => { this.generateUrl() }}>Новая ссылка</Button></Col>
                    </Row>
                </FormGroup>
            </div>
        );
    }
}
