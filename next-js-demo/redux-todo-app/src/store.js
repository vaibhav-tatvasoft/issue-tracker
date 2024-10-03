import { configureStore } from "@reduxjs/toolkit";
import createSagaMiddleware from "redux-saga";
import usersReducer from "./reducers/userSlice";
import usersSagaReducer from "./actions/userSaga"; 
import { watchFetchUsersSaga } from "./actions/userSaga";

const sagaMiddleware = createSagaMiddleware();

const store = configureStore({
  reducer: {
    users: usersReducer,
    usersSaga: usersSagaReducer
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(sagaMiddleware),
});

sagaMiddleware.run(watchFetchUsersSaga);

export default store;
