import React, { Component } from 'react';
import "./buttons.css"
import ToolTipsBus from "../../controls/tooltips/tool-tips-bus.js"

//
class RoundBtnSecondary extends Component {
    render() {
        let {caption,addStyle,inlineBtnLeft,tips,cb,showInlineBtn,btnSkin,readOnly}=this.props;
        if(!caption || caption.length === 0)
            return <div></div>
        addStyle=addStyle||{maxWidth:"150px"}
        
        if(readOnly)
            addStyle["opacity"]="0.6";
        
        let inlineBtn=null;  
        if(showInlineBtn)
        inlineBtn=<i key="weree" className={btnSkin}></i>;

        let divCaption = <div key="ddda">{caption}</div>;
        let content= inlineBtnLeft?[inlineBtn,divCaption]:[divCaption,inlineBtn];     
        let css=this.props.on?"rl-btn":"rl-btn rl-btn-off"
        css=css + " "+this.props.addCss;
        return (<div 
                     key={caption} 
                     style={addStyle}
                     className={css}
                     onMouseOver={(p)=>{
                        tips && ToolTipsBus.showXY(p,tips)}}                        
                     onMouseOut={()=>{
                        tips && ToolTipsBus.hide()}} 
                     onClick={()=>{ToolTipsBus.hide(); cb && cb()}} >
                     {content}    
                </div>
        );
    }
}
RoundBtnSecondary.defaultProps ={
    addStyle:undefined, // размер кнопки
    addCss:"",
    caption:"", // текст
    tips:"",
    on:true, // во включенном состоянии  фон уходит - остается рамка
    inlineBtnLeft:false, // с какой стороны значок 
    showInlineBtn:true, //скрывать значок
    btnSkin:"fas fa-plus-circle", // тема значка по умолчанию 
    cb:null, // коллбэк на нажатие кнопки
};

export default RoundBtnSecondary;