import {MEMBERINIT,NEXT,PREV, SETACTIVESTAGE,SETVALUETM,ONBUSINESSERROR } from './action-types'
import {FetchGet, FetchPost} from './../tools-hooks/use-request'


// goto next currentstep
export const  next=()=> {
    return {
      type: NEXT
    }
}

// goto prev currentstep
  export const prev=()=>{
    return {
      type: PREV
    }
  }

  // goto prev currentstep p={tmType:0,tmValue:''}
  export const setvaluetm=(p)=>{
    return {
      type: SETVALUETM,
      payload:p
    }
  }

  export const setactivestage=(p)=>{
    return {
      type: SETACTIVESTAGE,
      payload:p
    }
  }

  export const memberInitAction=(dispatch,getState)=>{
     FetchGet('member','getwizardsettings',null,
          (p)=>{
                dispatch({type:MEMBERINIT,payload:p})
              },
          (e)=>{alert('ERR'+ JSON.stringify(e))});
  }
  
  export const checkValueTmAction=(dispatch,getState)=>{    
    FetchPost('wizard','checkvaluetm',getState().tm,
          (p)=>{  
              dispatch(next())             
            },
          (e)=>{
              dispatch({type:ONBUSINESSERROR,payload:e})
          });
     
 }