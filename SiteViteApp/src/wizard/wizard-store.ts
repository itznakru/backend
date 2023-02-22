import { configureStore } from "@reduxjs/toolkit";
import { createLogger } from "redux-logger";
import  {pokemonApi } from "./wizard-api";

const logger = createLogger({
  // ...options
  //
});
export const store = configureStore({
  reducer: {
    // Add the generated reducer as a specific top-level slice
    [pokemonApi.reducerPath]: pokemonApi.reducer,
  },
  // Adding the api middleware enables caching, invalidation, polling,
  // and other useful features of `rtk-query`.
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(pokemonApi.middleware),
})
