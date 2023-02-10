import React, { Component } from 'react';


class WizardStepHelper extends Component {
    constructor(props) {
        super(props);
        this.state = { visible: true }
    }
    render() {
        if (this.state.visible === false || this.props.helpMessage === "")
            return <div></div>

        return <div className="wm-step-info">
            <i className="fas fa-times wm-step-info-close" onClick={() => { this.setState({ visible: false }) }} aria-hidden="true"></i>
            <div className="wm-step-info-header">
                <img src={this.props.assistantImage}></img>
                <div className="wm-step-info-header-right">
                    <h3>{this.props.assistantName}</h3>
                    <h4>{this.props.position}</h4>
                </div>
            </div>
            <p>
                {this.props.helpMessage}
            </p>
        </div>
    }
}
export default WizardStepHelper;