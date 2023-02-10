import React, { Component } from 'react';
import BtnSecondary from "../../controls/buttons/round-btn-secondary.jsx"


class WizardStepHeader extends Component {
    constructor(props) {
        super(props);
    }
    render() {
        let commonProps = { btnSkin: "fas fa-check", addStyle: { d: "" } };
        let btnPrev = <BtnSecondary
            {...commonProps}
            on={this.props.readOnly}
            readOnly={this.props.readOnly}
            showInlineBtn={true}
            inlineBtnLeft={true}
            btnSkin=""
            cb={() => { this.props.onClickEdit() }}
            caption="Редактировать">
        </BtnSecondary>

        if (this.props.readOnly)
            btnPrev = null;

        return (<div className="wm-step-header">
            <div className="wm-step-header-left">
                <div className="wm-step-number"><span className="wm-step-number-text">{this.props.number}</span></div>
                <h1>{this.props.caption}</h1>
            </div>
            <div className="wm-step-header-right">
                {this.props.showEditButton ? btnPrev : null}
            </div>
        </div>)
    }
}
export default WizardStepHeader;