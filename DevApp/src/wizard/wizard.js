import React, { useState, useEffect } from "react";
import { Provider } from "react-redux";
import { next,prev,memberInitAction } from "./actions";
import WizardFrame from'./wizard-frame/wizard-frame' 
import {wizardStore  as store , wizardReducer as rootReducer} from "./reducer"


export default ()=>
{
    const [flag,setFlag]=useState(new Date())    
    useEffect(()=>{
               store.subscribe((p)=>{setFlag(new Date()) })// subscribe on store
               store.dispatch(memberInitAction);// init store     
               store.getError();       
        },[])

    const currentStep=store.getState().currentStep
   
    return <div>
                <WizardFrame></WizardFrame>      
           </div>
}