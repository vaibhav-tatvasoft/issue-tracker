import { createSlice } from "@reduxjs/toolkit";

export const initialState = {
    clickedUser : {},
    prevClickedUser : {},
    clickedGroupChat : {},
    isGroupChat: false
};

export const UserSelectedSlice = createSlice({
    initialState,
    name: 'UserSelected',
    reducers: {
        setClickedUser(state, action) {
            state.clickedUser = action.payload
        },
        setPrevClickedUser(state, action){
            state.prevClickedUser = action.payload
        },
        setClickedGroupChat(state, action){
            state.clickedGroupChat = action.payload
        },
        setIsGroupChat(state, action){
            state.isGroupChat = action.payload
        },
        resetUserClickedSlice(state){
           state.clickedUser = {};
           state.prevClickedUser = {};
           state.clickedGroupChat = {};
           state.isGroupChat = false
        }
    }
});

export const { setClickedUser, setPrevClickedUser, setClickedGroupChat, setIsGroupChat, resetUserClickedSlice } = UserSelectedSlice.actions;
export default UserSelectedSlice.reducer;