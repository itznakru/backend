export const logMdl = store => next => action => {
    const result = next(action);
    console.log("next state:", store.getState())
    return result;
};