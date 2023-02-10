import React from 'react';
import WizardStepHeader from "../wizard-controls/wizard-step-header.jsx"
import WizardStepHelper from "../wizard-controls/wizard-step-helper.jsx"
import { wizardStore as store } from "../wizard-reducer"
import { setactivestage, setvaluetm, checkValueTmAction } from '../wizard-actions.js';
import '../../partners/demo/skin_demo.css';
// import FaceList from "../../controls/face-list/face-list.jsx"

export default ({_id,isOpen,caption,number,helpMessage,assistant})=> { 


    const renderHeaderModeHtml = () => {
        return (
            <div className='wm-step'>
                <WizardStepHeader
                    showEditButton={true}
                    stepKey={_id}
                    caption={caption}
                    number={number}
                    readOnly={false}
                    onClickEdit={() => { store.dispatch(setactivestage(_id))}}>
                </WizardStepHeader>
            </div>
        )
    }

    if(isOpen)
        return <div>
                {renderHeaderModeHtml()}
                {/* <FaceList></FaceList> */}
        </div>   
    else
        return <div>{renderHeaderModeHtml()}</div>
}