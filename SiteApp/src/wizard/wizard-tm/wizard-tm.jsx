import React from 'react';
import WizardStepHeader from "../wizard-controls/wizard-step-header.jsx"
import WizardStepHelper from "../wizard-controls/wizard-step-helper.jsx"
import { wizardStore as store } from "../wizard-reducer"
import { setactivestage, setvaluetm, checkValueTmAction } from '../wizard-actions.js';
import "./wizard-tm.css";
import '../../partners/demo/skin_demo.css';

import BtnContinue from "../wizard-controls/wizard-btn"
import TextInput from "../wizard-controls/wizard-input.jsx"
import ImageInput from "../wizard-controls/wizard-image.jsx"

export default (props)=> { 

    const renderHeaderModeHtml = () => {
        return (
            <div className='wm-step'>
                <WizardStepHeader
                    showEditButton={!props.isOpen}
                    stepKey={props._id}
                    caption={props.caption}
                    number={props.number}
                    readOnly={false}
                    onClickEdit={() => {
                        store.dispatch(setactivestage(props._id))
                    }
                    }>
                </WizardStepHeader>
            </div>
        )
    }

    const getImageTypeFlat = (tp, lbl, lbl1, key) => {
        let imgSrc = "https://tramatm.co.uk/images/form/nike-logo.svg";
        let _tmType = 0;
        switch (tp) {
            case "mixt":
                imgSrc = "../images/nike-logo_text.svg";
                _tmType = 2;
                break;
            case "img":
                imgSrc = "../images/nike-swoosh.svg";
                _tmType = 1
                break;
            case "txt":
                imgSrc = "../images/nike-name.svg";
                _tmType = 0;
                break;
        }

        return (
            <li>
                <label className={props.tm.tmType === key ? 'wtm-item wtm-item-checked' : 'wtm-item'} >
                    <div className='wtm-item-content' onClick={() => {
                          store.dispatch(setvaluetm({ tmType: _tmType, tmValue: "" }))                        
                    }}>
                        <div className='wtm-item-content-img'>
                            <img src={imgSrc}></img>
                        </div>
                        <div className='wtm-item-content-text'>
                            <h3>{lbl}</h3>
                            <h4>{lbl1}</h4>
                            {<div>{props.tm.tmType === key}</div>}
                        </div>
                        <div className='wtm-radio-button'>
                            <span className={props.tm.tmType === key ? "wtm-radio wtm-checked" : "wtm-radio"} >
                            </span>
                        </div>
                    </div>
                </label>
            </li>
        )
    }

    const getDescriptionTmInput = () => {
        return <div className='wtm-description' >
            <h2>Опишите  вид деятельности которым вы занимаетесь</h2>
            <TextInput
                placeholder=''
                value={props.tmValue}
                onChange={(e) => {
                   // setTmValue(e.target.value);
                }}
                onBlur={() => {
                    store.dispatch(checkValueTmAction({ value: props.tmValue }))
                }}
            ></TextInput>
        </div>
    }

    const getWordInput=()=> {
        return <div className='wtm-description' >
            <h2>Введите наименование знака</h2>
            <TextInput placeholder={""}
                error={store.getError('tmValue')}
                value={props.tm.tmText}
                onChange={(p) => { store.dispatch(setvaluetm({ 
                        tmType: props.tm.tmType, 
                        tmValue:props.tm.tmValue,
                        tmText: p })) }} ></TextInput>
        </div>
    }

    const getImageInput=()=> {
        return <div className='wtm-description' >
            <h2>Загрузите изображение знака</h2>
            <ImageInput
                value={props.tm.tmValue}
                onRemoveImageValue={() => { props.tm.tmValue = null }}
                onUpload={(p) => {
                    store.dispatch(setvaluetm({ tmType: props.tm.tmType, tmValue: p.fileBase64 }))
                }}
                error={''}
            ></ImageInput>
        </div>
    }
   /* RETURN HTML */ 
   if(props.isOpen)    
    return (        
            <div className='wtm-step-content' >
                {renderHeaderModeHtml()}
                <WizardStepHelper
                    helpMessage={props.helpMessage}
                    assistantName={props.assistant.name}
                    position={props.assistant.position}
                    assistantImage={props.assistant.image}>
                </WizardStepHelper>

                <div className='wtm-content' >
                    <div className='wtm-type' >
                        <h2>Выберите тип товарного знака</h2>
                        <ul>
                            {getImageTypeFlat("txt", "Текстовая марка", "Название компании, логотип", 0)}
                            {getImageTypeFlat("img", "Изображение", "Логотип, символ, эмблема", 1)}
                            {getImageTypeFlat("mixt", "Комбинированная марка", "Логотип + название компании", 2)}
                        </ul>
                    </div>
                    {(props.tm.tmType == 2 || props.tm.tmType == 1) && getImageInput()}
                    { props.tm.tmType != 1 && getWordInput()}
                    {getDescriptionTmInput()}
                </div>

                <BtnContinue
                    caption='Далее'
                    onClick={() => {
                        store.dispatch(checkValueTmAction)                         
                    }}>
                </BtnContinue>
            </div>
        );
      else
        return(renderHeaderModeHtml())
       

}
