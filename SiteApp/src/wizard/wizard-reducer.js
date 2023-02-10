import {
  MEMBERINIT,
  NEXT,
  PREV,
  ONCRITICALERROR,
  SETACTIVESTAGE,
  SETVALUETM,
  ONBUSINESSERROR,
} from "./wizard-action-types";
import { createStore, applyMiddleware } from "redux";
import { logMdl as log } from "../tools-middleware/log-mdl";
import thunk from "redux-thunk";

const defaultState = {
  error: undefined,
  currentStageId: 0,
  stages: [],
  tm: { tmType: 2, tmValue: "", tmText: "" }, // 0-txt 1-img 2-mixt
};

const wizardReducer = (state = defaultState, action) => {
  const getNextStage = (isForvard) => {
    let idx = state.stages.findIndex((_) => {
      return _._id === state.currentStageId;
    });
    let isNoEnd = state.stages.length - 1 > idx;

    if (isForvard) {
      if (isNoEnd) return state.stages[idx + 1]._id;
    } else {
      if (idx > 0) return state.stages[idx - 1]._id;
    }

    return state.currentStageId;
  };

  switch (action.type) {
    case NEXT:
      return { ...state, error: undefined, currentStageId: getNextStage(true) };

    case PREV:
      if (state.currentStageId > 0)
        return {
          ...state,
          error: undefined,
          currentStageId: getNextStage(false),
        };

    case MEMBERINIT: {
      return {
        ...state,
        error: undefined,
        stages: action.payload.stages,
        assistant: action.payload.assistant,
      };
    }
    // payload equals _id of stage
    case SETACTIVESTAGE:
      return { ...state, error: undefined, currentStageId: action.payload };

    case SETVALUETM:
      return { ...state, error: undefined, tm: action.payload };

    case ONCRITICALERROR:
      return defaultState;

    case ONBUSINESSERROR:
      return { ...state, error: action.payload };
  }
  return state;
};

const wizardStore = createStore(wizardReducer, applyMiddleware(thunk, log));
wizardStore.getError = function (field) {
  if (this.getState().error && this.getState().error[field])
    return this.getState().error[field];
}.bind(wizardStore);

window.store = wizardStore;

export { wizardStore, wizardReducer };
