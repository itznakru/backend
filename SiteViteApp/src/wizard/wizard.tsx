import React, { useState, useEffect } from "react";
import "./wizard.css";

import {store}  from "./wizard-store";
import { Provider } from "react-redux";

// import WizardTm from '../wizard-tm/wizard-tm.jsx';
// import WizardOrg from '../wizard-org/wizard-org.jsx';
//import { wizardStore as store, wizardReducer as rootReducer } from "../wizard-reducer"
//import { memberInitAction } from "../wizard-actions";

/*  FRAME OF WIZARD. 
    PROPOUSE - BULD STAGES ON DEPENDENCY OF LIST OF STAGES FROM SERVER */
export default () => {
  // const [flag, setFlag] = useState(new Date())
  // useEffect(() => {
  //     store.subscribe((p) => {setFlag(new Date())})// subscribe on store
  //     store.dispatch(memberInitAction);// init store
  //     store.getError();
  // }, [])

  //let _store = store.getState();
  const btnClose = (
    <div className="wm-container-close-panel">
      <i className="fas fa-times-circle" onClick={() => alert("go home")}></i>
    </div>
  );

  // /* builder functions */
  // const controlsMap = {
  //     /* first stage. Input form for  */
  //     _id0: (p) => {
  //         return
  //     },
  //     _id1: (p) => {
  //         return <WizardOrg
  //                     _id={p._id} // id of stage
  //                     isOpen={p._id === _store.currentStageId} // stage in state open if stageid==currentStageId
  //                     key={p._id}
  //                     caption={p.name}
  //                     number={p.number}
  //                     helpMessage={p.helpText}
  //                     assistant={_store.assistant}
  //                 >

  //                 </WizardOrg>
  //             },
  //     _id2: (p) => { return <div key={p._id}>{p.name} 2</div> },
  //     _id3: (p) => { return <div key={p._id}>{p.name} 3</div> },
  //     _id4: (p) => { return <div key={p._id}>{p.name} 4</div> },

  // }
  // /* walk by array of element constructions and to create form */
  // let stages = _store.stages.map(p => {
  //     let map = controlsMap["_id" + p._id];
  //     return map ? map(p) : <div key={p._id}>{p.name}</div>;
  // });

  return (
    <div className="wizard">
      <Provider store={store}>
        <div className="wizard-container">
          {btnClose}
          <div>stages</div>
        </div>
      </Provider>
    </div>
  );
};
