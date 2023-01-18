import React, { Component } from 'react';
import WizardStepHeader from "../wizard-frame/wizard-step-header.jsx"
import WizardStepHelper from "../wizard-frame/wizard-step-helper.jsx"
import {wizardStore  as store } from "../reducer"
import { setactivestage, setvaluetm, checkValueTmAction } from '../actions.js';
import "./wizard-tm.css";
import '../../partners/demo/skin_demo.css';

import BtnContinue from "../wizard-frame/wizard-btn.jsx"
import TextInput from "../wizard-frame/wizard-input.jsx"
import ImageInput from "../wizard-frame/wizard-image.jsx"


class WizardMarkType extends Component {
    constructor(props){
        super(props);
    }

    
    renderHeaderModeHtml(){
       
       return <div className='wm-step'>
                    <WizardStepHeader 
                        showEditButton={true}
                        stepKey={this.props._id} 
                        caption={this.props.caption} 
                        number={this.props.number}
                        readOnly={false} 
                        onClickEdit={()=>{
                            store.dispatch(setactivestage(this.props._id))   
                            }
                            }>
                    </WizardStepHeader>
                </div>
                        
    }

    getImageTypeFlat(tp,lbl,lbl1,key){
        // Word,Image,Combined
        let imgSrc="https://tramatm.co.uk/images/form/nike-logo.svg";
        let _tmType=0;
        switch(tp){
            case "mixt": //id=2
                imgSrc="../images/nike-logo_text.svg";
                _tmType=2;
                break;
            case "img": // id=1
                imgSrc="../images/nike-swoosh.svg";
                _tmType=1
                break;
            case "txt": // id=0
                imgSrc="../images/nike-name.svg";
                _tmType=0;
                break;         
        }   

        return <li>
                    <label className={this.props.tm.tmType==key?'wtm-item wtm-item-checked':'wtm-item'} >
                        <div className='wtm-item-content'  onClick={()=>{
                                       store.dispatch(setvaluetm({tmType:_tmType,tmValue:""}))
                                    }}>
                            <div className='wtm-item-content-img'>
                                <img src={imgSrc}></img>
                            </div>
                            <div className='wtm-item-content-text'>
                                <h3>{lbl}</h3>
                                <h4>{lbl1}</h4>
                                { <div>{this.props.tm.tmType==key}</div> }
                            </div>
                            <div className='wtm-radio-button'>
                                    <span className={this.props.tm.tmType==key?"wtm-radio wtm-checked":"wtm-radio"} >
                                    </span>
                            </div>
                        </div>
                    </label>
                </li>
    }

    getDescriptionTmInput(){
        return <div className='wtm-description' >
                    <h2>Опишите  вид деятельности которым вы занимаетесь</h2>
                    {/* <p>Кратко опишите свой вид деятельности и продукцию которую вы производите. Укажите свой сайт, если он у вас есть. Эта информация позволит нам правильно подобрать необходимый список МКТУ</p> */}
                    <TextInput 
                        multiRow={true}
                        placeholder={"Кратко опишите свой вид деятельности и продукцию которую вы производите. Укажите свой сайт, если он у вас есть. Эта информация позволит нам правильно подобрать необходимый список МКТУ"} 
                        error={this.getError("codesInfo")} 
                        value={this.sp.codesInfo} onChange={(p)=>{
                                    this.sp.codesInfo=p;                        
                        }} ></TextInput>
                </div>

    }

    getWordInput(){
        return <div className='wtm-description' >
                    <h2>Введите наименование знака</h2>
                    <div>{this.d}</div>
                    <TextInput placeholder={"Пример:NIKE"} 
                               error={store.getError('tmValue')} 
                               value={this.props.tm.tmValue} 
                    onChange={(p)=>{store.dispatch(setvaluetm({tmType:this.props.tm.tmType,tmText:p}))}} ></TextInput>    
                </div>
    }

    getImageInput(){
        return <div className='wtm-description' >
                    <h2>Загрузите изображение знака</h2>
                    <ImageInput 
                        value={this.props.tm.tmValue} 
                                onRemoveImageValue={()=>{ this.props.tm.tmValue=null}}
                                onUpload={(p)=>{
                                        store.dispatch(setvaluetm({tmType:this.props.tm.tmType,tmValue:p.fileBase64}))
                                    }} 
                        error={''}
                        ></ImageInput>
                </div>
    }

    onCommit(){
        if(this.sp.tmValue && this.sp.codesInfo){ 
            this.resetError()
            this.props.onCommit(this.sp.key);
        }else{
            this.sp.tmValue || (this.sp.errors={tmValue:"укажите значение"});
            this.sp.codesInfo || (this.sp.errors={codesInfo:"опишите вид деятельности"});            
        }


    }


    renderHtml(){
        
        let imgInput=null;
        let wordInput=this.getWordInput();
   
        if(this.props.tm.tmType==2||this.props.tm.tmType==1)
            imgInput=this.getImageInput();
            
        if(this.props.tm.tmType==1)
            wordInput = null;

        return <div className='wm-step'>
                    <WizardStepHeader 
                        showEditButton={!this.props.isOpen}
                        stepKey={this.props._id} 
                        caption={this.props.caption} 
                        number={this.props.number}
                        readOnly={false} 
                        onClickEdit={()=>{
                            //this.sp.isCheck=true;    
                            //this.props.onClickEdit(this.sp.key)
                        }
                            }>
                    </WizardStepHeader>
                
                    <WizardStepHelper 
                            helpMessage={this.props.helpMessage}
                            assistantName={this.props.assistant.name}
                            position={this.props.assistant.position}
                            assistantImage={this.props.assistant.image}
                    ></WizardStepHelper>
                    <h2>Выберите тип товарного знака</h2>
                    <div className='wtm-content-selector'>
                            <ul>
                                {this.getImageTypeFlat("mixt","Комбинированный","Логотип знака и текст",2)}
                                {this.getImageTypeFlat("img","Изобразительный","Логотип знака",1)}
                                {this.getImageTypeFlat("txt","Словесный","Наименование бренда или слоган",0)} 
                            </ul>
                    </div> 
                        {imgInput}
                        {wordInput}   
                        {}        
                        {this.props.tm.tmType==-1?null:<BtnContinue caption={'ДАЛЕЕ'} onClick={()=>{store.dispatch(checkValueTmAction)}}></BtnContinue>}   
                </div>
    }

    render() {return this.props.isOpen?this.renderHtml():this.renderHeaderModeHtml()}

}



export default WizardMarkType;