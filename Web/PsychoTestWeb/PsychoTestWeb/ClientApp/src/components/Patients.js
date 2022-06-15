import React, { Component } from 'react';
import { Button, Row, Col, Input, InputGroup, InputGroupAddon } from 'reactstrap';
import ModalPatient from './ModalPatient';
import { useHistory } from "react-router-dom";


export class Patients extends Component {
    static displayName = Patients.name;

    constructor(props) {
        super(props);
        this.state = {
            patients: [],
            searchString: "",
            emptyPatient: { name: "", tests: [], id: "", results: [] }
        };
        this.getPatients = this.getPatients.bind(this);
        this.onSearchStringChange = this.onSearchStringChange.bind(this);
    }

    componentDidMount() {
        this.getPatients("/api/patients/");
        this.setState({ searchString: "" });
    }

    onSearchStringChange(e) {
        this.setState({ searchString: e.target.value });
    }

    async getPatients(url) {
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
            this.setState({ patients: [] });
            this.setState({ patients: data });
        }
        else {
            console.log("Error: ", response.status);
        }
    }

    render() {
        return (
            <div>
                <h2>Список пациентов:</h2>
                <br />
                <Row>
                    <Col xs="1"><strong>Поиск</strong></Col>
                    <Col xs="5">
                        <InputGroup>
                            <Input value={this.state.searchString} onChange={this.onSearchStringChange} />
                            <InputGroupAddon addonType="append">
                                <Button color="secondary" outline onClick={() => { this.getPatients("api/patients/"); this.setState({ searchString: "" }) }}>&#215;</Button>
                            </InputGroupAddon>
                        </InputGroup>
                    </Col>
                    <Col xs="3"><Button color="info" onClick={() => {
                        if (this.state.searchString !== "")
                            this.getPatients("api/patients/name/" + this.state.searchString);
                        else this.getPatients("api/patients/");
                    }}>Найти</Button></Col>
                    <Col xs="auto"><ModalPatient patient={this.state.emptyPatient} isCreate={true} onClose={this.getPatients} /></Col>
                </Row>
                <br />
                <hr />
                <div>
                    {
                        this.state.patients.map((patient) => {
                            return <Patient patient={patient} getPatients={this.getPatients} key={patient.id} />
                        })
                    }
                </div>
            </div>
        );
    }
}

class Patient extends Component {
    static displayName = Patient.name;

    constructor(props) {
        super(props);
        this.state = {
            patient: props.patient
        };
    }

    render() {
        return (
            <div>
                <Row>
                    <Col xs="6">{this.state.patient.name}</Col>
                    {/*    <Col xs="auto"><ModalPatient patient={this.state.patient} isCreate={false} onClose={this.props.getPatients} /></Col>*/}
                    <Col xs="auto"><ViewPatient patientId={this.state.patient.id} /></Col>
                    <Col xs="auto"></Col>
                </Row>
                <br />
            </div>
        );
    }
}

const ViewPatient = (patientId) => {
    let history = useHistory();

    const goToPatient = () => {
        history.push("/patient/" + patientId.patientId);
    };
    return (
        <Button color="info" onClick={goToPatient}>Просмотр</Button>
    );
};

