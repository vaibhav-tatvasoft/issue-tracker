import { configureStore } from "@reduxjs/toolkit";
import messageReducer from "./Slices/MessageSlice"
import UserSelectedReducer from "./Slices/UserClickedSlice"
import userDetailsReducer from './Slices/UserSlice'
import grouSettingReducer from "./Slices/GroupSettingSlice";

export const store = configureStore({
    reducer: {
        messages : messageReducer,
        userSelected : UserSelectedReducer,
        users: userDetailsReducer,
        groupSetting: grouSettingReducer
    },
    middleware: getDefaultMiddleware =>
        getDefaultMiddleware({
          serializableCheck: false,
        })
})

export default store;