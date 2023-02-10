import React, { Component } from 'react';
import "./wizard-controls.css"

class WizardBtn extends Component {
    constructor(props) {
        super(props);
    }
    render() {
        return <div className='wm-btn' onClick={() => { this.props.onClick && this.props.onClick() }}>{this.props.caption || "ДАЛЕЕ"}</div>
    }
}
export default WizardBtn;