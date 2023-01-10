import React, { useState, useEffect } from "react";
import './wizard-frame.css';
import WizardTm from '../wizard-tm/wizard-tm.jsx';
import {wizardStore  as store , wizardReducer as rootReducer} from "../reducer"

export default (props)=>
{
    let dd=store.getState();
    const btnClose= <div className='wm-container-close-panel'><i className="fas fa-times-circle" onClick={()=>alert('go home')}></i></div>
    const controlsMap={
        _id0:(p)=>{return <WizardTm isOpen={p._id===dd.currentStageId} 
                            key={p._id} 
                            caption={p.name} 
                            _id={p._id} 
                            number={p.number}
                            helpMessage={p.helpText}
                            assistant={dd.assistant}
                            tm={dd.tm}></WizardTm>},
        _id1:(p)=>{return <div  key={p._id}>{p.name}</div>},          
                          
       
    }
     
     let stages=dd.stages.map(p=>{ 
        let b=controlsMap["_id"+p._id];
        return b ? b(p):<div key={p._id}>{p.name}</div> ;         
     });

     return <div className="wm-page">
                <div className="wm-container">
                    {btnClose}
                    {stages}
                </div>
            </div>
}